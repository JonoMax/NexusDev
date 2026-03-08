import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface User {
  id: number;
  username: string;
  isAdmin: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7146/api/auth';
  user = signal<User | null>(null);
  isLoggedIn = computed(() => !!this.user());

  login(username: string, password: string) {
    return this.http.post<User>(`${this.apiUrl}/login`, { username, password })
  }

  register(username: string, password: string) {
    return this.http.post<User>(`${this.apiUrl}/register`, { username, password })
  }

  setUser(user: User){
    this.user.set(user);
  }

  logout(){
    this.user.set(null);
  }

  isAdmin() {
    return this.user()?.isAdmin === true;
  }
}