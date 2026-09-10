# Trainee Tracker

Trainee Tracker is a web application for managing trainee learning assignments, tracking progress, and collecting structured feedback.

It was developed as a six-person university team project at the University of Augsburg, in collaboration with **makandra GmbH**.

## Features

* Role-based access for trainees, mentors, and administrators
* Management of learning curricula, lessons, and lesson assignments
* Tracking of trainee progress and working time
* Feedback creation, editing, and deletion
* Authentication and authorization
* Automated unit and end-to-end tests

## My Contribution

My main responsibility was the **feedback management** feature.

I implemented:

* Feedback creation, editing, and deletion
* Role-based feedback views for trainees, mentors, and administrators
* Assignment status updates after feedback submission
* Unit tests with xUnit
* End-to-end tests with Selenium

## Tech Stack

* C# and .NET 10
* ASP.NET Core MVC
* Razor Views
* Entity Framework Core
* SQLite
* Bootstrap and jQuery
* xUnit, Moq, Coverlet
* Selenium WebDriver
* Docker
* GitLab CI/CD and Portainer deployment

## Run Locally

### Requirements

* .NET 10 SDK
* Docker (optional)

```bash
dotnet restore Trainee_Tracker/Trainee_Tracker.csproj
dotnet run --project Trainee_Tracker/Trainee_Tracker.csproj
```

## Run Tests

```bash
dotnet test Trainee_Tracker.UnitTests/Trainee_Tracker.UnitTests.csproj
dotnet test Trainee_Tracker.E2ETests/Trainee_Tracker.E2ETests.csproj
```

## Team Project

This is a university team project. The application was developed collaboratively; the feedback management functionality and its automated tests were my primary contribution.
