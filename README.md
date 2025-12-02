# IronAmbit

A modern workout tracking application with a .NET backend API and an AstroJs frontend.

## 🏗️ Architecture

IronAmbit is a full-stack application consisting of two main components:

- **Client** - Modern web frontend built with AstroJs and React
- **Service** - RESTful API backend built with .NET 10

## 📋 Overview

IronAmbit helps users track their fitness journey by managing:
- **Users** - User accounts and profiles
- **Exercises** - Exercise library with categories, muscle groups, and difficulty levels
- **Workouts** - Daily workout logs with sets, reps, weight, and duration tracking

---

## 🎨 Client (Frontend)

### Technology Stack

- **Framework**: [Astro](https://astro.build/) 5.16.2
- **UI Library**: React 19.2.0
- **Styling**: Tailwind CSS 4.1.17
- **Language**: TypeScript
- **Package Manager**: npm

### Features

- 📊 Interactive data tables with pagination
- 🔄 Loading states and error handling
- 📱 Responsive design for mobile and desktop
- 🎯 Type-safe API integration
- 🧪 Mock API for development and testing

### Prerequisites

- Node.js 18.x or higher
- npm 9.x or higher

### Setup

1. Navigate to the Client directory:
   ```bash
   cd Client
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Configure environment variables:
   ```bash
   cp .env.example .env
   ```

4. Edit `.env` and set your API URL:
   ```env
   PUBLIC_API_URL=http://localhost:8080/api
   ```

### Development

Run the development server:

```bash
npm run dev
```

The application will be available at: **http://localhost:4321**

### Available Scripts

- `npm run dev` - Start development server with hot reload
- `npm run build` - Build for production
- `npm run preview` - Preview production build locally
- `npm run lint` - Lint code with Biome
- `npm run format` - Format code with Biome
- `npm run check` - Check and fix code issues

### Project Structure

```
Client/
├── src/
│   ├── components/       # React components and Astro components
│   │   ├── DataTable.tsx        # Reusable data table with pagination
│   │   ├── UsersTable.tsx       # Users management table
│   │   ├── ExercisesTable.tsx   # Exercises library table
│   │   └── WorkoutsTable.tsx    # Workouts tracking table
│   ├── pages/           # Astro pages (routes)
│   │   ├── users.astro          # /users page
│   │   ├── exercises.astro      # /exercises page
│   │   └── workouts.astro       # /workouts page
│   ├── layouts/         # Page layouts
│   ├── types/           # TypeScript type definitions
│   ├── utils/           # Utility functions and API services
│   │   ├── api.ts              # API service layer
│   │   └── mockApi.ts          # Mock API for testing
│   └── styles/          # Global styles
├── public/              # Static assets
└── package.json
```

### Testing Without Backend

The client includes a mock API service for development without a backend:

1. See `MOCK_API_GUIDE.md` for instructions on switching between mock and real API
2. Use `DemoUsersTable.tsx` as an example component with mock data
3. Mock data is available in `src/utils/mockApi.ts`

### Documentation

- `GETTING_STARTED.md` - Quick start guide
- `API_INTEGRATION.md` - API integration details
- `API_ENDPOINTS.md` - Expected API endpoint specifications
- `IMPLEMENTATION_SUMMARY.md` - Architecture overview
- `MOCK_API_GUIDE.md` - How to use mock data

---

## ⚙️ Service (Backend)

### Technology Stack

- **Framework**: .NET 10
- **API Type**: RESTful Web API
- **Language**: C#
- **Database**: TBD (SQL Server / PostgreSQL / SQLite)

### Features

- 🔌 RESTful API endpoints
- 📄 Paginated responses
- 🔐 Data validation
- 🛡️ Error handling
- 📊 Supports Users, Exercises, and Workouts management

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- IDE: Visual Studio 2025, Visual Studio Code, or JetBrains Rider

### Setup

1. Navigate to the Service directory:
   ```bash
   cd Service
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Configure connection strings and settings in `appsettings.json` or `appsettings.Development.json`

### Development

Run the development server:

```bash
dotnet run
```

Or with hot reload:

```bash
dotnet watch run
```

The API will be available at: **http://localhost:8080** (or the port specified in your launch settings)

### Available Commands

- `dotnet run` - Run the application
- `dotnet watch run` - Run with hot reload
- `dotnet build` - Build the project
- `dotnet test` - Run unit tests
- `dotnet publish -c Release` - Publish for production

### API Endpoints

#### Users
- `GET /api/users?page=1&pageSize=10` - Get paginated users

#### Exercises
- `GET /api/exercises?page=1&pageSize=10` - Get paginated exercises

#### Workouts
- `GET /api/workouts?page=1&pageSize=10` - Get paginated workouts

For complete API specifications, see `Client/API_ENDPOINTS.md`

### Expected Response Format

All paginated endpoints return:

```json
{
  "data": [...],
  "total": 100,
  "page": 1,
  "pageSize": 10,
  "totalPages": 10
}
```

### CORS Configuration

Ensure CORS is configured to allow requests from the frontend:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4321")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors("AllowFrontend");
```

---

## 🚀 Running the Full Stack

### Development Mode

1. **Start the Backend Service:**
   ```bash
   cd Service
   dotnet watch run
   ```
   Service runs on: http://localhost:8080

2. **Start the Frontend Client** (in a new terminal):
   ```bash
   cd Client
   npm run dev
   ```
   Client runs on: http://localhost:4321

3. **Access the Application:**
   - Frontend: http://localhost:4321
   - Backend API: http://localhost:8080/api
   - Swagger UI: http://localhost:8080/swagger (if configured)

### Production Build

#### Client:
```bash
cd Client
npm run build
```

Output will be in `Client/dist/`

#### Service:
```bash
cd Service
dotnet publish -c Release -o ./publish
```

Output will be in `Service/publish/`

---

## 🛠️ Development Workflow

1. **Backend Development:**
   - Implement API endpoints in the Service project
   - Follow the response format specified in `Client/API_ENDPOINTS.md`
   - Test endpoints using Swagger UI or tools like Postman/curl

2. **Frontend Development:**
   - Use mock API during initial development
   - Connect to real backend once endpoints are ready
   - Update components in `Client/src/components/`
   - Add new pages in `Client/src/pages/`

3. **Integration Testing:**
   - Start both Client and Service
   - Verify data flows correctly
   - Test pagination, error handling, and loading states

---

## 📦 Project Structure

```
IronAmbit/
├── Client/                 # Frontend application
│   ├── src/
│   ├── public/
│   ├── package.json
│   └── README.md
├── Service/                # Backend API
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── appsettings.json
│   └── Program.cs
├── README.md              # This file
├── LICENSE
└── CHANGELOG.md
```

---

## 🧪 Testing

### Client Testing
```bash
cd Client
npm run check        # Type checking
npm run lint         # Linting
```

### Service Testing
```bash
cd Service
dotnet test          # Run unit tests
```

---

## 📝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- Built with [Astro](https://astro.build/)
- Powered by [.NET](https://dotnet.microsoft.com/)
- Styled with [Tailwind CSS](https://tailwindcss.com/)

---

## 📧 Contact

Project Link: [https://github.com/YenkoTools/IronAmbit](https://github.com/YenkoTools/IronAmbit)

---

## 🗺️ Roadmap

- [x] Client setup with Astro and React
- [x] API integration layer with pagination
- [x] Users management page
- [x] Exercises management page
- [x] Workouts management page
- [ ] Service API implementation
- [ ] Database integration
- [ ] User authentication
- [ ] Create/Edit/Delete functionality
- [ ] Advanced filtering and search
- [ ] Workout calendar view
- [ ] Exercise video demonstrations
- [ ] Progress tracking and analytics
- [ ] Mobile application
