import { Injectable, computed, signal } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/User';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  role: string | string[];
}

@Injectable({
  providedIn: 'root',
})
export class AuthStoreService {
  private currentUserSubject: BehaviorSubject<User | null>;
  currentUser$: Observable<User | null>;

  isLoggedIn$: Observable<boolean>;
  isAdmin$: Observable<boolean>;
  roles$: Observable<string[]>;

  constructor() {
    const user = this.getUserFromLocalStorage();
    this.currentUserSubject = new BehaviorSubject<User | null>(user);
    this.currentUser$ = this.currentUserSubject.asObservable();

    this.isLoggedIn$ = this.currentUser$.pipe(map((user) => !!user));

    this.isAdmin$ = this.currentUser$.pipe(
      map((user) => {
        if (!user) return false;
        try {
          const decoded = jwtDecode<JwtPayload>(user.token);
          const role = decoded.role;

          return Array.isArray(role)
            ? role.includes('Admin')
            : role === 'Admin';
        } catch {
          return false;
        }
      })
    );

    this.roles$ = this.currentUser$.pipe(
      map((user) => {
        try {
          if (!user) return [];
          const decoded = jwtDecode<JwtPayload>(user.token);
          const roles = decoded.role;
          return Array.isArray(roles) ? roles : [roles];
        } catch {
          return [];
        }
      })
    );
  }

  setUser(user: User | null) {
    if (user) {
      localStorage.setItem('user', JSON.stringify(user));
    } else {
      localStorage.removeItem('user');
    }

    this.currentUserSubject.next(user);
  }

  private getUserFromLocalStorage(): User | null {
    const user = localStorage.getItem('user');

    return user ? JSON.parse(user) : null;
  }
}
