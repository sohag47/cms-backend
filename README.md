
## 🚀 Project Roadmap / To-Do List

This is the planned roadmap for building the project.

### ✅ Phase 1: Project Setup & Architecture
- [X] Create a new **Blank Solution** and project structure.
- [X] Set up **Clean Architecture** layers (`Domain`, `Application`, `Infrastructure`, `API`).
- [X] Configure project references correctly.
- [ ] Create **Domain Entities** (`Product`, `User`, `Order`).
- [ ] Set up **Entity Framework Core** in the `Infrastructure` layer.
- [ ] Create the initial database migration.

### ✅ Phase 2: Building Core Features
- [ ] Set up the **CQRS pattern** using the **MediatR** library.
- [ ] Implement the **Product Feature** (CreateProduct, GetAllProducts, GetProductById).
- [ ] Implement the **User Feature** (RegisterUser).
- [ ] Create corresponding **API Endpoints** in the `Presentation.API` layer.

### ✅ Phase 3: Security & Advanced Features
- [ ] Implement **JWT Authentication** for user login.
- [ ] Implement **Role-based Authorization** (`[Authorize]` attribute).
- [ ] Implement **Global Error Handling** using custom middleware.
- [ ] Add request **Validation** using **FluentValidation**.

### ✅ Phase 4: Deployment & DevOps
- [ ] Create a `Dockerfile` to **containerize** the application.
- [ ] (Optional) Create a `docker-compose.yml` file.
- [ ] Set up a **CI/CD pipeline** with **GitHub Actions** to build and test on push.
- [ ] Configure the pipeline to **deploy** the application to a cloud service (e.g., Azure App Service).

### ✅ Phase 5: Quality & Maintenance
- [ ] Implement **Unit Testing** for application logic using xUnit.
- [ ] Implement structured **Logging** with **Serilog**.
- [ ] Add and customize **Swagger/OpenAPI** for API documentation.

## 🛠️ How to Run Locally

### Prerequisites
- .NET 8 SDK
- SQL Server (or any other database supported by EF Core)
- Docker (optional)

### Steps
1.  **Clone the repository:**
    ```bash
    git clone https://github.com/YOUR_USERNAME/YOUR_REPO.git
    cd YOUR_REPO
    ```
2.  **Configure the database connection:**
    - Open `Presentation/Presentation.API/appsettings.Development.json`.
    - Modify the `DefaultConnection` string to point to your local database instance.

3.  **Apply EF Core migrations:**
    ```bash
    dotnet ef database update --project Infrastructure
    ```
4.  **Run the application:**
    ```bash
    dotnet run --project Presentation/Presentation.API
    ```
5.  **Access the API:**
    - The API will be running at `https://localhost:7001` (or a similar port).
    - Access the Swagger UI for documentation and testing at `https://localhost:7001/swagger`.
