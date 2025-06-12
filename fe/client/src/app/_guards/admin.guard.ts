import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthStoreService } from '../_service/auth-store.service';
import { map, take, tap } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  const authStore = inject(AuthStoreService);
  const toastr = inject(ToastrService);
  const router = inject(Router);

  return authStore.roles$.pipe(
    take(1),
    map((roles) => roles.includes('Admin') || roles.includes('Moderator')),
    tap((hasAccess) => {
      if (!hasAccess) {
        router.navigate(['/']);
      }
    })
  );
};
