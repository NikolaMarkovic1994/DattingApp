import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_model/user';
import {BehaviorSubject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'WeatherForecast/';
  jwtHelper = new JwtHelperService();
  decodeToken: any;
  curentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

constructor(private http: HttpClient) { }
changeMemberPhoto(photo: string) {
this.photoUrl.next(photo);
}

loging(model: any) {
  return this.http.post(this.baseUrl + 'log', model).pipe(
    map((response: any) => {
       const user = response;
       if (user) {
         localStorage.setItem('token', user.token);
         localStorage.setItem('user', JSON.stringify(user.user));
         this.curentUser = user.user;
         this.decodeToken = this.jwtHelper.decodeToken(user.token);
         this.changeMemberPhoto(this.curentUser.photoUrl);
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
