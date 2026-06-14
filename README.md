SOAP – Smart Travel Planner System
Overview
SOAP (Smart Travel Planner System) is a Service-Oriented Architecture (SOA) web application that helps users plan, organize, and manage trips efficiently. The system allows users to create trips, manage destinations, track budgets, calculate trip durations, and generate smart travel itineraries.

The application consists of:

ASP.NET Core Web API (.NET 9)

Angular Frontend

PostgreSQL Database

Entity Framework Core

JWT Authentication & Authorization

Features
Authentication & Authorization
User Registration

User Login

JWT Token Authentication

Role-Based Authorization (Admin/User)

Trip Management
Create Trips

View Trips

Update Trips

Delete Trips

Budget Management

Duration Calculation

Over-Budget Detection

Location Management
Create Locations

View Locations

Update Locations

Delete Locations

Trip Location Management
Add Locations to Trips

Remove Locations from Trips

View Trip Destinations

Smart Itinerary Generation
Automatically generates travel itineraries

Prioritizes destinations

Organizes visits efficiently

System Architecture
Angular Frontend
        ↓
ASP.NET Core Web API
        ↓
Service Layer
        ↓
Repository Layer
        ↓
PostgreSQL Database
Design Patterns Used
Repository Pattern

Service Layer Pattern

Dependency Injection

DTO Pattern

Technologies Used
Category	Technology
Backend	ASP.NET Core Web API (.NET 9)
Frontend	Angular
Database	PostgreSQL
ORM	Entity Framework Core
Authentication	JWT Bearer Tokens
Authorization	ASP.NET Identity
API Documentation	Swagger
Deployment	Render
Testing	xUnit, NSubstitute, EF Core InMemory
Version Control	Git & GitHub
Project Structure
SOAP
│
├── Controllers
├── Services
├── Repository
├── Models
├── DTOs
├── Data
├── Profiles
├── Migrations
├── frontend
│
└── SOAP.Tests
Folder Description
Folder	Purpose
Controllers	API endpoints
Services	Business logic
Repository	Data access layer
Models	Domain entities
DTOs	Request/Response objects
Data	DbContext configuration
Profiles	AutoMapper mappings
Migrations	EF Core migrations
frontend	Angular application
SOAP.Tests	Unit tests
Database Entities
ApplicationUser
Id
FullName
Email
Trip
Id
Name
Budget
StartDate
EndDate
UserId
Location
Id
Name
Country
EstimatedCost
Priority
VisitDurationHours
TripLocation
Id
TripId
LocationId
Order
ScheduledStartTime
API Endpoints
Authentication
Method	Endpoint
POST	/api/Auth/register
POST	/api/Auth/login
Trips
Method	Endpoint
GET	/api/Trip
GET	/api/Trip/{id}
POST	/api/Trip
PUT	/api/Trip/{id}
DELETE	/api/Trip/{id}
GET	/api/Trip/{id}/cost
GET	/api/Trip/{id}/duration
GET	/api/Trip/{id}/overbudget
Locations
Method	Endpoint
GET	/api/Location
GET	/api/Location/{id}
POST	/api/Location
PUT	/api/Location/{id}
DELETE	/api/Location/{id}
Trip Locations
Method	Endpoint
GET	/api/TripLocation/{tripId}
POST	/api/TripLocation/{tripId}
DELETE	/api/TripLocation/{tripId}/{locationId}
Itinerary
Method	Endpoint
GET	/api/Itinerary/{tripId}
Running the Project Locally
Backend
dotnet restore
dotnet build
dotnet run
Swagger:

https://localhost:7299/swagger
Frontend
cd frontend

npm install

ng serve
Angular:

http://localhost:4200
Testing
The project includes automated unit tests for:

Controllers

Services

Repositories

Technologies
xUnit

NSubstitute

EF Core InMemory

Run tests:

dotnet test
Deployment
Backend
Deployed on Render,First's the frontend, Second is the backend:

https://soap-qj7p.onrender.com/
https://soap-api-qiv2.onrender.com/swagger/index.html

Frontend
Deployed on Render Static Site.

Team Members
Ivona Sareska
Backend Development

Frontend Development

Database Design

Deployment

Testing

Diellza Behadini
Backend Development

Frontend Development

Database Design

Documentation

Testing

Contribution Split:

Ivona Sareska 50%
Diellza Behadini 50%
Mentor
MSc. Florina Asani

South East European University

License
This project was developed as part of the Service Oriented Architecture course at South East European University (SEEU) during the academic year 2025/2026.