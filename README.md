# VillustraTask

VillustraTask is a full-stack application built with ASP.NET Core 6.0, SQL Server, Dapper, and Bootstrap. The solution includes a database project, a RESTful API, and an MVC web interface for managing users and tasks.

## Features

- **Database Project**
  - SQL scripts to create the "VillustraTask" database.
  - Table definitions for `Userlogin` and `Tasks`.
  - Stored procedures for CRUD operations and data seeding.

- **API Project**
  - Built with ASP.NET Core 6.0.
  - RESTful endpoints for user authentication (JWT-based), user registration, and task management (create, update, delete).
  - Uses Dapper for data access.
  - Sends email notifications for task assignments.

- **Web Interface**
  - Built with ASP.NET Core MVC and Bootstrap.
  - Supports login, user registration, and task management.
  - Uses DataTables for task display and interactivity.

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server (or LocalDB)
- Git

## Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Osamamicro/VillustraTask.git
