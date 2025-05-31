import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../_service/admin.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { Tag } from '../../_models/tag';
import { CommonModule } from '@angular/common';
import { TagService } from '../../_service/tag.service';



@Component({
  selector: 'app-tag-management',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './tag-management.component.html',
  styleUrl: './tag-management.component.css',
})
export class TagManagementComponent implements OnInit {
  private adminService = inject(AdminService);

  private toastr = inject(ToastrService);

  tags: Tag[] = [];

  newTag = '';

  constructor(private tagService:TagService){

  }

  ngOnInit(): void {
    this.loadTags();
  }

  loadTags() {
    this.tagService.getAllTags().subscribe({
      next: (tags) => (this.tags = tags),

      error: (err) => {
        console.error(err);
        this.toastr.error("Couldn't load tags");
      },
    });
  }

  addTag() {
    const name = this.newTag.trim();

    if (!name) return;

    this.tagService.addTag({ name }).subscribe({
      next: (tag) => {
        this.tags = [...this.tags, tag];
        this.newTag = '';
        this.toastr.success('Tag added');
      },

      error: (err) => {
        console.error(err);
        this.toastr.error('Failed to add tag');
      },
    });
  }

  deleteTag(tagId: number) {
    this.tagService.deleteTag(tagId).subscribe({
      next: () => {
        this.tags = this.tags.filter((t) => t.id !== tagId);
        this.toastr.success('Tag deleted');
      },

      error: (err) => {
        console.error(err);
        this.toastr.error('Failed to delete tag');
      },
    });
  }
}
