import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_service/account.service';
import { compileNgModule } from '@angular/compiler';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule,BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService=inject(AccountService);
  
  model:any={};
  login(){
    
    this.accountService.login(this.model).subscribe({
      next:response=>{
        console.log(response);
        
      },
      
      error:eror=>console.log(eror)
      
    })
  }
  logout(){
   this.accountService.logout();
  }
  
}
