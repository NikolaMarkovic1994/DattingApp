import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginationResult } from '../_model/pagination';
import { User } from '../_model/user';


@Injectable({
  providedIn: 'root'
})
export class UserService {
   baseUrl = environment.apiUrl + 'users/';


constructor(private http: HttpClient) { }

getUsers(page?, itemsPerPage?, userParams?, likesParams?): Observable<PaginationResult<User[]>> {
  const paginateResulet: PaginationResult<User[]> = new PaginationResult<User[]>();
  let params = new HttpParams();
  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);

  }
  if (userParams != null) {
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

  }
  if (likesParams === 'Likers') {
    params = params.append('likers', 'true');
  }
  if (likesParams === 'Likees') {
    params = params.append('likees', 'true');
  }
  return this.http.get<User[]>(this.baseUrl + 'userss', {observe : 'response', params})
  .pipe(
    map(response => {
      paginateResulet.result = response.body;
      if (response.headers.get('Pagination') != null) {
        paginateResulet.pagination = JSON.parse(response.headers.get('Pagination'));

      }
      return paginateResulet;
    })
  );
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
sendLike(id: number , recipientId: number) {
  return this.http.post(this.baseUrl +  id + '/like/' + recipientId, {});
  // http post mora da posanje nesto u telo alo je mosze moslati prazno {}

}
}
