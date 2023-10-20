import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { UserClaim, Response } from '../UserClaim';
import { of, pipe } from 'rxjs';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { ApiResult } from '../ApiResult';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {
  loginForm!: FormGroup;
  authFailed: boolean = false;
  signedIn:boolean = false;
  claims:UserClaim[]=[];

  constructor(private authService: AuthService,private formBuilder:FormBuilder, private router:Router ){
    this.authService.isSignedIn().subscribe(isSignedIn => {this.signedIn = isSignedIn})
    console.log(`${this.signedIn} signin result`)
    
  }
  ngOnInit(){
    this.authFailed = false;
    this.loginForm = this.formBuilder.group(
      {
        email:['', [Validators.required, Validators.email]],//разобратсья как вндеряется формбилдер и дописать методы!!!
        password:['', Validators.required]
      }
    )
  }

  public logIn(event:any){
    if(!this.loginForm.valid){
      return;
    }
    const email = this.loginForm.get('email')?.value;
    const password = this.loginForm.get('password')?.value;
    console.log(`form auth comp ${email} - ${password}`)
    this.authService.logIn(email,password).subscribe(
      {
        next:(response:any)=>{
          window.document.location.reload();
        },
        error:(err:any)=>{
          if(!err?.error?.isSuccess){
            this.authFailed = true
            console.log(err)
            console.log("in error block of auth-comp " + email, password)
        }
        }
      }
      
    )
    this.router.navigateByUrl("/home")
  }
}
