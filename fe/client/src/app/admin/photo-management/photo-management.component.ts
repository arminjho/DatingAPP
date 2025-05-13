import { Component, inject, OnInit } from '@angular/core';
import { Photo } from '../../_models/photo';
import { AdminService } from '../../_service/admin.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-photo-management',
  standalone: true,
  imports: [],
  templateUrl: './photo-management.component.html',
  styleUrl: './photo-management.component.css'
})
export class PhotoManagementComponent implements OnInit {
  photos: Photo[] = [];
  private adminService = inject(AdminService);
  private toastr = inject(ToastrService);

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotosForApproval().subscribe({
      next: photos => this.photos = photos,
      error:err=> {
        console.log(err);
        this.toastr.error("Couldn't get photos for approval")
      }
    })
  }

  approvePhoto(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe({
      next: () => {
        const photoToApprove = this.photos.find(p=>p.id===photoId);
        if(photoToApprove){
          this.photos=this.photos.filter(p=>p.id !==photoId)
        }
      },
       error:err=> {
        console.log(err);
        this.toastr.error("Issue with approving photo")
      }
    })
  }

  rejectPhoto(photoId: number) {
    this.adminService.rejectPhoto(photoId).subscribe({
      next: () => {
        const photoToReject = this.photos.find(p=>p.id===photoId);
        if(photoToReject){
          this.photos=this.photos.filter(p=>p.id !==photoId)
        }
      },
       error:err=> {
        console.log(err);
        this.toastr.error("Issue with rejecting photo")
      }
    })
  }
}
