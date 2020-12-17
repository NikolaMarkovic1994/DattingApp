import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/WeatherForecast/';

constructor(private http: HttpClient) { }

loging(model: any){
  return this.http.post(this.baseUrl+'log',model).pipe(
    map((response:any)=> {
       const user = response;
       if(user){
         localStorage.setItem('token',user.token)
       }
    })
  )
}

register(model: any){
  return this.http.post(this.baseUrl+'reg',model);
}

}
