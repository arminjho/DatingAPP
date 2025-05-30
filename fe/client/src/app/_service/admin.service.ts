import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../_models/User';
import { Photo } from '../_models/photo';
import { Tag } from '../_models/tag';
import { PhotoStats } from '../_models/photoStats';
import { UserWithoutMainPhoto } from '../_models/userWithoutMainPhoto';

const ApiRoutes = {
  Admin: 'admin'

};

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  private adminUrl = `${this.baseUrl}${ApiRoutes.Admin}`;


  getUsersWithRoles() {
    return this.http.get<User[]>(`${this.adminUrl}/users-with-roles`);
  }

  updateUserRoles(username: String, roles: string[]) {
    return this.http.post<string[]>(
      `${this.adminUrl}/edit-roles/${username}`,
      roles
    );
  }

  getPhotoApprovalStats() {
    return this.http.get<PhotoStats[]>(`${this.adminUrl}/photo-approval-stats`);
  }
  getUsersWithoutMainPhoto() {
    return this.http.get<UserWithoutMainPhoto[]>(
      `${this.adminUrl}/users-without-main-photo`
    );
  }
}
