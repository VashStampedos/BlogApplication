import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserClaim } from '../UserClaim';
import { AuthService } from '../auth.service';

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

  constructor(private authService: AuthService,private formBuilder:FormBuilder, private router:Router ){
    // this.authService.isSignedIn().subscribe(isSignedIn => {this.signedIn = isSignedIn})
    // console.log(`${this.signedIn} signin result`)
    
  }
  ngOnInit(){
    this.authFailed = false;
    this.loginForm = this.formBuilder.group(
      {
        username:['', [Validators.required]],
        email:['', [Validators.required, Validators.email]],//разобратсья как вндеряется формбилдер и дописать методы!!!
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
    console.log(`form auth comp ${email} - ${password}`)
    this.authService.register(username, email, password).subscribe(
      {
        next:(response:any)=>{
          console.log("in next block")
          console.log(response)
          this.responseString = response;
        },
        error:(err:any)=>{
          if(!err?.error?.isSuccess){
            this.authFailed = true
            console.log("in error block of register-comp " + username, email, password)
            console.log(err)
        }
        },
        complete:()=>{
          if(!this.authFailed)
          console.log("authfaild:"+ this.authFailed)
            
            //this.router.navigateByUrl("/blogs")
          }

        
      }
      
    )
    
  }
}
