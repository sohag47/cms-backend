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
```

# Remove Database

```bash
Drop-Database
```

# Remove Migrations

```bash
Remove-Migration
```

# dotnet-ef

```bash
dotnet tool install dotnet-ef --version 8.0.6
```

```bash
dotnet ef --version
```

```bash
dotnet new tool-manifest
```

```bash
dotnet tool restore
```

```bash
dotnet ef migrations add InitialCreate
```

```bash
dotnet ef database update
```

# When cloning project on a new PC

```bash
# After cloning, restore local tools first
dotnet tool restore

# Then restore packages
dotnet restore

# Then run migrations
dotnet ef database update
```
