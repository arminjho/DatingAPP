import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_service/members.service';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MemberCardComponent } from '../member-card/member-card.component';
import { AccountService } from '../../_service/account.service';
import { UserParams } from '../../_models/userParams';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { AuthStoreService } from '../../_service/auth-store.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule, ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent implements OnInit {
  memberService = inject(MembersService);
  authStore = inject(AuthStoreService);
  private accountService = inject(AccountService);
  userParams!: UserParams;
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];

  ngOnInit(): void {
    this.authStore.currentUser$.pipe(take(1)).subscribe((user) => {
      if (!user) return;

      this.userParams = new UserParams(user);

      if (!this.memberService.paginatedResult()) {
        this.loadMembers();
      }
    });
  }
  loadMembers() {
    this.memberService.getMembers(this.userParams);
  }
  resetFilters() {
    this.authStore.currentUser$.pipe(take(1)).subscribe((user) => {
      if (!user) return;

      this.userParams = new UserParams(user);

      this.loadMembers();
    });
  }
  pageChanged(event: any) {
    if (this.userParams.pageNumber != event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }
  }
}
