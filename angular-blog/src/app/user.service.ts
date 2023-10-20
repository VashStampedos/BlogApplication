import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { User } from './models/user';
import { UserResponse } from './UserClaim';
import { ApiResult } from './ApiResult';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http:HttpClient, private authService: AuthService){

  }

  getUserInfo(id:number):Observable<ApiResult<UserResponse>>{
    return this.http.get<ApiResult<UserResponse>>(`https://localhost:7018/User/GetUser?id=${id}`);
  }
  getCurrentUser():Observable<ApiResult<User>>{
    return this.http.get<ApiResult<User>>(`https://localhost:7018/User/GetCurrentUser`);
  }
  subscribe(id:number):Observable<ApiResult<UserResponse>>{
    return this.http.post<ApiResult<UserResponse>>(`https://localhost:7018/User/Subscribe`, {id:id});
  }
  unSubscribe(id:number):Observable<ApiResult<UserResponse>>{
    return this.http.post<ApiResult<UserResponse>>(`https://localhost:7018/User/UnSubscribe`, {id:id});
  }
}
