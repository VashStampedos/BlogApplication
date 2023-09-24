import { Component } from '@angular/core';
import { UserClaim } from '../UserClaim';
import { AuthService } from '../auth.service';
import { BlogService } from '../blog.service';
import { UserService } from '../user.service';
import { User } from '../models/user';
import { ActivatedRoute } from '@angular/router';
import { Blog } from '../models/blog';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  user?:User;
  userBlogs?:Blog[];
  subscribers?:User[] = [];
  subscribes?:User[]= [];
  isSubscribe:boolean = false;
  state:number=1;
  switcher:Switcher;

  constructor(private userService:UserService, private blogService:BlogService,private route:ActivatedRoute) {
    this.switcher = Switcher.Blogs;
  }

  ngOnInit(){
    this.getUser();
    
  }
  switch(event:any){
    const id = event.target.id;
    this.state = id;
    console.log(this.state);
  }
  getUser() {
    let id = Number(this.route.snapshot.paramMap.get('id'));
    this.userService.getUserInfo(id).subscribe(x=> 
      {
        this.user = x.userModel; 
        this.isSubscribe = x.isSubscribe;
       
      });
    this.blogService.getUserBlogs(id).subscribe(x=> {this.userBlogs = x});
  }

  subscribe(){
    this.userService.subscribe(this.user?.id!).subscribe(x=> 
      {
        this.user = x.userModel; 
        this.isSubscribe = x.isSubscribe;
        console.log("in subscribe id= " +  this.isSubscribe)
      });
  }
  unSubscribe(){
    this.userService.unSubscribe(this.user?.id!).subscribe(x=> 
      {
        this.user = x.userModel; 
        this.isSubscribe = x.isSubscribe;
        console.log("in unsubscribe id= " +  this.isSubscribe)
        
      });
  }
  

}
export enum Switcher{
  Blogs,
  Subscribers
}

