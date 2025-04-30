import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_service/account.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const accService=inject(AccountService);
  const toastrService=inject(ToastrService);

  if(accService.currentUser()){
    return true;
  }else
  {
    toastrService.error('You shall not pass');
  }
  return false;
};
