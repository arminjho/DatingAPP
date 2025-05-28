import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../_models/User';
import { Photo } from '../_models/photo';
import { Tag } from '../_models/tag';
import { PhotoStats } from '../_models/photoStats';
import { UserWithoutMainPhoto } from '../_models/userWithoutMainPhoto';
import { ApiRoutes } from '../constants/api-routes';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  private adminUrl = `${this.baseUrl}${ApiRoutes.Admin}/`;
  private photoUrl = `${this.baseUrl}${ApiRoutes.Photos}/`;

  getUsersWithRoles() {
    return this.http.get<User[]>(
      this.baseUrl + `${this.adminUrl}users-with-roles`
    );
  }

  updateUserRoles(username: String, roles: string[]) {
    return this.http.post<string[]>(
      this.baseUrl +
        `${this.adminUrl}edit-roles` +
        username +
        '?roles=' +
        roles,
      {}
    );
  }

  getPhotosForApproval() {
    return this.http.get<Photo[]>(this.baseUrl + `${this.photoUrl}photos-to-moderate`);
  }

  approvePhoto(photoId: number) {
    return this.http.post(this.baseUrl + `${this.photoUrl}approve-photo/` + photoId, {});
  }

  rejectPhoto(photoId: number) {
    return this.http.post(this.baseUrl + `${this.photoUrl}reject-photo/` + photoId, {});
  }

  getPhotosByTags(tags: string[]) {
    const params = new HttpParams({ fromObject: { tags } });
    return this.http.get<Photo[]>(this.baseUrl + `${this.photoUrl}unapproved-by-tags`, {
      params,
    });
  }

  getAllTags() {
    return this.http.get<Tag[]>(this.baseUrl + `${this.adminUrl}tags`);
  }

  addTag(tag: { name: string }) {
    return this.http.post<Tag>(this.baseUrl + `${this.adminUrl}add-tag`, tag);
  }

  deleteTag(tagId: number) {
    return this.http.delete(
      this.baseUrl + `${this.adminUrl}delete-tag/` + tagId
    );
  }

  getPhotoApprovalStats() {
    return this.http.get<PhotoStats[]>(
      this.baseUrl + `${this.adminUrl}photo-approval-stats`
    );
  }
  getUsersWithoutMainPhoto() {
    return this.http.get<UserWithoutMainPhoto[]>(
      this.baseUrl + `${this.adminUrl}users-without-main-photo`
    );
  }
}
