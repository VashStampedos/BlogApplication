import { Component } from '@angular/core';
import { UserClaim } from '../UserClaim';
import { AuthService } from '../auth.service';
import { BlogService } from '../blog.service';
import { UserService } from '../user.service';
import { User } from '../models/user';
import { ActivatedRoute, Params } from '@angular/router';
import { Blog } from '../models/blog';
import { first, map, Subscription } from 'rxjs';

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
  id:number = 0;
  switcher:Switcher;
  routeSubscription?:Subscription;

  constructor(private userService:UserService, private blogService:BlogService,private route:ActivatedRoute) {
    this.switcher = Switcher.Blogs;
  }

  ngOnInit(){
    const routeSubscription = this.route.params.subscribe((x:any) =>{
      
      this.id = x.id;
      this.getUser();

      
      
    });
    
  }
  ngOnDestroy() { this.routeSubscription?.unsubscribe(); }
  switch(event:any){
    const idTarget = event.target.id;
    this.state = idTarget;
    console.log(this.state);
  }
  getUser() {
    
    this.userService.getUserInfo(this.id).subscribe(x=> 
      {
        this.user = x.data.userModel; 
        this.isSubscribe = x.data.isSubscribe;
       
      });
    this.blogService.getUserBlogs(this.id).subscribe(x=> {this.userBlogs = x.data});
  }

  subscribe(){
    this.userService.subscribe(this.user?.id!).subscribe(x=> 
      {
        this.user = x.data.userModel; 
        this.isSubscribe = x.data.isSubscribe;
        console.log("in subscribe id= " +  this.isSubscribe)
      });
  }
  unSubscribe(){
    this.userService.unSubscribe(this.user?.id!).subscribe(x=> 
      {
        this.user = x.data.userModel; 
        this.isSubscribe = x.data.isSubscribe;
        console.log("in unsubscribe id= " +  this.isSubscribe)
        
      });
  }
  

}
export enum Switcher{
  Blogs,
  Subscribers
}

