# LOTO NET Monitor - Database Schema Design

## Overview
This document outlines the complete database schema for the Network Monitoring System.

## Database Tables

### 1. Categories Table
```sql
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedDate DATETIME2 DEFAULT GETUTCDATE(),
    IsActive BIT DEFAULT 1
);
```

### 2. Devices Table
```sql
CREATE TABLE Devices (
    DeviceID INT PRIMARY KEY IDENTITY(1,1),
    CategoryID INT NOT NULL FOREIGN KEY REFERENCES Categories(CategoryID),
    DeviceName NVARCHAR(200) NOT NULL,
    IPAddress VARCHAR(45) NOT NULL UNIQUE,
    Location NVARCHAR(300),
    Description NVARCHAR(500),
    EmailAlertEnabled BIT DEFAULT 1,
    SMSAlertEnabled BIT DEFAULT 1,
    PingIntervalSeconds INT DEFAULT 30,
    TimeoutThreshold INT DEFAULT 3,
    CurrentStatus NVARCHAR(20) DEFAULT 'Unknown', -- Online, Offline, Warning
    LastPing DATETIME2,
    ResponseTime INT, -- milliseconds
    FailureCount INT DEFAULT 0,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedDate DATETIME2 DEFAULT GETUTCDATE(),
    IsActive BIT DEFAULT 1
);

CREATE INDEX IX_Devices_CategoryID ON Devices(CategoryID);
CREATE INDEX IX_Devices_CurrentStatus ON Devices(CurrentStatus);
CREATE INDEX IX_Devices_IsActive ON Devices(IsActive);
```

### 3. PingLog Table
```sql
CREATE TABLE PingLogs (
    LogID BIGINT PRIMARY KEY IDENTITY(1,1),
    DeviceID INT NOT NULL FOREIGN KEY REFERENCES Devices(DeviceID),
    PingTime DATETIME2 DEFAULT GETUTCDATE(),
    ResponseTime INT, -- milliseconds, NULL if timeout
    Status NVARCHAR(20) NOT NULL, -- Online, Offline, Timeout
    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);

CREATE INDEX IX_PingLogs_DeviceID ON PingLogs(DeviceID);
CREATE INDEX IX_PingLogs_PingTime ON PingLogs(PingTime);
```

### 4. AlertHistory Table
```sql
CREATE TABLE AlertHistory (
    AlertID INT PRIMARY KEY IDENTITY(1,1),
    DeviceID INT NOT NULL FOREIGN KEY REFERENCES Devices(DeviceID),
    AlertType NVARCHAR(20) NOT NULL, -- StatusChange, Recovery, Critical
    PreviousStatus NVARCHAR(20),
    CurrentStatus NVARCHAR(20) NOT NULL,
    Message NVARCHAR(MAX),
    EmailSent BIT DEFAULT 0,
    SMSSent BIT DEFAULT 0,
    Acknowledged BIT DEFAULT 0,
    Notes NVARCHAR(500),
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    AcknowledgedDate DATETIME2,
    AcknowledgedBy NVARCHAR(MAX)
);

CREATE INDEX IX_AlertHistory_DeviceID ON AlertHistory(DeviceID);
CREATE INDEX IX_AlertHistory_CreatedDate ON AlertHistory(CreatedDate);
CREATE INDEX IX_AlertHistory_CurrentStatus ON AlertHistory(CurrentStatus);
```

### 5. Settings Table
```sql
CREATE TABLE Settings (
    SettingID INT PRIMARY KEY IDENTITY(1,1),
    SettingKey NVARCHAR(100) NOT NULL UNIQUE,
    SettingValue NVARCHAR(MAX),
    Description NVARCHAR(500),
    SettingType NVARCHAR(50), -- String, Int, Bool
    UpdatedDate DATETIME2 DEFAULT GETUTCDATE()
);
```

### 6. AspNetUsers (ASP.NET Identity - Extended)
```sql
-- Standard ASP.NET Identity tables are created automatically
-- Extended fields can be added via migrations
```

## Category Examples
- Data Center
- Servers
- Routers
- Switches
- Internet
- Firewalls
- Branches
- Lottery Terminals
- POS

## Device Status Values
- **Online**: Device is responding to pings
- **Offline**: Device has exceeded failure threshold
- **Warning**: Device is responding but with high latency
- **Unknown**: Initial state or no ping data yet

## Alert Types
- **StatusChange**: Device status changed
- **Recovery**: Device came back online
- **Critical**: Repeated failures

## Settings Examples
- `SMTPServer`: SMTP server address
- `SMTPPort`: SMTP port number
- `SMTPUsername`: SMTP username
- `SMTPPassword`: SMTP password (encrypted)
- `TwilioAccountSID`: Twilio account SID
- `TwilioAuthToken`: Twilio auth token
- `CompanyName`: Company name for emails
- `DefaultRefreshInterval`: Dashboard refresh interval (seconds)
- `DefaultTimeoutCount`: Default failure threshold
- `DefaultEmail`: Default alert recipient email
- `DefaultMobileNumber`: Default alert recipient mobile
