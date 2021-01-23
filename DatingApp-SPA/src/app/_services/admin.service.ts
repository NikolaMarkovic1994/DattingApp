import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_model/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }


getUsersWithRoles() {
  return this.http.get(this.baseUrl + 'admin/usersWithRoles');
}

updateUserRoles(user: User, role: {}) {
  return this.http.post(this.baseUrl + 'admin/editRoles/' + user.userName, role);
}

getPhotosForApproval() {
  return this.http.get(this.baseUrl + 'admin/photosFromModeratiom');
}
approvePhoto(photoId) {
  return this.http.post(this.baseUrl + 'admin/approvePhoto/' + photoId, {});
}
rejectPhotol(photoId) {
  return this.http.post(this.baseUrl + 'admin/rejectPhoto/' + photoId, {});
}

}
