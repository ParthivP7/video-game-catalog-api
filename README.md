# 🎮 Video Game Catalog – Backend API

A clean, maintainable catalog API for video games built with **ASP.NET Core 8**, using:

- ✅ CQRS architecture (Commands & Queries)
- ✅ Entity-driven for simplicity
- ✅ EF Core 8 (Code-First) with SQLite
- ✅ FluentValidation for input validation
- ✅ Unit Testing with xUnit
- ✅ Seeded data for easy testing
- ✅ REST client support via `.http` file
- ✅ Runs on HTTPS (`https://localhost:7154`)

---

## 🔧 Tech Stack

| Layer         | Technology                        |
|---------------|-----------------------------------|
| API           | ASP.NET Core 8 Web API            |
| Data Access   | EF Core 8 + SQLite (Code First)   |
| Validation    | FluentValidation                  |
| Architecture  | CQRS (Command/Query separation)   |
| Testing       | xUnit                             |
| Tooling       | Rider / VS Code, `.http` client   |

---

## 🧱 Project Structure

```bash
VideoGameCatalog/
├── WebApp/                         # ASP.NET Core API
│   ├── Controllers/                # REST endpoints
│   ├── Properties/                 # launchSettings.json for dev
│   ├── Program.cs                  # DI, Swagger, seeding
│   ├── DbSeeder.cs                 # Preloaded game data
│   ├── appsettings.json            # Base config
│   ├── WebApp.http                 # REST client script
│
├── Application/                    # Application logic
│   ├── Commands/                   # Write operations (CQRS)
│   ├── Queries/                    # Read operations (CQRS)
│   ├── Validators/                 # FluentValidation rules
│   ├── Pipelines/                  # Request logging + validation
│   └── DependencyInjection.cs      # DI extension for MediatR
│
├── Persistence/                    # Infrastructure layer
│   ├── ApplicationDbContext.cs     # EF Core DbContext
│   ├── Configurations/             # Entity mapping
│   ├── Migrations/                 # EF migrations
│
├── Domain/                         # Entity definitions
│   └── VideoGames/VideoGame.cs     # Core game model
│
├── Tests/                          # Unit tests
│   ├── Commands/
│   │   ├── AddVideoGameCommandTests.cs
│   │   ├── UpdateVideoGameCommandTests.cs
│   │   └── DeleteVideoGameCommandTests.cs
│   ├── Queries/
│   │   ├── GetAllVideoGamesQueryTests.cs
│   │   └── GetVideoGameDetailByIdQueryTests.cs
│   ├── Validation/
│   │   └── VideoGameCommandValidatorTests.cs
│
├── .gitignore                     # Git exclusions
├── VideoGameCatalog.sln           # Solution file
└── README.md                      # You're reading it!
````

---

## ▶️ Running the API

### 📦 Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* SQLite (optional — database file will be auto-generated)

> ℹ️ This project targets **.NET 8**
> ✅ Compatible with any patch version (e.g., `8.0.100`, `8.0.204`)

---

## 🎥 Demo

Watch the live demo of the local API running on `https://localhost:7154`:

👉 [Click here to view the demo video](https://github.com/user-attachments/assets/57d2751e-23ad-4d57-85a3-5d6d480c2b4f)

---


### 🚀 Start the API

```bash
cd WebApp
dotnet run
```

* Swagger UI: [https://localhost:7154/swagger](https://localhost:7154/swagger)
* Database: `videogames.db` created and seeded automatically on first run
* CORS: enabled for `localhost`

---

### ⚙️ EF Core Setup (Run Once)

To apply migrations manually:

```bash
dotnet ef database update -p Persistence -s WebApp
```

To install the EF CLI (if not already):

```bash
dotnet tool install --global dotnet-ef
```

---

### 💡 JetBrains Rider / VS Code Tips

If Rider or VS Code doesn't auto-detect `launchSettings.json`:

* ✅ Set Project: `WebApp`
* ✅ Framework: `net8.0`
* ✅ Environment Variable: `ASPNETCORE_ENVIRONMENT=Development`

---

## 🌐 API Endpoints

All routes are under: `/api/videogames`

| Method | Endpoint               | Description                          |
| ------ | ---------------------- | ------------------------------------ |
| GET    | `/api/videogames`      | Get all video games                  |
| GET    | `/api/videogames/{id}` | Get a game by ID                     |
| POST   | `/api/videogames`      | Add a new game (Create DTO)          |
| PUT    | `/api/videogames/{id}` | Update an existing game (Update DTO) |
| DELETE | `/api/videogames/{id}` | Delete a game by ID                  |

---

## 📬 API Test Script (`.http`)

```http
# Get all games
GET https://localhost:7154/video-games

# Get by ID
GET https://localhost:7154/video-games/1

# Create a new game
POST https://localhost:7154/video-games
Content-Type: application/json

{
  "title": "Test Game",
  "genre": "RPG",
  "releaseDate": "2022-01-01"
}

# Update a game
PATCH https://localhost:7154/video-games/1
Content-Type: application/json

{
  "title": "Updated Title",
  "genre": "Adventure",
  "releaseDate": "2023-03-01"
}

# Delete a game
DELETE https://localhost:7154/video-games/1
```

> Supports JetBrains Rider HTTP client and VS Code REST Client.

---

## ✅ Unit Testing

```bash
cd Tests
dotnet test
```

* ✅ `AddVideoGameCommandTests`
* ✅ `UpdateVideoGameCommandTests`
* ✅ `DeleteVideoGameCommandTests`
* ✅ `GetAllVideoGamesQueryTests`
* ✅ `GetVideoGameDetailByIdQueryTests`
* ✅ `VideoGameCommandValidatorTests`

---

## 💡 Highlights

* CQRS-style separation of responsibilities
* MediatR-powered command/query architecture
* Validation via FluentValidation
* Pre-seeded SQLite database for instant testing
* Swagger enabled for dev exploration
* HTTPS + dev launch profile ready
