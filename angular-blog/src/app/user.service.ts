import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { User } from './models/user';
import { UserResponse } from './UserClaim';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http:HttpClient, private authService: AuthService){

  }

  getUserInfo(id:number):Observable<UserResponse>{
    return this.http.get<UserResponse>(`https://localhost:7018/User/GetUser?id=${id}`);
  }
  getCurrentUser():Observable<User>{
    return this.http.get<User>(`https://localhost:7018/User/GetCurrentUser`);
  }
  subscribe(id:number):Observable<UserResponse>{
    return this.http.post<UserResponse>(`https://localhost:7018/User/Subscribe`, {id});
  }
  unSubscribe(id:number):Observable<UserResponse>{
    return this.http.post<UserResponse>(`https://localhost:7018/User/UnSubscribe`, {id});
  }
}
