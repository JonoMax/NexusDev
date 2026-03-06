import { Injectable, signal, computed } from '@angular/core';

export interface User {
  username: string;
  role: 'admin' | 'user';
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _user = signal<User | null>(null);

  user = computed(() => this._user());
  isLoggedIn = computed(() => !!this._user());
  isAdmin = computed(() => this._user()?.role === 'admin');

  login(username: string, role: 'admin' | 'user') {
    this._user.set({ username, role });
  }

  logout() {
    this._user.set(null);
  }
}