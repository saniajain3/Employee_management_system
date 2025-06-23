import { Component, inject,ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatDrawerContent, MatSidenavModule} from '@angular/material/sidenav';
import {RouterLink, Router} from '@angular/router'
import { AuthService } from './services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, 
    MatToolbarModule, 
    MatIconModule, 
    MatButtonModule,
    MatSidenavModule,
    RouterLink,
    MatDrawerContent,
    CommonModule  
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'web-app';
      AuthService=inject(AuthService);
  router = inject(Router);
  currentYear: number = new Date().getFullYear();
  logout(){
    this.AuthService.logout();
  }
  
  isLoginPage(): boolean {
    return this.router.url === '/login'; // adjust this if your login route differs
  }
}
