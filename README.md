# EmployeeManagementSystem
### Prerequisites
- Node.js 18+
- .NET 7 SDK
- SQL Server 2019+

### Installation
1. **Clone repository**
   ```bash
   git clone https://github.com/saniajain3/Employee_management_system.git
   cd Employee_management_system

## Backend setup
cd EmployeeManagementSystem
dotnet restore
dotnet user-secrets set "Jwt:Key" "your-secret-key-32"
dotnet run

## Frontend Setup
cd web-app
npm install
npm start
