import {
  Component,
  HostListener,
  inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Member } from '../../_models/member';
import { MembersService } from '../../_service/members.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PhotoEditorComponent } from '../photo-editor/photo-editor.component';
import { DatePipe } from '@angular/common';
import { TimeagoModule } from 'ngx-timeago';
import { AuthStoreService } from '../../_service/auth-store.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [
    TabsModule,
    FormsModule,
    PhotoEditorComponent,
    DatePipe,
    TimeagoModule,
  ],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css',
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member?: Member;
  private toastr = inject(ToastrService);
  private authStore = inject(AuthStoreService);
  private memberService = inject(MembersService);
  ngOnInit(): void {
    this.loadMember();
  }
  loadMember() {
    this.authStore.currentUser$.pipe(take(1)).subscribe((user) => {
      if (!user) return;

      this.memberService.getMember(user.username).subscribe({
        next: (member) => (this.member = member),
        error: (err) => console.log(err),
      });
    });
  }
  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: (_) => {
        this.toastr.success('Profile updated successfully');
        this.editForm?.reset(this.member);
      },
    });
  }
  onMemberChange(event: Member) {
    this.member = event;
  }
}
