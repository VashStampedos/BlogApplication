import { Component } from '@angular/core';
import { Blog } from './models/blog';
import { BlogService } from './blog.service';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  
  isSignedIn:boolean = false;
  constructor(private authService:AuthService){

  }

  ngOnInit(){
    this.authService.isSignedIn().subscribe(x=> this.isSignedIn = x);
  }


  
}
