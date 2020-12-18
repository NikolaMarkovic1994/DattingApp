import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}; // promenjiva model tipa any koja ce u sebi imati Username and Pass

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login(){
    //console.log(this.model);
    this.authService.loging(this.model).subscribe(next =>{
      console.log('Logg succes');
    },error => {
      console.log(error);
    });
  }
  loggedIN(){
      const token = localStorage.getItem('token');
      return !!token;
  }

  logout(){
    localStorage.removeItem('token');
    console.log('LOGGED OUT');
}
}
