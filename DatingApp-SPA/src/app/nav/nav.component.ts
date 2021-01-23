import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { PresenceService } from '../_services/presence.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}; // promenjiva model tipa any koja ce u sebi imati Username and Pass
  photoUrl: string;
  constructor(public authService: AuthService, private presencs: PresenceService,
     private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
    // console.log(this.model);
    this.authService.loging(this.model).subscribe(next => {
      this.alertify.success('Logg succes');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }
  loggedIN() {
      //  const token = localStorage.getItem('token');
      // return !!token;
     return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodeToken = null;
    this.authService.curentUser = null;
    this.alertify.message('LOGGED OUT');
    this.router.navigate(['/home']);
    this.authService.stopHub();
  }
}
