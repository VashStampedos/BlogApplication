import { Component, Input } from '@angular/core';
import { UserService } from '../user.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { User } from '../models/user';
import { Subscribe } from '../models/subscribe';

@Component({
  selector: 'app-subscribers',
  templateUrl: './subscribers.component.html',
  styleUrls: ['./subscribers.component.css']
})
export class SubscribersComponent {

  @Input() subscribers?:Subscribe[] = [];
  @Input() showSubscribers?:Subscribe[] = [];
  userName:string = "";
  


  constructor(private userService:UserService, private route:ActivatedRoute, private router:Router){
    
  }



  redirectToUser(id:number){
    this.router.navigateByUrl(`/user/${id}`, {skipLocationChange:false})
  }

  searching(){
    console.log(this.subscribers)
    console.log(this.showSubscribers)
   
    if(this.userName==""){
      this.showSubscribers = this.subscribers
    }
    else{
      this.showSubscribers = this.subscribers?.filter( x=> x.subscriber?.userName!.toLowerCase().includes(this.userName.toLowerCase()));
      
    }
  }
}

