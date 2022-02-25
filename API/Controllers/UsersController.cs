using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace API.Controllers


{
    
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            var users = _context.Users.ToListAsync();
            return await users;
        }

        [HttpGet("{id}")]
        [Authorize]

        public async Task<ActionResult<AppUser>> GetUser(int id){
            var user = _context.Users.FindAsync(id);
            return await user;
        }

    }
}