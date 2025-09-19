📖 BlogAI – ASP.NET Core Web API
🚀 Giới thiệu

BlogAI là một Web API viết bằng ASP.NET Core với các tính năng:

Quản lý người dùng (User, Admin).

CRUD bài viết blog (thêm, sửa, xóa, xem).

Authentication với JWT.

Database SQLite (Entity Framework Core).

Tích hợp Swagger UI để test API.

🛠 Yêu cầu hệ thống

.NET 9 SDK

SQLite (tích hợp sẵn, không cần cài thêm)

Git

📂 Cấu trúc thư mục
BlogAI/
│   Program.cs
│   BlogAI.csproj
│   appsettings.json
│   README.md
│
├── Controllers/
│     BlogController.cs
│     AuthController.cs
│
├── Models/
│     User.cs
│     BlogPost.cs
│
├── Data/
│     AppDbContext.cs
│
└── Migrations/
      ...

▶️ Cách chạy dự án
1. Clone repo
git clone https://github.com/YOUR-USERNAME/BlogAI.git
cd BlogAI

2. Cài package & tạo database
dotnet restore
dotnet ef database update

3. Chạy project
dotnet run


API chạy tại:
👉 https://localhost:5001/swagger
👉 http://localhost:5000/swagger

⚠️ Trang chủ (/) sẽ tự redirect sang /swagger.

🔑 Tài khoản seeding sẵn

Khi chạy lần đầu, database có dữ liệu mặc định:

Username	Password	Role
admin	123456	Admin
user1	123456	User
user2	123456	User
📌 API Endpoints
Auth

POST /api/auth/register – Đăng ký

POST /api/auth/login – Đăng nhập (trả về JWT token)

Blog

GET /api/blog – Lấy tất cả bài viết

GET /api/blog/{id} – Lấy chi tiết bài viết

POST /api/blog – Thêm bài viết (cần JWT)

PUT /api/blog/{id} – Sửa bài viết (chủ sở hữu hoặc Admin)

DELETE /api/blog/{id} – Xóa bài viết (chủ sở hữu hoặc Admin)
