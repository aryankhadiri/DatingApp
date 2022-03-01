import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { User } from '../models/user';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = "https://localhost:5001/api";
  private currentUserSource = new ReplaySubject<User>(1); // stores the value here and anytime someone subscribes to this observable, they will get the value in it. 1 is the size
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http:HttpClient) {
   }

   login(model:any){
     return this.http.post(this.baseUrl+"/account/login", model).pipe(
       map((response:User) => {
         const user = response;
         if (user){
           localStorage.setItem('user', JSON.stringify(user));
           this.currentUserSource.next(user);
         }
       })
     );
   }
   setCurrentUser(user){
     this.currentUserSource.next(user);
   }
   logout(){
     localStorage.removeItem('user');
     this.currentUserSource.next(null);
   }

   registerUser(user:any){
     return this.http.post(this.baseUrl+"/account/register", user).pipe(
       map((response:User)=>{
         const user = response;
         if(user){
           localStorage.setItem('user', JSON.stringify(user));
           this.currentUserSource.next(user);
         }
       return user;})
     );
  }
}
