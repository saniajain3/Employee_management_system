import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { IAuthToken } from '../types/auth';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  http = inject(HttpClient);
  constructor() { }
  router = inject(Router);
  login(email: string, password: string) {
    return this.http.post<IAuthToken>(environment.apiUrl + "/api/Auth/login", {
      email: email,
      password: password,
    })
  }
  saveToken(authToken: IAuthToken) {
    localStorage.setItem('auth', JSON.stringify(authToken));
    localStorage.setItem('token', authToken.Token);

  }
  get isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
  logout() {
    localStorage.removeItem('auth');
    localStorage.removeItem('token');
    this.router.navigateByUrl("/login");
  }
  // getUserRole(): string | null {
  //   const token = localStorage.getItem('token');
  //   if (!token) return null;
  //   try {
  //     const decoded = JSON.parse(atob(token.split('.')[1]));
  //     return decoded.role || null;
  //   } catch (e) {
  //     return null;
  //   }
  // }
  // get isAdmin(): boolean {
  //   return this.getUserRole() === 'Admin';
  // }
  get isEmployee(): boolean {
    const authData = localStorage.getItem('auth');
    if (!authData) return false;

    try {
      const token = JSON.parse(authData);
      return token?.Role === 'Employee';
    } catch (e) {
      return false;
    }
  }
  get AuthDetails(): IAuthToken | null {
    if (!this.isLoggedIn) return null;
    const auth = localStorage.getItem('auth');
    if (!auth) return null;
    let token: IAuthToken = JSON.parse(auth);
    return token;
  }

  getProfile() {
    return this.http.get(environment.apiUrl + "/api/Auth/get-profile");
  }
  updateProfile(profile: { username: string; profileImage?: string }) {
    return this.http.post(environment.apiUrl + "/api/Auth/update-profile", profile);
  }

}
