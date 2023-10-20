import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserClaim } from '../UserClaim';
import { AuthService } from '../auth.service';
import { ApiResult } from '../ApiResult';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  loginForm!: FormGroup;
  authFailed: boolean = false;
  signedIn:boolean = false;
  claims:UserClaim[]=[];
  responseString?:string;
  returnUrl?:string;

  constructor(private authService: AuthService,private formBuilder:FormBuilder, private router:Router ){
    
    
  }
  ngOnInit(){
    this.authFailed = false;
    this.loginForm = this.formBuilder.group(
      {
        username:['', [Validators.required]],
        email:['', [Validators.required, Validators.email]],
        password:['', Validators.required]
      }
    )
  }

  public register(event:any){
    if(!this.loginForm.valid){
      return;
    }
    const username = this.loginForm.get('username')?.value;
    const email = this.loginForm.get('email')?.value;
    const password = this.loginForm.get('password')?.value;
    const returnUrl = document.location.protocol + document.location.host + "";
    console.log(`form auth comp ${email} - ${password}`)
    this.authService.register(username, email, password, returnUrl).subscribe(
      {
        next:(response:any)=>{
          console.log(response)
          this.responseString = response.data;
        },
        error:(err:any)=>{
          if(!err?.error?.isSuccess){
            this.authFailed = true
            console.log(err.errors)
        }
        },
        complete:()=>{
          if(!this.authFailed)
          console.log("authfaild:"+ this.authFailed)
          }

        
      }
      
    )
    
  }
}
