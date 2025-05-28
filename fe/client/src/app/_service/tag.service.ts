import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Tag } from '../_models/tag';

const ApiRoutes = {
  Tags: 'tags',
};

@Injectable({
  providedIn: 'root',
})
export class TagService {
  baseUrl = environment.apiUrl;
  private tagsUrl = `${this.baseUrl}${ApiRoutes.Tags}/`;
  constructor(private http: HttpClient) {}

  getAllTags() {
    return this.http.get<Tag[]>(`${this.tagsUrl}tags`);
  }

  addTag(tag: { name: string }) {
    return this.http.post<Tag>(`${this.tagsUrl}add-tag`, tag);
  }

  deleteTag(tagId: number) {
    return this.http.delete(`${this.tagsUrl}delete-tag/` + tagId);
  }
}
