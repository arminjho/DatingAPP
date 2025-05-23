import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/User';
import { Photo } from '../_models/photo';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

baseUrl=environment.apiUrl;
private http=inject(HttpClient);

getUsersWithRoles(){
  return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
}

  updateUserRoles(username:String,roles:string[]){
    return this.http.post<string[]>(this.baseUrl+ 'admin/edit-roles/' + username + '?roles='+roles, {});
  }

   getPhotosForApproval() {
    return this.http.get<Photo[]>(this.baseUrl + 'admin/photos-to-moderate');
  }

  approvePhoto(photoId: number) {
    return this.http.post(this.baseUrl + 'admin/approve-photo/' + photoId, {});
  }

  rejectPhoto(photoId: number) {
    return this.http.post(this.baseUrl + 'admin/reject-photo/' + photoId, {});
  }
}
