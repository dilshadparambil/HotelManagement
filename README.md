# 🏨 Hotel Management System – Full Stack Web Application

A comprehensive **Hotel Management System** built with **Angular (Frontend)** and **.NET Web API (Backend)** to streamline hotel operations including room booking, guest management, and reservation tracking.

🚀 **Mini Project 1** – Built during internship at **Geekywolf**

---

## ✅ Features

| Feature | Description |
|---------|-------------|
| 🏨 Room Management | Create, Read, Update, Delete (CRUD) hotel rooms with details |
| 📅 Booking System | Manage reservations with check-in/check-out dates |
| 👥 Guest Management | Store and manage guest information and profiles |
| 🔍 Room Availability | Real-time room availability tracking and search |
| 💰 Pricing Management | Dynamic pricing based on room types and seasons |
| 📊 Booking Status | Track booking status (Confirmed, Pending, Cancelled, Completed) |
| 🛏️ Room Types | Support for multiple room categories (Single, Double, Suite, Deluxe) |
| 📱 Responsive Design | Mobile-friendly interface with SCSS styling |
| 🔐 RESTful API | Clean REST API architecture with ASP.NET Core |
| 💾 Database Integration | Persistent data storage using Entity Framework Core |

---

## 🛠️ Tech Stack

**Frontend:** Angular, TypeScript, HTML, SCSS, RxJS, Bootstrap  
**Backend:** ASP.NET Core Web API (.NET, C#)  
**Database:** SQL Server / SQLite (configurable via Entity Framework)  
**ORM:** Entity Framework Core  
**Tools:** Git, GitHub, VS Code / Visual Studio, Node.js, npm  

---

## 📂 Project Structure

```
HotelManagement/
│
├── Frontend/                    # Angular Application
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/      # Angular components
│   │   │   │   ├── rooms/       # Room management
│   │   │   │   ├── bookings/    # Booking management
│   │   │   │   ├── guests/      # Guest management
│   │   │   │   └── dashboard/   # Dashboard & statistics
│   │   │   ├── services/        # API services
│   │   │   ├── models/          # TypeScript models/interfaces
│   │   │   ├── guards/          # Route guards
│   │   │   └── ...
│   │   ├── assets/              # Images & static files
│   │   ├── environments/        # Environment configurations
│   │   └── styles.scss          # Global styles
│   ├── angular.json
│   ├── package.json
│   ├── tsconfig.json
│   └── ...
│
├── Backend/                     # .NET Web API Backend
│   ├── Controllers/             # API endpoints
│   │   ├── RoomsController.cs
│   │   ├── BookingsController.cs
│   │   └── GuestsController.cs
│   ├── Models/                  # Data models
│   │   ├── Room.cs
│   │   ├── Booking.cs
│   │   └── Guest.cs
│   ├── Data/                    # DbContext & configurations
│   │   └── HotelDbContext.cs
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Services/                # Business logic layer
│   ├── Migrations/              # EF Core migrations
│   ├── appsettings.json         # Configuration file
│   ├── Program.cs               # Application entry point
│   └── HotelManagement.csproj   # Project file
│
├── README.md
└── .gitignore
```

---

## ⚙️ Installation & Setup (Local Development)

### 📋 Prerequisites

- **.NET SDK** (6.0 or higher) - [Download](https://dotnet.microsoft.com/download)
- **Node.js** (16.x or higher) & **npm** - [Download](https://nodejs.org/)
- **Angular CLI** - Install via: `npm install -g @angular/cli`
- **SQL Server** / **SQLite** (for database)
- **Git** - [Download](https://git-scm.com/)

---

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/dilshadparambil/HotelManagement.git
cd HotelManagement
```

---

### 🔹 Backend Setup (.NET API)

#### 2️⃣ Navigate to Backend Folder

```bash
cd Backend
```

#### 3️⃣ Restore Dependencies

```bash
dotnet restore
```

#### 4️⃣ Configure Database Connection

Edit `appsettings.json` to set up your database:

**For SQL Server:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelManagementDB;Trusted_Connection=True;"
  }
}
```

**For SQLite (simpler setup):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=hotelmanagement.db"
  }
}
```

#### 5️⃣ Apply Database Migrations

```bash
dotnet ef database update
```

If `dotnet ef` tools are not installed:

```bash
dotnet tool install --global dotnet-ef
```

#### 6️⃣ Run the Backend API

```bash
dotnet run
```

✅ **API will be available at:**
- `https://localhost:5001` (HTTPS)
- `http://localhost:5000` (HTTP)

---

### 🔹 Frontend Setup (Angular)

#### 7️⃣ Navigate to Frontend Folder

```bash
cd ../Frontend
```

#### 8️⃣ Install Dependencies

```bash
npm install
```

#### 9️⃣ Configure API Base URL

Update the backend API endpoint in:

**`src/environments/environment.ts`**

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'  // Adjust if needed
};
```

#### 🔟 Run Angular Application

```bash
ng serve
```

Or:

```bash
npm start
```

✅ **Open browser at:**
- `http://localhost:4200/`

---

## 🎯 Usage Guide

### For Hotel Staff/Admins:

1. **Manage Rooms**
   - Add new rooms with details (room number, type, price, amenities)
   - Update room information and availability status
   - Delete rooms that are no longer available

2. **Handle Bookings**
   - Create new reservations for guests
   - View all bookings with check-in/check-out dates
   - Update booking status (confirm, cancel, complete)
   - Search bookings by guest name or date range

3. **Guest Management**
   - Register new guests with contact information
   - View guest history and past bookings
   - Update guest profiles

4. **Dashboard**
   - View room occupancy statistics
   - Monitor upcoming check-ins and check-outs
   - Track revenue and booking trends

---

## 🔑 API Endpoints (Backend)

### Rooms API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/rooms` | Get all rooms |
| GET | `/api/rooms/{id}` | Get specific room details |
| GET | `/api/rooms/available` | Get available rooms |
| POST | `/api/rooms` | Create a new room |
| PUT | `/api/rooms/{id}` | Update room information |
| DELETE | `/api/rooms/{id}` | Delete a room |

### Bookings API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/bookings` | Get all bookings |
| GET | `/api/bookings/{id}` | Get specific booking |
| GET | `/api/bookings/guest/{guestId}` | Get bookings by guest |
| POST | `/api/bookings` | Create new booking |
| PUT | `/api/bookings/{id}` | Update booking |
| DELETE | `/api/bookings/{id}` | Cancel booking |

### Guests API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/guests` | Get all guests |
| GET | `/api/guests/{id}` | Get specific guest |
| POST | `/api/guests` | Register new guest |
| PUT | `/api/guests/{id}` | Update guest information |
| DELETE | `/api/guests/{id}` | Delete guest |

---

## 🚀 Deployment Options

### Backend Deployment:
- **Azure App Service** (recommended for .NET applications)
- **Render.com** (supports .NET with Docker)
- **Railway.app**
- **AWS Elastic Beanstalk**
- **DigitalOcean App Platform**

### Frontend Deployment:
- **Vercel** (easiest for Angular)
- **Netlify**
- **Azure Static Web Apps**
- **GitHub Pages**
- **Firebase Hosting**
- **AWS Amplify**

### Database for Production:
- **Azure SQL Database** (recommended with Azure)
- **PostgreSQL** (on Render/Railway)
- **AWS RDS**
- **Google Cloud SQL**

---

## 🐳 Docker Setup (Optional)

### Backend Dockerfile

Create `Dockerfile` in the `Backend` folder:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HotelManagement.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelManagement.dll"]
```

### Docker Compose

Create `docker-compose.yml` in the root directory:

```yaml
version: '3.8'

services:
  database:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Password
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

  backend:
    build: ./Backend
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=database;Database=HotelManagementDB;User=sa;Password=YourStrong@Password;
    depends_on:
      - database

  frontend:
    build: ./Frontend
    ports:
      - "4200:80"
    depends_on:
      - backend

volumes:
  sqldata:
```

### Run with Docker

```bash
docker-compose up --build
```

---

## 🧪 Testing

### Backend Tests

```bash
cd Backend.Tests
dotnet test
```

### Frontend Tests

```bash
cd Frontend
npm test                    # Run unit tests
npm run test:coverage      # Generate coverage report
npm run e2e                # Run end-to-end tests
```

---

## 💡 Future Enhancements

- ✅ **User Authentication & Authorization** (Role-based access: Admin, Staff, Guest)
- ✅ **Payment Integration** (Stripe, PayPal for online payments)
- ✅ **Email Notifications** (Booking confirmations, reminders)
- ✅ **SMS Alerts** (Check-in/check-out reminders)
- ✅ **Room Service Management** (Order food, housekeeping requests)
- ✅ **Reports & Analytics** (Revenue reports, occupancy trends)
- ✅ **Multi-language Support** (i18n internationalization)
- ✅ **Invoice Generation** (PDF invoices for bookings)
- ✅ **Calendar View** (Visual booking calendar)
- ✅ **Rating & Reviews** (Guest feedback system)
- ✅ **Loyalty Program** (Reward points for frequent guests)
- ✅ **Mobile App** (Native iOS/Android app)
- ✅ **AI-powered Recommendations** (Suggest rooms based on preferences)
- ✅ **Integration with OTAs** (Booking.com, Airbnb APIs)

---

## 🛠️ Troubleshooting

### Common Issues & Solutions

**Issue:** Database connection fails  
**Solution:** 
- Verify connection string in `appsettings.json`
- Ensure SQL Server/SQLite is running
- Check firewall settings for SQL Server

**Issue:** CORS errors when calling API  
**Solution:** Configure CORS in `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// After app is built
app.UseCors("AllowAngular");
```

**Issue:** Angular CLI commands not working  
**Solution:** Install Angular CLI globally:
```bash
npm install -g @angular/cli
```

**Issue:** Migration fails  
**Solution:** 
```bash
# Remove existing migrations
dotnet ef migrations remove

# Create fresh migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

**Issue:** Port already in use  
**Solution:** 
- Change port in `launchSettings.json` (Backend)
- Change port in `angular.json` or use: `ng serve --port 4201` (Frontend)

---

## 📚 Learning Resources

### Angular
- [Official Angular Documentation](https://angular.io/docs)
- [Angular Tutorial: Tour of Heroes](https://angular.io/tutorial)
- [RxJS Documentation](https://rxjs.dev/)

### .NET
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)

---

## 🤝 Contributing

Contributions make the open-source community amazing! Any contributions you make are **greatly appreciated**.

### How to Contribute:

1. Fork the Project
2. Create your Feature Branch
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. Commit your Changes
   ```bash
   git commit -m 'Add some AmazingFeature'
   ```
4. Push to the Branch
   ```bash
   git push origin feature/AmazingFeature
   ```
5. Open a Pull Request

### Contribution Guidelines:
- Write clear, descriptive commit messages
- Follow existing code style and conventions
- Add tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting PR

---

## 📜 License

This project is open-source and available under the **MIT License**.

```
MIT License

Copyright (c) 2024 Dilshad P

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

## 📞 Contact & Support

**Developer:** Dilshad P  
**GitHub:** [@dilshadparambil](https://github.com/dilshadparambil)

### Get Help:
- 🐛 [Report a Bug](https://github.com/dilshadparambil/HotelManagement/issues)
- 💡 [Request a Feature](https://github.com/dilshadparambil/HotelManagement/issues)

---

## ⭐ Show Your Support

If this project helped you learn or build something awesome:

- ⭐ **Star** the repository
- 🍴 **Fork** it for your own projects
- 📢 **Share** with others who might find it useful
- 🐛 **Report bugs** or suggest features

---

## 👨‍💻 Developed by [Dilshad P](https://github.com/dilshadparambil)

Built as **Mini Project 1** during internship at **Geekywolf**  
Learning full-stack development with **Angular** & **.NET Core**

**Tech Stack Mastered:**
- Frontend: Angular, TypeScript, RxJS, SCSS
- Backend: ASP.NET Core, Entity Framework, C#
- Database: SQL Server, SQLite
- Tools: Git, Docker, Visual Studio Code

---

## 🙏 Acknowledgments

- **Geekywolf** - For the internship opportunity and mentorship
- **Angular Team** - For the powerful frontend framework
- **Microsoft** - For .NET Core and excellent documentation
- **Stack Overflow Community** - For troubleshooting help
- **GitHub** - For hosting this project

---

## 📊 Project Statistics

![C#](https://img.shields.io/badge/C%23-43.9%25-purple)
![TypeScript](https://img.shields.io/badge/TypeScript-26.6%25-blue)
![SCSS](https://img.shields.io/badge/SCSS-15.4%25-pink)
![HTML](https://img.shields.io/badge/HTML-14.1%25-orange)

---

## 🌟 Key Learnings

During this project, I gained hands-on experience with:

- **Full-stack Development:** Building complete applications from database to UI
- **RESTful API Design:** Creating clean, maintainable API endpoints
- **Angular Framework:** Component architecture, services, and reactive programming
- **Entity Framework:** Database migrations, relationships, and LINQ queries
- **Problem Solving:** Debugging, testing, and optimizing code
- **Version Control:** Git workflows and collaboration
- **Software Architecture:** Separating concerns, dependency injection, MVC pattern

---

**Happy Coding! 🏨✨**
