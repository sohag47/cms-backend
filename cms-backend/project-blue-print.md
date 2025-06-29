
# 🛒 E-Commerce Web Application – Learning Project

## 🔧 Tech Stack Overview

| Part       | Tech                                |
|------------|-------------------------------------|
| Frontend   | React + Vite + Tailwind CSS         |
| Backend    | ASP.NET Web API (.NET Framework 4.8)|
| Database   | SQL Server + Entity Framework 6     |
| Auth       | ASP.NET Identity + JWT              |
| API Comm.  | JSON over HTTP (Axios/Fetch)        |

> 🔗 You’ll need to enable CORS on the backend for frontend access.

---

## 📁 Backend Structure (ASP.NET Web API – .NET Framework)

```
/ECommerce.API
|-- /Controllers
|-- /Models
|-- /DTOs
|-- /DAL (EF DbContext + Repositories)
|-- /Services (Business logic)
|-- /Helpers (JWT, Utilities)
|-- /ViewModels
|-- Web.config
```

---

## 📁 Frontend Structure (React + Tailwind CSS)

```
/ecommerce-frontend
|-- /src
|   |-- /components
|   |-- /pages
|   |-- /services (API calls)
|   |-- /contexts (auth, cart)
|   |-- /hooks
|   |-- App.tsx
|-- tailwind.config.js
|-- vite.config.js
```

---

## 🔐 Authentication Flow

- User logs in → API returns **JWT**
- Store JWT in `localStorage`
- Send token in request header:
  ```
  Authorization: Bearer <token>
  ```
- Use React Context for auth state

---

## 📦 Backend API Endpoints

| Endpoint                     | Method | Description                     |
|-----------------------------|--------|---------------------------------|
| `/api/auth/login`           | POST   | User login, return JWT          |
| `/api/auth/register`        | POST   | Register new user               |
| `/api/products`             | GET    | List all products               |
| `/api/products/{id}`        | GET    | Product details                 |
| `/api/categories`           | GET    | Product categories              |
| `/api/cart`                 | POST   | Add item to cart                |
| `/api/cart`                 | GET    | View user's cart                |
| `/api/orders`               | POST   | Place an order                  |
| `/api/orders/user`          | GET    | View user's order history       |
| `/api/admin/products`       | POST   | Admin: create new product       |
| `/api/admin/orders`         | GET    | Admin: view all orders          |

---

## 🧩 Frontend Pages & Features

| Page               | Description                            |
|--------------------|----------------------------------------|
| Home               | List products & categories             |
| Product Details    | Show product info, images, add to cart |
| Cart               | View/edit/remove cart items            |
| Checkout           | Enter address, confirm order           |
| Order Success      | Show order summary                     |
| Login/Register     | Auth pages                             |
| Profile/Orders     | View user info & order history         |
| Admin Dashboard    | Admin: manage products/orders          |

---

## 🧾 Database Schema (Simplified)

- **User** (from Identity)
- **Category**: `Id`, `Name`
- **Product**: `Id`, `Name`, `Price`, `CategoryId`, `Stock`, `ImageUrl`, `Description`
- **CartItem**: `Id`, `UserId`, `ProductId`, `Quantity`
- **Order**: `Id`, `UserId`, `TotalAmount`, `Status`, `OrderDate`
- **OrderItem**: `Id`, `OrderId`, `ProductId`, `Quantity`, `Price`

---

## 🌐 Enable CORS in Web API (WebApiConfig.cs)

```csharp
public static void Register(HttpConfiguration config)
{
    config.EnableCors(new EnableCorsAttribute("*", "*", "*")); // For development
}
```

Install CORS NuGet package:

```
Install-Package Microsoft.AspNet.WebApi.Cors
```

---

## 🚀 Suggested Roadmap

### ✅ Backend
- [ ] Create ASP.NET Web API Project (.NET Framework 4.8)
- [ ] Setup Entity Framework 6 and SQL Server
- [ ] Create models, DTOs, and DbContext
- [ ] Setup JWT-based authentication
- [ ] Build Product, Category, Cart, and Order APIs
- [ ] Add role-based auth for admin/user

### ✅ Frontend
- [ ] Scaffold React + Tailwind project (Vite)
- [ ] Create Auth and Cart Contexts
- [ ] Build pages: Home, Product, Cart, Checkout
- [ ] Connect with backend using Axios
- [ ] Protect routes based on login state

---

## 🌟 Optional Features
- [ ] Product ratings & reviews
- [ ] Wishlist
- [ ] Admin product image upload with preview
- [ ] Email confirmation on order
- [ ] Dashboard with charts (Chart.js)
- [ ] Coupons / discount system

---

## ✅ Tools to Use

- **Postman**: Test backend APIs
- **JWT.io**: Decode/verify JWT tokens
- **SQL Server Management Studio (SSMS)**: Manage DB
- **GitHub**: Version control
- **Figma** *(Optional)*: UI planning

---

## 📚 Bonus Tips

- Use `.env` in frontend for storing API URLs.
- Keep `localStorage` access in one place (auth context).
- For image uploads: backend receives `multipart/form-data` and stores images in `/Images`.
