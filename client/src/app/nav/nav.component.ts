import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../models/user';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any = {}
  constructor(public accountService:AccountService, private toast:ToastrService) { 
    
  }

  ngOnInit(): void {
  }
  login(){
    this.accountService.login(this.model).subscribe(response=>{
      console.log(response);
    }, error => {
      console.log(error);
      this.toast.error(error.error);
    });
  }
  logout(){
    this.accountService.logout();
  }
 
}
