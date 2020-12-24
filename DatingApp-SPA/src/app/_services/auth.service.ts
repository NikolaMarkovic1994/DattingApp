import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'WeatherForecast/';
  jwtHelper = new JwtHelperService();
  decodeToken: any;
constructor(private http: HttpClient) { }

loging(model: any) {
  return this.http.post(this.baseUrl + 'log', model).pipe(
    map((response: any) => {
       const user = response;
       if (user) {
         localStorage.setItem('token', user.token);
         this.decodeToken = this.jwtHelper.decodeToken(user.token);
         console.log(this.decodeToken);
       }
    })
  );
}

register(model: any) {
  return this.http.post(this.baseUrl + 'reg', model);
}

loggedIn() {
  const token = localStorage.getItem('token');
  return !this.jwtHelper.isTokenExpired(token);
}

}
