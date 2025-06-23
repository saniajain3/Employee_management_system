import { Component, OnInit, inject, Inject, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { IDepartement } from '../../../types/department';
import { HttpService } from '../../../services/http.service';
import { Gender, Category } from '../../../types/employee';
import { MatCard, MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatButtonModule,
    MatDialogModule,
    MatCardModule
  ],
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss']
})
export class EmployeeFormComponent implements OnInit {
  // @Input() employeeId!: number;

  employeeForm: FormGroup;
  genderOptions = Object.values(Gender);
  categoryOptions = Object.values(Category);
  departments: IDepartement[] = [];

  private fb = inject(FormBuilder);
  private httpService = inject(HttpService);
  private dialogRef = inject(MatDialogRef<EmployeeFormComponent>);
  // private data_emp=inject<EmployeeFormComponent>(MAT_DIALOG_DATA);

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    this.employeeForm = this.fb.group({
      IC_No: ['', Validators.required],
      PIS_No: [null, Validators.required],
      Name: ['', Validators.required],
      Designation: [''],
      Cadre: ['', Validators.required],
      Sub_Cadre: [''],
      Group: [''],
      Email: ['', Validators.email],
      Phone: ['', [Validators.pattern(/^[0-9]{10}$/)]],
      DOB: [null],
      Date_of_Superannuation: [null],
      Category: ['', Validators.required],
      Gender: ['', Validators.required],
      Latest_Qualification: [''],
      Date_of_Joining: [null],
      Latest_Discipline: ['']
    });

    if (data) {
      this.employeeForm.patchValue(data);
    }
  }

  ngOnInit(): void {
    this.httpService.getDepartments({}).subscribe((result) => {
      this.departments = result.data;
    });

    if (this.data?.employeeId) {
      this.httpService.getEmployeeById(this.data.employeeId).subscribe((employeeData) => {
        this.employeeForm.patchValue(employeeData as any);

        // Disable fields that should NOT be edited
        this.employeeForm.get('IC_No')?.disable();
        this.employeeForm.get('PIS_No')?.disable();
        this.employeeForm.get('DOB')?.disable();
        this.employeeForm.get('Gender')?.disable();
        this.employeeForm.get('Cadre')?.disable();
        // this.employeeForm.get('Group')?.disable();
        // this.employeeForm.get('Latest_Qualification')?.disable();
        this.employeeForm.get('Date_of_Joining')?.disable();
        // this.employeeForm.get('Latest_Discipline')?.disable();
      });
    }
  }


  onSubmit(): void {
    if (this.data?.employeeId) {
      this.httpService.updateEmployee(this.data.employeeId, this.employeeForm.value).subscribe(() => {
        alert('Employee Details Updated Successfully');
        this.dialogRef.close(this.employeeForm.value);
      });
    } else {
      if (this.employeeForm.valid) {
        this.httpService.addEmployee(this.employeeForm.value).subscribe(() => {
          alert('Employee Added Successfully');
          this.dialogRef.close(this.employeeForm.value);
        });
      }
    }
  }

}
