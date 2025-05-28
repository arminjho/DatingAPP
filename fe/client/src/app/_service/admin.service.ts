import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../_models/User';
import { Photo } from '../_models/photo';
import { Tag } from '../_models/tag';
import { PhotoStats } from '../_models/photoStats';
import { UserWithoutMainPhoto } from '../_models/userWithoutMainPhoto';


const ApiRoutes = {
  Admin: 'admin',
  Photos: 'photos'
};

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
      `${this.adminUrl}users-with-roles`
    );
  }

  updateUserRoles(username: String, roles: string[]) {
    return this.http.post<string[]>(
      `${this.adminUrl}edit-roles/${username}`,
      roles
    );
  }

  getPhotosForApproval() {
    return this.http.get<Photo[]>(
      `${this.photoUrl}photos-to-moderate`
    );
  }

  approvePhoto(photoId: number) {
    return this.http.post(
      `${this.photoUrl}approve-photo/` + photoId,
      {}
    );
  }

  rejectPhoto(photoId: number) {
    return this.http.post(
      `${this.photoUrl}reject-photo/` + photoId,
      {}
    );
  }

  getPhotosByTags(tags: string[]) {
    const params = new HttpParams({ fromObject: { tags } });
    return this.http.get<Photo[]>(
       `${this.photoUrl}unapproved-by-tags`,
      {
        params,
      }
    );
  }

  getAllTags() {
    return this.http.get<Tag[]>(`${this.adminUrl}tags`);
  }

  addTag(tag: { name: string }) {
    return this.http.post<Tag>(`${this.adminUrl}add-tag`, tag);
  }

  deleteTag(tagId: number) {
    return this.http.delete(
      `${this.adminUrl}delete-tag/` + tagId
    );
  }

  getPhotoApprovalStats() {
    return this.http.get<PhotoStats[]>(
      `${this.adminUrl}photo-approval-stats`
    );
  }
  getUsersWithoutMainPhoto() {
    return this.http.get<UserWithoutMainPhoto[]>(
      `${this.adminUrl}users-without-main-photo`
    );
  }
}
