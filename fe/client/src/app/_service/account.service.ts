import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal, Signal } from '@angular/core';
import { User } from '../_models/User';
import { map } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { LikesService } from './likes.service';
import { PresenceService } from './presence.service';
import { AuthStoreService } from './auth-store.service';
import { LoginDto } from '../_models/loginDto';
import { RegisterDto } from '../_models/registerDto';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  role: string | string[];
}

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private likeService = inject(LikesService);
  private presenceService = inject(PresenceService);
  private http = inject(HttpClient);

  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);

  roles = computed(() => {
    const user = this.currentUser();
    if (user && user.token) {
      const decoded = jwtDecode<JwtPayload>(user.token);
      const role = decoded.role;
      return Array.isArray(role) ? role : [role];
    }
    return [];
  });

  constructor(private authStore: AuthStoreService) {}

  login(model: LoginDto) {
    return this.http.post<User>(`${this.baseUrl}account/login`, model).pipe(
      map((user) => {
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  register(model: RegisterDto) {
    return this.http.post<User>(`${this.baseUrl}account/register`, model).pipe(
      map((user) => {
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.authStore.setUser(user);
    this.likeService.getLikeIds();
    this.presenceService.CreateHubConnection(user);
  }

  logout() {
    this.authStore.setUser(null);
    localStorage.removeItem('user');
    this.presenceService.stopHubConnection();
  }
}
