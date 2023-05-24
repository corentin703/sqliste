---
sidebar_position: 1
---

# Installation

SQListe is an ASP.NET Core application that connects to your database and exposes its stored procedures through a [REST](https://en.wikipedia.org/wiki/Representational_state_transfer) web API.

## Configuration

The configuration is done through the _appsettings.json_ file, located in the same directory as the executable. It has the following structure:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "Enrich": [
      "FromLogContext",
      "WithMachineName"
    ],
    "Properties": {
      "ApplicationName": "SQListe"
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": { "path":  "./logs/log-.txt", "rollingInterval": "Day" }
      }
    ]
  },
  "AllowedHosts": "*",
  "AllowedOrigins": ["*"],
  "Coravel": {
    "Queue": {
      "ConsummationDelay": 1
    }
  },
  "Session": {
    "IdleTimeout": 1440
  },
  "Database": {
    "ConnectionString": "Data Source=MySQLServer; Database=MyDB; User ID=MyUser; Password=MyStrongPassword!; App=SQListe; TrustServerCertificate=true;",
    "Migration": {
      "Enable": true
    }
  }
}

There are 3 sections in the configuration file:

### Logging
SQListe uses [Serilog](https://serilog.net/) for logging.<br/>
This section of the configuration file allows you to define the desired log level and the location of log files if desired.

For more information on configuring Serilog, you can refer to the [associated documentation](https://github.com/serilog/serilog-settings-configuration).

### Task Scheduler
This section corresponds to the parameters of the task scheduler (SQListe uses [Coravel](https://docs.coravel.net/)).<br/>
The `ConsummationDelay` parameter defines the processing delay when a new task has been added to the queue.

### Session
Provides access to certain session parameters, including its idle timeout (expressed in minutes, default is 20).

### Database
This section may vary depending on the DBMS you are using (refer to the corresponding section).<br/>
However, it will always include the DBMS type, the connection string to it, and migration parameters.
