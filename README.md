# LOTO NET Monitor

A production-ready Network Monitoring System built with ASP.NET Core 8, C#, and SQL Server.

## Technology Stack

- **Backend**: ASP.NET Core 8 MVC
- **Database**: SQL Server 2022 / Express
- **ORM**: Entity Framework Core
- **Frontend**: Bootstrap 5, Chart.js
- **Hosting**: IIS
- **Background Service**: Windows Service
- **Architecture**: Clean Architecture with Repository Pattern

## Project Structure

```
LOTONetMonitor/
├── LOTONetMonitor.Domain/              # Business entities and interfaces
├── LOTONetMonitor.Application/         # Business logic and use cases
├── LOTONetMonitor.Infrastructure/      # External services (Email, SMS)
├── LOTONetMonitor.Persistence/         # Database context and migrations
├── LOTONetMonitor.Services/            # Background monitoring service
└── LOTONetMonitor.Web/                 # ASP.NET Core MVC Web App
```

## Features

### 1. **Authentication & Authorization**
- ASP.NET Identity integration
- Role-based access (Admin, Operator)
- CSRF protection

### 2. **Dashboard**
- Real-time device status display
- Pie chart visualization
- Status summary cards
- Latest alerts view
- 30-second auto-refresh

### 3. **Device Management**
- CRUD operations for devices
- Assign devices to categories
- Configure ping intervals and thresholds
- Enable/disable alerts (Email/SMS)

### 4. **Category Management**
- Manage device categories
- Predefined categories (Data Center, Servers, Routers, etc.)

### 5. **Background Monitoring Service**
- Windows Service for continuous monitoring
- Pings devices every 30 seconds
- Stores response times in database
- Detects device status changes

### 6. **Alerting System**
- Email alerts via SMTP
- SMS alerts via Twilio
- Status change detection (Online ↔ Offline ↔ Warning)
- Alert history tracking

### 7. **Reporting**
- Daily/Monthly availability reports
- Downtime analysis
- Most unstable devices report
- Export to PDF/Excel

### 8. **Audit & Logging**
- Serilog integration
- Error and exception logging
- Alert history
- Ping logs

## Getting Started

[Setup instructions will be added after database creation]

## Development Status

Currently under development. Following step-by-step implementation plan.
