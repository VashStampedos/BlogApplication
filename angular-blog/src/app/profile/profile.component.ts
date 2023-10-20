import { Component } from '@angular/core';
import { BlogService } from '../blog.service';
import { User } from '../models/user';
import { UserService } from '../user.service';
import { Switcher } from '../user/user.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {
  currentUser?:User;
  state:number=1;
  switcher?:Switcher;

  constructor(private userService:UserService, private blogService:BlogService) {
    
  }

  ngOnInit(){
    
    this.getUser();
  }

  getUser() {
    this.userService.getCurrentUser().subscribe(x => this.currentUser = x.data);
  }
  switch(event:any){
    const id = event.target.id;
    this.state = id;
    console.log(this.state);
  }
}
