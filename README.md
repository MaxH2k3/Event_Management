
# Event Management System

The Event Management System is a web application developed using ASP.NET for the backend. This system provides comprehensive tools for managing various events, including event creation, attendee registration, and event scheduling. The platform is designed to streamline the event management process, making it efficient and user-friendly for both organizers and participants.


## Demo

- [API Swagger](https://fpt-event-management.azurewebsites.net/swagger/index.html)


## Features

- Event Creation and Management: Create, update, and delete events with details like date, time, location, and description.
- Attendee Registration: Allow users to register for events, manage attendee lists, and send confirmation emails.
- Event Scheduling: View and manage event schedules using a calendar interface.
- User Authentication: Secure login and registration for event organizers and attendees using JWT (JSON Web Tokens).
- Admin Panel: Administrative dashboard for managing events, users, and system settings.
- Notifications: Automated email notifications for event reminders and updates using Gmail.
- Caching: Improve performance with Redis for caching frequently accessed data.
- Checking Realtime

## Architecture

````
EventManagementSystem/
├── API/
│   ├── Controllers/
│   ├── Models/
│   └── ...
├── Application/
│   ├── Interfaces/
│   ├── Services/
│   ├── UseCases/
│   └── ...
├── Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Services/
│   └── ...
├── Infrastructure/
│   ├── Data/
│   ├── Email/
│   ├── Caching/
│   ├── Configuaration/
│   └── .../
└── ...
````
## Installation & Setup

#### 1. Clone the Repository

- git clone '...'

- cd event-management-system

#### 2. Setup the Database
- Ensure SQL Server is installed and running on your machine.
- Create a new database named EventManagement.
- Update the connection string in appsettings.json file located in the root directory of the project.

```html
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=EventManagementDB;Trusted_Connection=True;"
}
```

#### 3. Configure Redis

- Ensure Redis is installed and running on your machine or server.
- Update the Redis configuration in appsettings.json.

```html
"Redis": {
    "ConnectionString": "your_redis_server:6379",
    "Password; "your_redis_password_if_using_cloud"
}

```

#### 4. Configure Gamil

- Set up a Gmail account for sending notifications.
- Enable "Less secure app access" in your Gmail account settings.
- Update the Gmail configuration in appsettings.json.


```html
"GmailSetting": {
  "DisplayName": "your_email@gmail.com",
  "SmtpServer": "smtp.gmail.com",
  "Port": 587,
  "Mail": "your_email@gmail.com",
  "Password": "your_password"
},

```

#### 5. Configure JWT

```html
"Jwt": {
    "Securitykey": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience",
    "TokenExpiry": "your_time"
}
```

#### 6. Run the Application

- dotnet run

## Tech Stack

**Server:** asp.net

**Framework:** EF6

**Library:** FluentMail, FluentValidation, JWT Bear, Cache

**Database:** SQL Server, Redis

## Authors

- [Le Huy](https://github.com/MaxH2k3)
- [Gia Khang](https://github.com/giakhang3005)
- [Cong Lam](https://github.com/CongLam1806)
- [Duc Minh](https://github.com/Minhduc027)
- [Truong Thanh](https://github.com/truongthanhvu2337)
