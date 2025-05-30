import { Component, inject, Inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  registerMode = false;
  private http=inject(HttpClient);
  users:any;

  constructor() { }

  ngOnInit(): void {
    
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
  getUsers() {
    this.http.get('http://localhost:5075/api/users').subscribe({
      next: response => this.users = response,
      error: (error) => console.log(error),
      complete: () => console.log('Request has completed'),
    })
  }
  

  

}
