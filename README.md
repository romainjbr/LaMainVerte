[![Project Board](https://img.shields.io/badge/Project%20Board-Kanban-blue?style=for-the-badge)](https://github.com/users/romainjbr/projects/4)

# La Main Verte: a Plant Watering Manager

A clean architecture ASP.NET Core web app built with Blazor, EF Core and hosted on Azure, showcasing workflow, maintainability and testing. 

## ▶ Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Development Workflow](#development-workflow)
- [CI with GitHub Actions](#ci)
- [CD with Azure](#cd)
- [Azure Architecture](#azure-architecture)
- [Tech Stack](#tech-stack)
- [Access the Project](#access)

## ▶ Overview <a name="overview"></a>

**La Main Verte** is a small and production-style application built to demonstrate how I approach software development using:

- **Clean Architecture** (Presentation / Core / Infrastructure layers)
- **ASP.NET Core** 
- **Entity Framework Core**
- **Unit Testing with xUnit & Moq**
- **User Interface using Blazor**
- **Continuous Integration**
- **Continuous Deployment using Azure**
- **Kanban Board (Github Project) + Pull Request–based workflow**

The app models a simple plant watering manager where users can create and add their own Plants. The user can perform CRUD operations, and the the app showcases a Dashboard to have a quick and practical overview of all the user's plants and their watering status.
This project also displays how I maintain a clean workflow from idea > ticket > implementation > PR.

## ▶ Architecture <a name="architecture"></a>

Clean Architecture was chosen to structure the project because it:
- keeps business rules independent from frameworks
- improves testability, maintanability and scalability

<details> 
  <summary><strong> $${\color{blue}Check \space the \space Project \space Structure \space Tree:}$$ </strong></summary>
<pre>

├── src
│   ├── Core
│   │   ├── Core.csproj
│   │   ├── Dtos
│   │   │   ├── Plants
│   │   │   │   ├── PlantCreateDto.cs
│   │   │   │   ├── PlantMapper.cs
│   │   │   │   ├── PlantReadDto.cs
│   │   │   │   └── PlantUpdateDto.cs
│   │   │   └── WateringLog
│   │   │       ├── WateringLogCreateDto.cs
│   │   │       ├── WateringLogMapper.cs
│   │   │       ├── WateringLogReadDto.cs
│   │   │       └── WateringLogUpdateDto.cs
│   │   ├── Entities
│   │   │   ├── Plant.cs
│   │   │   └── WateringLog.cs
│   │   ├── Enums
│   │   │   ├── WaterFrequency.cs
│   │   │   ├── WaterFrequencyExtension.cs
│   │   │   └── WaterStatus.cs
│   │   ├── Interface
│   │   │   ├── Repositories
│   │   │   │   ├── IPlantRepository.cs
│   │   │   │   ├── IRepository.cs
│   │   │   │   └── IWateringLogRepository.cs
│   │   │   └── Services
│   │   │       ├── IPlantImageService.cs
│   │   │       ├── IPlantService.cs
│   │   │       └── IWateringLogService.cs
│   │   └── Services
│   │       ├── PlantService.cs
│   │       └── WateringLogService.cs
│   ├── Infrastructure
│   │   ├── Data
│   │   │   ├── PlantDbContext.cs
│   │   │   └── SeedData.cs
│   │   ├── Infrastructure.csproj
│   │   ├── Migrations
│   │   │   ├── 20251203122512_InitialCreate.Designer.cs
│   │   │   ├── 20251203122512_InitialCreate.cs
│   │   │   └── PlantDbContextModelSnapshot.cs
│   │   ├── Repositories
│   │   │   ├── EfRepository.cs
│   │   │   ├── PlantRepository.cs
│   │   │   └── WateringLogRepository.cs
│   │   └── Service
│   │       └── AzurePlantImageService.cs
│   └── Presentation
│       ├── Components
│       │   └── Pages
│       │       └── Dashboard
│       │           └── Models
│       │               ├── LocationStat.cs
│       │               └── WateringFrequencyStat.cs
│       ├── Model
│       │   └── PlantFormModel.cs
│       ├── Presentation.csproj
│       └── Program.cs
└── test
    ├── Core.Tests
    │   ├── Core.Tests.csproj
    │   └── Services
    │       ├── PlantServiceTests.cs
    │       └── WateringLogServiceTests.cs
    ├── Infrastructure.Tests
    │   ├── Infrastructure.Tests.csproj
    │   └── Repositories
    │       ├── EfRepositoryPlantTests.cs
    │       └── EfRepositoryWateringLogTests.cs
    └── Presentation.Tests
    |   ├── Components
    │   └── Pages
    │       ├── Dashboard
    │       │   └── DashboardPageTest.cs
    │       └── Plant
    │           ├── EditPlantPageTests.cs
    │           ├── PlantDetailsPageTests.cs
    │           └── PlantsPageTests.cs
    └── Presentation.Tests.csproj
</pre>
</details>

## ▶ Development Workflow <a name="development-workflow"></a>

This repository uses a GitHub Project board to organise work:
- Each task is represented as a ticket (issue)
- Every feature/fix is developed on a separate branch
- Each task involves a Pull Request referencing the issue
- PRs requires the pipeline to pass before merge
- Once the Pipleine passes, it is automatically deployed

[Check the board here!](https://github.com/users/romainjbr/projects/4)

## ▶ CI with GitHub Actions <a name="ci"></a>

A ci.yml workflow automatically runs on every:
- Push
- Pull Request

The pipeline performs:
- Restore
- Build
- Run unit tests

This ensures consistent quality and prevents regressions.

## ▶ CD with Azure <a name="cd"></a>

CD is handled using GitHub Actions and Azure App Service.

After a successful CI pipeline:
- The application is automatically deployed to Azure App Service
- The main branch represents the production-ready version
- No manual deployment steps are required
- Application settings and secrets (connection strings and storage keys) are managed via Azure App Service configuration, keeping sensitive data out of the repository

## ▶ Azure Architecture <a name="azure-architecture"></a>

The project makes use of three main Azure Services:
- **App Service** to host the Blazor Service
- **SQL Database** to contain the EF Core data
- **Blob Container** to contain the Plants images 

The following diagram represents the Azure setup for this project:

```mermaid
flowchart LR
    User -->|HTTPS| AppService[Azure App Service<br/>Blazor Server]
    AppService -->|EF Core| SqlDb[Azure SQL Database]
    AppService -->|BlobClient| Storage[Azure Storage Account<br/>Blob Container]
```` 


## ▶ Tech Stack <a name="tech-stack"></a>
- **Backend:** ASP.NET Core, C#  
- **Architecture:** Clean Architecture (Core / Infrastructure / Presentation)  
- **Database:** Entity Framework Core 
- **Testing:** xUnit, Moq  
- **CI:** GitHub Actions
- **CD:** Azure (AppService, SQL Database, Blob Container)
- **Project Management:** GitHub Projects (Kanban Board), Pull Request workflow  

## ▶ Access project <a name="access"></a>

Access the web app on the following link: https://lamainverteapp-hqfuf9e6etgad4cu.westeurope-01.azurewebsites.net/dashboard


  
