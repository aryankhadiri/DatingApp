import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  userLoggedIn = false;
  constructor(private accountService:AccountService) { }

  ngOnInit(): void {
   this.userExists();
  }
  registerToggle(){
    this.registerMode = !this.registerMode;
  }
 cancelRegisterMode(event:boolean){
  this.registerMode = event;
 }
 userExists(){
  this.accountService.currentUser$.pipe(map(user=>{
    if (user){
      this.userLoggedIn=true;
    }
    else{
      this.userLoggedIn=false;
    }
  })).subscribe();
 }
}
