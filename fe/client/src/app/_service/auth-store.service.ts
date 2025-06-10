import { Injectable, computed, signal } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root',
})
export class AuthStoreService {
  private currentUserSubject = new BehaviorSubject<User | null>(
    this.getUserFromLocalStorage()
  );

  currentUser$: Observable<User | null> =
    this.currentUserSubject.asObservable();

  isLoggedIn$: Observable<boolean> = this.currentUser$.pipe(
    map((user) => !!user)
  );

  isAdmin$: Observable<boolean> = this.currentUser$.pipe(
    map((user) => {
      if (!user) return false;

      const role = JSON.parse(atob(user.token.split('.')[1])).role;

      return Array.isArray(role) ? role.includes('Admin') : role === 'Admin';
    })
  );

  roles$: Observable<string[]> = this.currentUser$.pipe(
    map((user) => {
      try {
        if (!user) return [];
        const decoded = JSON.parse(atob(user.token.split('.')[1]));
        const roles = decoded.role;
        return Array.isArray(roles) ? roles : [roles];
      } catch {
        return [];
      }
    })
  );

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
