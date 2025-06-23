import { Component, OnInit, inject } from '@angular/core';
import { IEmployee } from '../../types/employee';
import { HttpService } from '../../services/http.service';
import { TableComponent } from '../../components/table/table.component';
import { EmployeeFormComponent } from './employee-form/employee-form.component';

// Angular Material modules
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { debounceTime } from 'rxjs';
import {MatPaginatorModule} from '@angular/material/paginator';
import { PagedData } from '../../types/paged-data';

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [
    TableComponent,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatPaginatorModule,
  ],
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent implements OnInit {
  httpService = inject(HttpService);
  dialog = inject(MatDialog);

  pagedEmployeeData!: PagedData<IEmployee>;
  allEmployees: IEmployee[] = [];       // Full original list
  value: string = ''; 

  showCols = [
    'IC_No',
    'PIS_No',
    'Name',
    'Designation',
    'Cadre',
    'Sub_Cadre',
    'Group',
    'Email',
    'Phone',
    'DOB',
    'Date_of_Superannuation',
    'Category',
    'Gender',
    'Latest_Qualification',
    'Date_of_Joining',
    'Latest_Discipline',
    'actions',
  ];

  filter:any={
    pageIndex:0,
    pageSize:10,
  };

  ngOnInit() {
    this.getLatestData();
    this.searchControl.valueChanges.pipe(debounceTime(200))
    .subscribe((result : string |null) => {
      console.log(result);
      this.filter.search=result;
      this.filter.pageIndex=0; 
      this.getLatestData();
    });
  }
totalData! : number;
  getLatestData() {
    this.httpService.getEmployees(this.filter).subscribe((result) => {
      this.pagedEmployeeData= result;
    });
  }

  edit(employee: IEmployee) {
    const ref = this.dialog.open(EmployeeFormComponent, {
      panelClass: 'm-auto',
      data: {
        employeeId: employee.PIS_No,
      },
    });
    ref.afterClosed().subscribe(() => this.getLatestData());
  }

  delete(employee: IEmployee) {
    this.httpService.DeleteEmployeeByICNo(employee.PIS_No).subscribe(() => {
      alert('Record Deleted');
      this.getLatestData();
    });
  }

  add() {
    this.openDialog();
  }

  openDialog(): void {
    const ref = this.dialog.open(EmployeeFormComponent, {
      panelClass: 'm-auto',
      data: {},
    });
    ref.afterClosed().subscribe(() => this.getLatestData());
  }

  searchControl = new FormControl('');

  pageChange(event:any){
    console.log(event);
    this.filter.pageIndex=event.pageIndex;
    this.getLatestData();
  }
}
