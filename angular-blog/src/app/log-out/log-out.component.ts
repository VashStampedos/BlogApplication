import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-log-out',
  templateUrl: './log-out.component.html',
  styleUrls: ['./log-out.component.css']
})
export class LogOutComponent {
  constructor(private authService: AuthService, private router: Router) {
   
}

ngOnInit(){
  console.log("log out created")
  this.signout();
}

public signout() {
    
    console.log('doc cok ' + document.cookie);
    this.authService.logOut().subscribe(x=> {window.localStorage.clear();} );
    this.router.navigateByUrl("/blogs");
    //document.location.reload();
    //document.location.replace("/blogs");
}
}
