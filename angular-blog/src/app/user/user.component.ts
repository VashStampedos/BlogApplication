import { Component } from '@angular/core';
import { UserClaim } from '../UserClaim';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  userClaims: UserClaim[]=[];
  /**
   *
   */
  constructor(private authService:AuthService) {
    this.getUser();
  }
  getUser() {
    this.authService.user().subscribe(
        result => {
            this.userClaims = result;
        });
}
}
