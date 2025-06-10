import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_service/account.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthStoreService } from '../_service/auth-store.service';
import { map, take } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const authStore = inject(AuthStoreService);
  const toastrService = inject(ToastrService);

  return authStore.isLoggedIn$.pipe(
    take(1),
    map((isLoggedIn) => {
      if (isLoggedIn) return true;

      toastrService.error('You shall not pass');
      return false;
    })
  );
};
