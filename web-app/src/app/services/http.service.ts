import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IDepartement } from '../types/department';
import { IEmployee } from '../types/employee';
import { environment } from '../../environments/environment';
import { from } from 'rxjs';
import { PagedData } from '../types/paged-data';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  http = inject(HttpClient);

  constructor() { }

  getDepartments(filter:any) {
    const params = new HttpParams({ fromObject: filter });
    return this.http.get<PagedData<IDepartement>>(environment.apiUrl + "/api/Department",{params});
  }
  addDepartment(cadre: string) {
    return this.http.post(environment.apiUrl + "/api/Department", {
      cadre: cadre
    });
  }
  updateDepartment(cadre: string) {
    return this.http.put(`${environment.apiUrl}/api/Department/${cadre}`, {
      cadre: cadre,
    });
  }
  deleteDepartment(cadre: string) {
    return this.http.delete(`${environment.apiUrl}/api/Department/${cadre}`);
  }

  getEmployees(filter: any) {
  const params = new HttpParams({ fromObject: filter });
  return this.http.get<PagedData<IEmployee>>(environment.apiUrl + "/api/Employee", { params });
}

  addEmployee(employee: IEmployee) {
    return this.http.post(environment.apiUrl + "/api/Employee", employee);
  }
  getEmployeeById(id: number) {
    return this.http.get<IEmployee>(`${environment.apiUrl}/api/Employee/${id}`);
  }
  updateEmployee(id: number, employee: IEmployee) {
    {
      return this.http.put(`${environment.apiUrl}/api/Employee/${id}`, employee);
    }
  }
  DeleteEmployeeByICNo(id: number) {
    return this.http.delete(`${environment.apiUrl}/api/Employee/by-pisno/${id}`);
  }

}