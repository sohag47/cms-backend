# CMS Backend

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) installed
- SQL Server or any compatible database
- IDE (e.g., [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/))

### Database CMD
1. Create Migration:
   ```bash
   Add-Migration InitialCreate
   ```
2. Database Migrate:
```bash
Update-Database
```
3. DB Connection String
```bash
Server=localhost\\SQLEXPRESS;Database=cms_backend_db;Trusted_Connection=True;TrustServerCertificate=True;
```
4. Server name
```bash
.\SQLEXPRESS 
