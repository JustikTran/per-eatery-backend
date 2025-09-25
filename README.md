# Per Eatery Backend

Backend API cho hệ thống **Eatery** sử dụng **.NET 8** và được container hóa bằng **Docker / Docker Compose**.

---

## Mô tả dự án

Dự án backend hỗ trợ các chức năng như quản lý món ăn, quản lý đơn hàng, người dùng, v.v.  
Xây dựng theo kiến trúc nhiều lớp (Clean Architecture / Onion / n-layer):  
- `Api/`: Web API (Controllers, startup)  
- `Application/`: Interfaces, Service
- `Domain/`: Entities, DTOs   
- `Infrastructure/`: Cơ sở dữ liệu, Repository, EF Core  

---

## Công nghệ & công cụ sử dụng

- .NET 8 (ASP.NET Core)  
- OData
- Docker  
- Database (PostgreSQL)  

---

## Cấu trúc thư mục

- `Backend.sln`
- `Api/` # Web API (Controllers, Swagger, Middleware, Startup)
- `Application/` # Business logic, DTOs, Services
- `Domain/` # Entities, Interfaces
- `Infrastructure/` # EF Core, Repository, Database context
- `docker-compose.yml` # File cấu hình Docker Compose


---

## Cấu hình

File `appsettings.json` trong `Api/` chứa thông tin cấu hình chính.  

```json
{
  "ConnectionStrings": {
    "EateryContext": "Host=db;Port=5432;Database=eateryDb;Username=admin;Password=admin@123"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
---
## Hướng dẫn chạy
#### 1. Clone dự án
```bash
git clone https://github.com/JustikTran/per-eatery-backend.git
cd per-eatery-backend
```
#### 2. Chạy với docker-compose
```bash
docker-compose up --build -d
```

Lệnh trên sẽ:

- Build image cho API
- Tạo container DB (SQL Server)
- Liên kết API với DB qua mạng nội bộ

#### 3. Truy cập API

API: `https://localhost:8000`

---

## License

Dự án sử dụng [MIT License](LICENSE).
