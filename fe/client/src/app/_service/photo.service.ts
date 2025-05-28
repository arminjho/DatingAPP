import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Photo } from '../_models/photo';

const ApiRoutes = {
  Photos: 'photos',
};

@Injectable({
  providedIn: 'root',
})
export class PhotoService {
  baseUrl = environment.apiUrl;
  private photoUrl = `${this.baseUrl}${ApiRoutes.Photos}/`;

  constructor(private http: HttpClient) {}

  getPhotosForApproval() {
    return this.http.get<Photo[]>(`${this.photoUrl}photos-to-moderate`);
  }

  approvePhoto(photoId: number) {
    return this.http.post(`${this.photoUrl}approve-photo/` + photoId, {});
  }

  rejectPhoto(photoId: number) {
    return this.http.post(`${this.photoUrl}reject-photo/` + photoId, {});
  }

  getPhotosByTags(tags: string[]) {
    const params = new HttpParams({ fromObject: { tags } });
    return this.http.get<Photo[]>(`${this.photoUrl}unapproved-by-tags`, {
      params,
    });
  }
}
