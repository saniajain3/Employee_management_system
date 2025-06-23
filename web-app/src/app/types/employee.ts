export enum Gender {
  Male = "Male",
  Female = "Female",
  Other = "Other"
}
export enum Category{
  General = "General",
  OBC = "OBC",
  SC = "SC",
  ST = "ST"
}

export interface IEmployee {
  IC_No: string;
  PIS_No: number; 
  Name: string;
  Designation: string;
  Cadre: string;
  Department?: any; 
  Sub_Cadre: string;
  Group: string;
  Email: string;
  Phone?: string;
  DOB?: string;
  Date_of_Superannuation?: string;
  Category?: string;
  Gender: string; // or Gender, if enums still work
  Latest_Qualification?: string;
  Date_of_Joining?: string;
  Latest_Discipline?: string;
}
// export interface IUserProfile {
//   name: string;
//   email: string;
//   phone: string;
//   profileImage: string;
// }

