import { Component } from '@angular/core';
import { BlogService } from '../blog.service';
import { User } from '../models/user';
import { UserService } from '../user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {
  currentUser?:User;
  constructor(private userService:UserService, private blogService:BlogService) {
    
  }

  ngOnInit(){
    
    this.getUser();
  }

  getUser() {
    this.userService.getCurrentUser().subscribe(x => this.currentUser = x);
  }
}
