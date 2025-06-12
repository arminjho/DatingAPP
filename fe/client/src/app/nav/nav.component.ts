import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_service/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HasRoleDirective } from '../_directives/has-role.directive';
import { AuthStoreService } from '../_service/auth-store.service';
import { Observable } from 'rxjs';
import { CommonModule, NgIf } from '@angular/common';
import { LoginDto } from '../_models/loginDto';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [
    FormsModule,
    BsDropdownModule,
    RouterLink,
    RouterLinkActive,
    HasRoleDirective,
    CommonModule,
  ],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  accountService = inject(AccountService);
  authStore = inject(AuthStoreService);
  private router = inject(Router);
  private toasterService = inject(ToastrService);

  model: LoginDto = {
    username: '',
    password: '',
  };

  isLoggedIn$: Observable<boolean> = this.authStore.isLoggedIn$;
  isAdmin$: Observable<boolean> = this.authStore.isAdmin$;

  currentUser$ = this.authStore.currentUser$;

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/members');
      },

      error: (eror) => this.toasterService.error(eror.error),
    });
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
