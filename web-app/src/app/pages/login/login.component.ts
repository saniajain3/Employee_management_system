import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  authService = inject(AuthService);
  fb = inject(FormBuilder);
  router = inject(Router);

  hidePassword = true;

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(5)]],
  });

  ngOnInit(): void {
    if (this.authService.isLoggedIn) {
      this.router.navigateByUrl('/');
    }
  }

  login(): void {
    const { email, password } = this.loginForm.value;
    this.authService.login(email, password).subscribe((result) => {
      console.log(result);
      this.authService.saveToken(result);
      if (result.Role == "Admin") {
        this.router.navigateByUrl('/');
      }
      else {
        this.router.navigateByUrl('/employeedashboard');
      }
    });
  }
}
