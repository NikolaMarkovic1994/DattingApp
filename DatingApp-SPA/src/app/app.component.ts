
import { Component , OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './_model/user';
import { UserTest } from './_model/userTest';
import { AuthService } from './_services/auth.service';
import { PresenceService } from './_services/presence.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  jwtHelper = new JwtHelperService();
  model: any = {}; // promenjiva model tipa any koja ce u sebi imati Username and Pass
   userTest: UserTest;
  constructor(public authService: AuthService, private presencs: PresenceService) { }

  ngOnInit() {
    const token = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));
      if (token) {
      this.authService.decodeToken = this.jwtHelper.decodeToken(token);

    }
    if (user) {
      this.authService.curentUser = user;
      this.authService.changeMemberPhoto(user.photoUrl);
     this.presencs.createHubConnection(user);
    }

  }

}
