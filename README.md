# RaceDay Platform - Full Stack

## Overview
This web application will allow you to manage sporting events, such as marathons, sprints, runs, walks, cycles etc. This is split into two sections: a RESTful API backend and ASP.NET Core MVC frontend. The application allows users to register to be an organiser/participant. Users can create/manage events, sign up to categories, and view results.

## Roles:

### Organiser
Create, edit and delete events.
Add categories to events (e.g. 5km, 10km, Half Marathon).
View all participants who have signed up to their event(s).
Record and publish completion times and positions for participants.

### Participant
View all events and categories.
Sign up to an event and choose a category.
View a dashboard with their sign-ups.
View their race history.

## Setup & Run
### Pre-requisites:

.NET 10.0 SDK
SQLite (via Entity Framework Core)

### Local Setup
Clone this repository.

Open the solution file RaceDayMVC.slnx in Visual Studio.
Ensure that both RaceDayAPI and RaceDayMVC are set as Startup Projects (Right click Solution > Properties > Multiple Startup Projects).
The API will run on https://localhost:7270 and the MVC application will run on https://localhost:7003.
Press F5 to run both applications. The database will automatically populate with default roles on first run.
## Docker Setup (MVC Application)
The MVC application has been set up with Docker. Ensure Docker Desktop is running.
Open a terminal inside RaceDayMVC directory.
Build the Docker image:
```bash
docker build -t racedaymvc .
