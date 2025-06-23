import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { DepartmentsComponent } from './pages/departments/departments.component';
import { EmployeeComponent } from './pages/employee/employee.component';
import { LoginComponent } from './pages/login/login.component';
import { EmployeedashboardComponent } from './pages/employeedashboard/employeedashboard.component';
import { ProfileComponent } from './pages/profile/profile.component';

export const routes: Routes = [
    {
        path:"",
        component: HomeComponent
    },
    {
        path:"departments",
        component: DepartmentsComponent
    },
    {
        path:"employee",
        component: EmployeeComponent
    },
    {
        path:"login",
        component: LoginComponent
    },
    {
        path:"employeedashboard",
        component: EmployeedashboardComponent,
    },
    {
        path:"profile",
        component: ProfileComponent,
    }
];
