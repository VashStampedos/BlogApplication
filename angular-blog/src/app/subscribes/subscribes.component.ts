import { Component, Input } from '@angular/core';
import { UserService } from '../user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../models/user';
import { Subscribe } from '../models/subscribe';

@Component({
  selector: 'app-subscribes',
  templateUrl: './subscribes.component.html',
  styleUrls: ['./subscribes.component.css']
})
export class SubscribesComponent {
  @Input() user?:User;
  @Input() subscribes?:Subscribe[]=[];
  @Input() showSubscribes?:Subscribe[] = [];
  userName:string = "";

  constructor(private userService:UserService, private route:ActivatedRoute, private router:Router) {
   
    
  }
  

  searching(){
    console.log(this.subscribes)
    console.log(this.showSubscribes)
   
    if(this.userName==""){
      this.showSubscribes = this.subscribes
    }
    else{
      this.showSubscribes = this.subscribes?.filter( x=> x.user?.userName!.toLowerCase().includes(this.userName.toLowerCase()));
      
    }
  }
}
