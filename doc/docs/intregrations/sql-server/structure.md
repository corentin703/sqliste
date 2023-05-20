---
sidebar_position: 2
---

# Structure

## Schémas

La base de donnée connectée se retrouve organisée sur 3 schémas après exécution de la migration.

### Schéma _sqliste_

Ce schéma contient toutes les procédures nécessaires au bon fonctionnement de SQListe.

On y retrouve les procédures permettant l'introspection de la base de données, ainsi que la génération et l'obtention du JSON OpenAPI.

:::info

Le bon fonctionnement de l'applicatif ne saurait-être garanti en cas de modification d'une de ces procédures sans consultation préalable de la documentation.

:::

### Schéma _web_

Ce schéma est destiné à accueillir les procédures définissant des contrôleurs et des middlewares.   

:::caution

Tout objet présent dans ce schéma peut potentiellement être exposé via l'API web : n'y mettez pas vos objets métiers et autres procédures sensibles.

:::

### Schéma _dbo_ / schéma par défaut

Le schéma par défaut n'est pas utilisé par SQListe : vous pouvez donc y développer sereinement en ayant la 
garantie que rien ne pourra être appelé depuis le web sans passer par une procédure du schéma _web_. 
