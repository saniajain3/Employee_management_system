import { Component, inject, OnInit } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Validators } from '@angular/forms';
import { tokenHttpInterceptor } from '../../services/token-http-interceptor';
import { MatTableModule } from '@angular/material/table';


@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatTableModule,
    ReactiveFormsModule,
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  private authService = inject(AuthService);
  profileForm !: FormGroup;
  fb=inject(FormBuilder);
  hidePassword = true; 
  ngOnInit() {
    this.profileForm=this.fb.group({
      email:[],
      profileImage:[],
      phone:[],
       password: [''],
      name:[]
    })
    this.authService.getProfile().subscribe(result => {
      console.log(result);
      this.profileForm.patchValue(result);
    })
  }
  displayedColumns = ['name', 'email', 'phone', 'actions'];
editing = false;

onUpdate() {
  if (this.profileForm.valid) {
    const updatedProfile = this.profileForm.value;
    this.authService.updateProfile(updatedProfile).subscribe({
      next: (res) => {
        console.log('Profile updated:', res);
        this.editing = false;
      },
      error: (err) => {
        console.error('Update failed:', err);
      }
    });
  }
}
}