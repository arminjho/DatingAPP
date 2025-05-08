import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_service/account.service';
import { compileNgModule } from '@angular/compiler';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HasRoleDirective } from '../_directives/has-role.directive';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule,BsDropdownModule,RouterLink,RouterLinkActive, HasRoleDirective],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService=inject(AccountService);
  private router=inject(Router);
  private toasterService=inject(ToastrService);
  
  model:any={};
  login(){
    
    this.accountService.login(this.model).subscribe({
      next:response=>{
        this.router.navigateByUrl('/members');
        
      },
      
      error:eror=>this.toasterService.error(eror.error)
      
    })
  }
  logout(){
   this.accountService.logout();
   this.router.navigateByUrl('/');

   
  }
  
}
