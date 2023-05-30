---
sidebar_position: 1
---

# Installation

SQListe est une application ASP.NET Core se connectant à votre base de données, 
et exposant ses procédures stockées à travers une API web de type [REST](https://fr.wikipedia.org/wiki/Representational_state_transfer).

## Configuration

La configuration se fait depuis le fichier _appsettings.json_, situé dans le même répertoire que l'exécutable.
Celui-ci prend la forme suivante.

```json lines
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
    }
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
    "IdleTimeout": 1440 // Session valable durant 24h
  },
  "Database": {
    "ConnectionString": "Data Source=MonServeurSQL; Database=MaBDD; User ID=MonUtilisateur; Password=MotDePasseSuperFort!; App=SQListe; TrustServerCertificate=true;",
    "Migration": {
      "Enable": true
    }
  }
}
```

Nous y retrouvons 3 sections :

### Journalisation
SQListe repose sur [Serilog](https://serilog.net/).<br/>
Cette partie du fichier de configuration permet de définir notamment le niveau de logs souhaité, 
ainsi que l'emplacement des fichiers de logs si souhaité.

Pour en savoir plus sur la configuration de Serilog, vous pouvez consulter la [doc associée](https://github.com/serilog/serilog-settings-configuration).

### Planificateur de tâches
Cette section correspond aux paramètres du planificateur de tâche (SQListe utilise [Coravel](https://docs.coravel.net/)).<br/>
Le paramètre _ConsummationDelay_ définit le délai de traitement lorsqu'une nouvelle tâche a été ajouté à la file d'attente. 

### Session
Donne un accès à certains paramètres de la session, notamment sa durée de vie (exprimée en minutes, 20 par défaut).

:::caution

La session se matérialise par un emplacement mémoire réservé à un utilisateur sur le serveur, et donc non disponible pour d'autres utilisations.<br/>
Si vous avez besoin de cette fonctionnalité, il est généralement recommandé de ne pas mettre une durée de vie trop importante, notamment en cas de fort trafic.<br/>
En effet, plus il y aura d'utilisateurs, plus il y aura de mémoire consommée.<br/>
Faites aussi attention à la quantité d'informations stockées par vos procédures.

:::

### Base de données
Cette section peut varier selon le SGBD que vous utilisez (cf. la section correspondante).<br/>
Cependant, nous retrouverons toujours le type de SGBD, la chaîne de connexion à celui-ci, ainsi que les paramètres de migration.
