import { Component, inject, OnInit } from '@angular/core';
import { HttpService } from '../../services/http.service';
import { IDepartement } from '../../types/department';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { PagedData } from '../../types/paged-data';
import { TableComponent } from '../../components/table/table.component';

@Component({
  selector: 'app-departments',
  imports: [MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    TableComponent
  ],
  templateUrl: './departments.component.html',
  styleUrl: './departments.component.scss'
})
export class DepartmentsComponent implements OnInit {
  httpService = inject(HttpService);
  departments!: PagedData <IDepartement>;
  isFormOpen = false;
  filter={
    pageIndex:0,
    pageSize:10,
  }
  showCols = [
    'Cadre',
    'Total_Employees',
    'Actions',
  ]
  ngOnInit() {
    this.getLatestData();
  }
  getLatestData() {
    this.httpService.getDepartments().subscribe((result) => {
      this.departments = result;
    });
  }
  departmentCadre!: string;
  // departmentName!: string;
  // addDepartment() {
  //   console.log(this.departmentName);
  //   this.httpService.addDepartment(this.departmentCadre, this.departmentName).subscribe(() => {
  //     // alert("Department added");
  //     this.isFormOpen = false;
  //     this.getLatestData();
  //     this.departmentCadre = '';
  //     this.departmentName = '';
  //   });
  // }
  addDepartment() {
  if (!this.departmentCadre) {
    alert("Please enter Cadre.");
    return;
  }

  this.httpService.addDepartment(this.departmentCadre).subscribe(() => {
    this.isFormOpen = false;
    this.getLatestData();
    this.departmentCadre = '';
  });
}

  editCadre = '';
  editDepartment(department: IDepartement) {
    this.departmentCadre = department.Cadre;
    this.isFormOpen = true;
    this.editCadre = department.Cadre;
  }
  updateDepartment() {
    this.httpService.updateDepartment(this.editCadre).subscribe(() => {
      // alert("Department added");
      this.isFormOpen = false;
      this.getLatestData();
        this.editCadre = '';
      this.departmentCadre = '';
    });
  }
  deleteDepartment(cadre: string) {
    if (confirm("Are you sure you want to delete this department?")) {
      this.httpService.deleteDepartment(cadre).subscribe(() => {
        alert("Department deleted");
        this.getLatestData();
      });
    }
  }
    pageChange(event:any){
    console.log(event);
    this.filter.pageIndex=event.pageIndex;
    this.getLatestData();
  }
}
