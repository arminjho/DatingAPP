import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../_models/User';
import { Photo } from '../_models/photo';
import { Tag } from '../_models/tag';
import { PhotoStats } from '../_models/photoStats';
import { UserWithoutMainPhoto } from '../_models/userWithoutMainPhoto';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getUsersWithRoles() {
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
  }

  updateUserRoles(username: String, roles: string[]) {
    return this.http.post<string[]>(
      this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles,
      {}
    );
  }

  getPhotosForApproval() {
    return this.http.get<Photo[]>(this.baseUrl + 'photos/photos-to-moderate');
  }

  approvePhoto(photoId: number) {
    return this.http.post(this.baseUrl + 'photos/approve-photo/' + photoId, {});
  }

  rejectPhoto(photoId: number) {
    return this.http.post(this.baseUrl + 'photos/reject-photo/' + photoId, {});
  }

  getPhotosByTags(tags: string[]) {
    const params = new HttpParams({ fromObject: { tags } });
    return this.http.get<Photo[]>(this.baseUrl + 'photos/unapproved-by-tags', {
      params,
    });
  }

  getAllTags() {
    return this.http.get<Tag[]>(this.baseUrl + 'admin/tags');
  }

  addTag(tag: { name: string }) {
    return this.http.post<Tag>(this.baseUrl + 'admin/add-tag', tag);
  }

  deleteTag(tagId: number) {
    return this.http.delete(this.baseUrl + 'admin/delete-tag/' + tagId);
  }

  getPhotoApprovalStats(){
    return this.http.get<PhotoStats[]>(this.baseUrl+'admin/photo-approval-stats');
  }
   getUsersWithoutMainPhoto(){
    return this.http.get<UserWithoutMainPhoto[]>(this.baseUrl+'admin/users-without-main-photo');
  }
}
