---
sidebar_position: 1
---

# Prérequis

## Version

SQListe requiert SQL Server 2017 minimum.<br/>
Azure SQL edge est supporté.<br/>
Le connecteur SQL serveur est testé avec SQL Server 2017 Express.

## Base de données

Vous devez activer le _service broker_ sur la base de données destinée à être utilisée avec SQListe pour pouvoir 
utiliser les fonctionnalités événementielles de l'applicatif.

## Support du JSON

Beaucoup de paramètres utilisent le format JSON au sein de SQListe.<br/>
SQL Server dispose d'un jeu de fonctions relativement complet pour lire et écrire des données au format JSON.<br/>
[Pour en savoir plus](https://learn.microsoft.com/fr-fr/sql/relational-databases/json/json-data-sql-server?view=sql-server-ver16).