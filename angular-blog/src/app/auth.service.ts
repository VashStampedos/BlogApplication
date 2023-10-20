import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Response, UserClaim } from './UserClaim';
import { Observable, catchError, map, of } from 'rxjs';
import { User } from './models/user';
import { ApiResult } from './ApiResult';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  constructor(private http: HttpClient ) { 

  }

  public logIn(email:string, password:string){
    
    console.log(`form auth service ${email} - ${password}`)
    return this.http.post<ApiResult<Response>>('https://localhost:7018/Account/Login',{email:email, password:password}) // проверить потом можно ли без тип Response
  }

  public register(username:string, email:string, password:string, returnUrl:string){
    
    console.log(`form auth service ${email} - ${password}`)
    return this.http.post<ApiResult<Response>>('https://localhost:7018/Account/Register',{username:username ,email:email, password:password, returnUrl:returnUrl}) // проверить потом можно ли без тип Response
  }

  public logOut(){
    return this.http.get('https://localhost:7018/Account/logout')
  }
  
  public user() {
    console.log("cookie is " + document.cookie)
    
    return this.http.get<ApiResult<UserClaim[]>>('https://localhost:7018/Account/GetUserClaims');
  } 

   
  public isSignedIn(): Observable<boolean> {
    return this.user().pipe(
      map((userClaims)=>{
        //console.log(userClaims);
        const hasClaims = userClaims.data.length>0;
        console.log("has claims " + hasClaims);
        return !hasClaims ? false: true;
      }),
      catchError((error) => {
        console.log("error from isSignedIn")
        return of(false);
      }));
    
  }
}
