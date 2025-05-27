import { Component, OnInit } from '@angular/core';
import { PhotoStats } from '../../_models/photoStats';
import { UserWithoutMainPhoto } from '../../_models/userWithoutMainPhoto';
import { AdminService } from '../../_service/admin.service';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-user-statistics',
  standalone: true,
  imports: [NgFor],
  templateUrl: './user-statistics.component.html',
  styleUrl: './user-statistics.component.css'
})
export class UserStatisticsComponent implements OnInit {
    photoStats:PhotoStats[]=[];
    usersWithoutMainPhoto:UserWithoutMainPhoto[]=[];

  constructor(private adminService:AdminService){

  }

  ngOnInit(): void {
    this.loadPhotoStats();
    this.loadUsersWithoutMainPhoto();
  }

    loadPhotoStats(){
      this.adminService.getPhotoApprovalStats().subscribe({
        next: response=>this.photoStats=response,
        error:err=>console.error("Couldn't load photo stats", err)
      })
    }

    loadUsersWithoutMainPhoto(){
      this.adminService.getUsersWithoutMainPhoto().subscribe({
        next: response=>this.usersWithoutMainPhoto=response,
        error:err=>console.error("Couldn't load users stats", err)
      })
    }
}
