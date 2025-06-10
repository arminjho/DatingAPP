import { Component, inject, OnInit } from '@angular/core';
import { Photo } from '../../_models/photo';
import { AdminService } from '../../_service/admin.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { CommonModule, NgFor } from '@angular/common';
import { PhotoService } from '../../_service/photo.service';
import { TagService } from '../../_service/tag.service';
import { PhotoFilterService } from '../../_service/photo-filter.service';

@Component({
  selector: 'app-photo-management',
  standalone: true,
  imports: [FormsModule, NgFor, CommonModule],
  templateUrl: './photo-management.component.html',
  styleUrl: './photo-management.component.css',
})
export class PhotoManagementComponent implements OnInit {
  photos: Photo[] = [];
  private adminService = inject(AdminService);
  private toastr = inject(ToastrService);

  tagFilter: string[] = [];
  availableTags: string[] = [];
  filteredPhotos$ = this.photoFilterService.filteredPhotos$;

  constructor(
    private photoService: PhotoService,
    private tagService: TagService,
    private photoFilterService: PhotoFilterService
  ) {}

  ngOnInit(): void {
    this.photoFilterService.loadPhotos();
    this.getPhotosForApproval();
    this.loadAvailableTags();
  }

  loadPhotosByTags() {
    const tagList = this.tagFilter.filter((tag) => tag.length > 0);

    if (tagList.length === 0) {
      this.getPhotosForApproval();

      return;
    }

    this.photoService.getPhotosByTags(tagList).subscribe({
      next: (photos) => {
        this.photoFilterService.setPhotosForCurrentView(photos);
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

  applyTagFilter() {
    this.tagFilter = [];
    this.photoFilterService.clearFilter();
  }

  getPhotosForApproval() {
    this.photoService.getPhotosForApproval().subscribe({
      next: (photos) => this.photoFilterService.setPhotosForCurrentView(photos),
      error: (err) => {
        console.log(err);
        this.toastr.error("Couldn't get photos for approval");
      },
    });
  }

  approvePhoto(photoId: number) {
    this.photoService.approvePhoto(photoId).subscribe({
      next: () => this.photoFilterService.removePhoto(photoId),

      error: (err) => {
        console.log(err);
        this.toastr.error('Issue with approving photo');
      },
    });
  }

  rejectPhoto(photoId: number) {
    this.photoService.rejectPhoto(photoId).subscribe({
      next: () => this.photoFilterService.removePhoto(photoId),

      error: (err) => {
        console.log(err);
        this.toastr.error('Issue with rejecting photo');
      },
    });
  }
}
