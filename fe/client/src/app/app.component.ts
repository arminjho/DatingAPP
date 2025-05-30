
import { Component, inject, Inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_service/account.service';
import { NgxSpinnerComponent } from 'ngx-spinner';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'DatingAPP';
  private accountService=inject(AccountService);

  

  ngOnInit(): void {
   
    
    this.setCurrentUser();

}

setCurrentUser(){
  const userString=localStorage.getItem('user');
  if(!userString)return;
  const user=JSON.parse(userString);
  this.accountService.setCurrentUser(user);
}
  

}
