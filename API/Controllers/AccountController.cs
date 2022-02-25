using API.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Interfaces;

namespace API.Controllers

{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService){
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto){
            if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken."); //when we are using actionResult we can return http requests. 400 is BadRequest
            
            using var hmac = new HMACSHA512(); //anytime we use using with a class, it will make sure to be disposed once we don't need it anymore
            var user = new AppUser{
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); 
            return new UserDTO{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO){
            var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == loginDTO.Username);
            if (user == null){return Unauthorized("Invalid Username");}
            
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));
            for (int i = 0; i < computedHash.Length; i++){
                if (computedHash[i] != user.PasswordHash[i]){
                    return Unauthorized("Invalid Password");
                }
            }
            return new UserDTO{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        private async Task<bool> UserExists(string userName){
            bool doesExist = await _context.Users.AnyAsync(user => user.UserName == userName.ToLower());
            return doesExist;
        }
    }
    
}