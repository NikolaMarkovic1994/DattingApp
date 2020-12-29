import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_model/user';


@Injectable({
  providedIn: 'root'
})
export class UserService {
   baseUrl = environment.apiUrl + 'users/';


constructor(private http: HttpClient) { }

getUsers(): Observable<User[]> {
  return this.http.get<User[]>(this.baseUrl + 'userss');
}
getUser(id): Observable<User> {
  return this.http.get<User>(this.baseUrl + id);
}
upadteUser(id: number, userName: string, user: User) {
  return this.http.put(this.baseUrl + id + '/' + userName, user);
}
setMainPhoto(userId: number, id: number) {
  return this.http.post(this.baseUrl + userId + '/photos/' + id + '/ismain', {});
}

deletePhoto(userId: number , id: number) {
  return this.http.delete(this.baseUrl + userId + '/photos/' + id);

}
}
