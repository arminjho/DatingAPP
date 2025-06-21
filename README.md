# 🧡 Dating App – Full-Stack Web Application

A fully functional dating platform built with **ASP.NET Core Web API** on the backend and **Angular** on the frontend. This application allows users to register, authenticate, browse and like other users, send messages, upload photos with tags, and filter photo galleries. Admin users have access to a dashboard for managing users and approving content.

---

## 🔧 Tech Stack

### 🖥 Frontend:
- Angular (Standalone Components, RxJS, Angular Signals)
- TypeScript, HTML, SCSS
- Bootstrap

### 🗄 Backend:
- ASP.NET Core Web API (.NET 7+)
- Entity Framework Core
- C#
- SQL Server
- AutoMapper
- JWT Authentication & Authorization

---

## ✨ Features

### 👤 User Side
- User registration and login (JWT)
- View other users’ profiles
- Like and unlike users
- Real-time messaging system
- Upload photos with tags
- Filter photos by tag

### 🛠 Admin Panel
- View all users and manage their roles
- Approve or reject uploaded photos
- Add/edit photo tags
- Manage user content visibility

### ⚙️ Technical Highlights
- RESTful API with clean architecture and repository pattern
- Secure role-based authorization using custom middleware
- Angular reactive forms and observable state management
- Image gallery filtering by tags
- Signal-based UI updates for optimal performance

---

## 🚀 Getting Started

### ✅ Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/)
- [Node.js & npm](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)

### 🔧 Backend Setup
```bash
cd DatingWebApp
dotnet restore
dotnet ef database update
dotnet run
```
This project is licensed for educational and demo purposes. Contributions are welcome via pull request or issue.

Developed by Armin Muratspahić.
Feel free to reach out via LinkedIn or email for collaboration.
