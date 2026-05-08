# EventHub — Event Management System
### ASP.NET Core MVC | C# | SQLite | Bootstrap 5

---

## How to Run (3 steps)

1. **Open** `EventManagement.sln` in Visual Studio 2022
2. **Wait** for NuGet packages to restore automatically
3. **Press F5** — the SQLite database is created automatically on first run

No migration commands needed. No database setup needed.

---

## Features

- **Register / Login / Logout** via ASP.NET Identity
- **Create** events with title, description, date, time, and location
- **View** all your events in a sortable list
- **Edit** any event using a pre-filled form
- **Delete** events with a confirmation page
- **Secure** — each user only sees their own events
- **Persistent** — data survives app restarts (SQLite file: `events.db`)

---

## Project Structure

```
EventManagement/
├── Controllers/
│   ├── HomeController.cs       ← Landing page
│   └── EventsController.cs     ← Full CRUD + [Authorize]
├── Models/
│   ├── Event.cs                ← Data model with validation
│   └── ErrorViewModel.cs
├── Data/
│   └── ApplicationDbContext.cs ← EF Core context (Events + Identity)
├── Views/
│   ├── Shared/_Layout.cshtml   ← Bootstrap 5 navbar + layout
│   ├── Home/Index.cshtml       ← Landing page
│   └── Events/
│       ├── Index.cshtml        ← Event list table
│       ├── Create.cshtml       ← Add event form
│       ├── Edit.cshtml         ← Edit event form
│       ├── Details.cshtml      ← View single event
│       └── Delete.cshtml       ← Delete confirmation
├── Areas/Identity/             ← Login/Register (default UI)
└── Program.cs                  ← App config + auto DB creation
```

---

## Verifying the Database

1. Open **DB Browser for SQLite**
2. Click **Open Database**
3. Navigate to your project folder and open `events.db`
4. You'll see tables: `Events`, `AspNetUsers`, `AspNetRoles`, etc.

---

## MVC Architecture Overview

| Layer      | File(s)                    | Responsibility                        |
|------------|----------------------------|---------------------------------------|
| Model      | `Models/Event.cs`          | Data structure + validation rules     |
| View       | `Views/Events/*.cshtml`    | UI display — forms, tables, buttons   |
| Controller | `Controllers/EventsController.cs` | Handles requests, calls model, returns views |
| Database   | `Data/ApplicationDbContext.cs` | EF Core bridge to SQLite            |
