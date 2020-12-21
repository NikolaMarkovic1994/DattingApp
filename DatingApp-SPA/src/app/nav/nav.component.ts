import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}; // promenjiva model tipa any koja ce u sebi imati Username and Pass

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
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
    this.alertify.message('LOGGED OUT');
    this.router.navigate(['/home']);
}
}
