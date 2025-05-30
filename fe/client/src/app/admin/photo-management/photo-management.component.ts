import { Component, inject, OnInit } from '@angular/core';
import { Photo } from '../../_models/photo';
import { AdminService } from '../../_service/admin.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { NgFor } from '@angular/common';
import { PhotoService } from '../../_service/photo.service';
import { TagService } from '../../_service/tag.service';

@Component({
  selector: 'app-photo-management',
  standalone: true,
  imports: [FormsModule, NgFor],
  templateUrl: './photo-management.component.html',
  styleUrl: './photo-management.component.css',
})
export class PhotoManagementComponent implements OnInit {
  photos: Photo[] = [];
  private adminService = inject(AdminService);
  private toastr = inject(ToastrService);

  tagFilter:string[] = [];
  availableTags: string[] = [];

  constructor(private photoService:PhotoService, private tagService:TagService){

  }

  ngOnInit(): void {
    this.getPhotosForApproval();
    this.loadAvailableTags();
  }


  loadPhotosByTags() {
    const tagList = this.tagFilter.filter((tag)=>tag.length>0);

    if (tagList.length === 0) {
      this.getPhotosForApproval();

      return;
    }

    this.photoService.getPhotosByTags(tagList).subscribe({
      next: (photos) => {
        this.photos = photos;
      },

      error: (err) => {
        console.log(err);

        this.toastr.error("Couldn't filter photos by tags");
      },
    });
  }

  
loadAvailableTags() {
  this.tagService.getAllTags().subscribe({
    next: (tags) => {
      console.log('Tags from API:', tags);
      this.availableTags = tags.map((t: any) => t.name);
    },
    error: (err) => {
      console.log(err);
      this.toastr.error("Couldn't load tags");
    },
  });
}


  getPhotosForApproval() {
    this.photoService.getPhotosForApproval().subscribe({
      next: (photos) => (this.photos = photos),
      error: (err) => {
        console.log(err);
        this.toastr.error("Couldn't get photos for approval");
      },
    });
  }

  approvePhoto(photoId: number) {
    this.photoService.approvePhoto(photoId).subscribe({
      next: () => {
        const photoToApprove = this.photos.find((p) => p.id === photoId);
        if (photoToApprove) {
          this.photos = this.photos.filter((p) => p.id !== photoId);
        }
      },
      error: (err) => {
        console.log(err);
        this.toastr.error('Issue with approving photo');
      },
    });
  }

  rejectPhoto(photoId: number) {
    this.photoService.rejectPhoto(photoId).subscribe({
      next: () => {
        const photoToReject = this.photos.find((p) => p.id === photoId);
        if (photoToReject) {
          this.photos = this.photos.filter((p) => p.id !== photoId);
        }
      },
      error: (err) => {
        console.log(err);
        this.toastr.error('Issue with rejecting photo');
      },
    });
  }
}
