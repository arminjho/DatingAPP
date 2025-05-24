import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/member';
import {
  DecimalPipe,
  JsonPipe,
  NgClass,
  NgFor,
  NgIf,
  NgStyle,
} from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_service/account.service';
import { environment } from '../../../environments/environment';
import { Photo } from '../../_models/photo';
import { MembersService } from '../../_service/members.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [
    NgIf,
    NgFor,
    NgClass,
    NgStyle,
    FileUploadModule,
    DecimalPipe,
    FormsModule,
  ],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css',
})
export class PhotoEditorComponent implements OnInit {
  member = input.required<Member>();
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  uploader?: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  memberChange = output<Member>();

  tagInput = '';
  tagFilter = '';
  filteredPhotos: Photo[] = [];

  ngOnInit(): void {
    this.initializeUploader();
    this.filteredPhotos = this.member().photos;
  }
  setPhotoAsMain(photo: Photo) {
    this.memberService.setMainPhoto(photo).subscribe({
      next: (_) => {
        const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user);
        }
        const updatedMember = { ...this.member() };
        updatedMember.photoUrl = photo.url;
        updatedMember.photos.forEach((p) => {
          if (p.isMain) p.isMain = false;
          if (p.id === photo.id) p.isMain = true;
        });
        this.memberChange.emit(updatedMember);
      },
    });
  }
  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }
  deletePhoto(photo: Photo) {
    this.memberService.deletePhoto(photo).subscribe({
      next: (_) => {
        const updatedMember = { ...this.member() };
        updatedMember.photos = updatedMember.photos.filter(
          (x) => x.id !== photo.id
        );
        this.memberChange.emit(updatedMember);
      },
    });
  }
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo-with-tags',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingAll = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onBeforeUploadItem = (item) => {
      if (this.tagInput.trim() !== '') {
        const tagList = this.tagInput
          .split(',')
          .map((tag) => `tags=${encodeURIComponent(tag.trim())}`)
          .join('&');
        item.url = this.baseUrl + 'users/add-photo-with-tags?' + tagList;
      } else {
        item.url = this.baseUrl + 'users/add-photo-with-tags';
      }
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      const photo = JSON.parse(response);
      const updatedMember = { ...this.member() };
      updatedMember.photos.push(photo);
      this.memberChange.emit(updatedMember);
      if (photo.isMain) {
        const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user);
        }
        updatedMember.photoUrl = photo.url;
        updatedMember.photos.forEach((p) => {
          if (p.isMain) p.isMain = false;
          if (p.id === photo.id) p.isMain = true;
        });
        this.memberChange.emit(updatedMember);
        this.filteredPhotos = updatedMember.photos;
      }
    };
  }

  filterPhotos() {
    const tags = this.tagFilter
      .split(',')
      .map((t) => t.trim().toLowerCase())
      .filter((t) => t.length > 0);

    if (!tags.length) {

      this.filteredPhotos = this.member().photos;
      return;
    }

    this.filteredPhotos = this.member().photos.filter((photo) =>
      photo.tags?.some((tag) => tags.includes(tag.name.toLowerCase()))
    );
  }

  clearFilter() {

    this.tagFilter = '';
    this.filteredPhotos = this.member().photos;
  }

  filterPhotosByTag(tagName: string) {

    this.tagFilter = tagName;
    this.filterPhotos();
  }
}
