import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_service/account.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  accountService = inject(AccountService);
  private toastrService=inject(ToastrService);
  private router=inject(Router);
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  register() {
    this.accountService.register(this.model).subscribe({
      next: _ => {this.router.navigateByUrl("/members"); this.cancel(); },
      error: err => this.toastrService.error(err.error)
    })
  }
  cancel() {
    this.cancelRegister.emit(false);
  }

}
