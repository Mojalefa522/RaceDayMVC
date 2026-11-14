# RaceDay Platform - Full Stack Solution

## Project Overview
RaceDay is a full-stack web application designed to manage sporting events (runs, walks, cycles). It consists of a RESTful API backend and an ASP.NET Core MVC frontend. The system allows users to register as either an Organiser or a Participant, manage events, enroll in categories, and track results.

## User Roles

### 1. Organiser
- Create, update, and delete events.
- Add categories to events (e.g., 5km, 10km, Half Marathon).
- View all participants enrolled in their specific events.
- Capture and publish finish times and positions for participants after an event concludes.

### 2. Participant
- Browse and view all upcoming events and available categories.
- Enrol in an event by selecting a specific category.
- View a personal dashboard of their enrolments and status.
- View their personal race history and results across completed events.

## Setup and Run Instructions

### Prerequisites
- .NET 10.0 SDK
- SQLite (integrated via Entity Framework Core)

### Running Locally
1. Clone the repository to your local machine.
2. Open the solution file `RaceDayMVC.slnx` in Visual Studio.
3. Ensure both `RaceDayAPI` and `RaceDayMVC` are set as **Startup Projects** (Right-click Solution -> Properties -> Multiple Startup Projects).
4. The API will run on `https://localhost:7270` and the MVC app will run on `https://localhost:7003`.
5. Press `F5` to launch both applications simultaneously. The database will automatically seed with default roles on the first run.

## Docker Setup (MVC Application)

The MVC application is containerized using Docker. Ensure Docker Desktop is running on your machine.

1. Open a terminal in the `RaceDayMVC` folder.
2. Build the Docker image:
   ```bash
   docker build -t racedaymvc .
