import { HttpInterceptorFn } from '@angular/common/http';
import { BusyService } from '../_service/busy.service';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';
import { LoadingService } from '../_service/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  loadingService.show();

  return next(req).pipe(
    delay(1000),
    finalize(() => {
      loadingService.hide();
    })
  );
};
