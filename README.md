# ASP.NET Core Employee Demo API

A simple demo project built with **ASP.NET Core Web API**.  
Shows basic controller setup, cookie-based authentication, and Swagger integration.

---

## 🚀 Features

- **API Endpoints**
  - `GET /api/employees` → Returns all employees
  - `GET /api/employees/{id}` → Returns a single employee
- **Authentication**
  - Cookie authentication middleware
  - Demo login: user is authenticated if `userId == password`
- **Swagger**
  - Interactive Swagger UI for testing endpoints

---

## 🛠️ Technologies

- ASP.NET Core 8 (Web API)
- Cookie Authentication
- Swagger (Swashbuckle.AspNetCore)

---

## ⚡ Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)

### Run Locally
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/employee-demo-api.git
   cd employee-demo-api
   dotnet restore
   cd TalentManager
   dotnet run

2. Open http://localost:5160 or whichever port is displayed in the line similar to the one below.
info: Microsoft.Hosting.Lifetime[14]
   Now listening on: http://localhost:5160

3. Add /swagger to the end of the endpoint to view Swagger UI
