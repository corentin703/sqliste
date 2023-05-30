---
sidebar_position: 4
---

# Sécurité

SQListe est conçu afin de vous donner toutes les clés nécessaires à la conception d'une application sécurisée.<br/>
Cependant, certaines pratiques doivent-être prises en considérations durant le développement.

## Injection SQL

Lors de l'introspection de la base de données, SQListe prend connaissance de la liste des paramètres de chaque procédure, 
ce qui lui permet de réaliser des requêtes paramétrées (et donc d'éviter les injections de code).<br/>
Cependant, il ne peut pas déterminer si le contenu d'un paramètre fourni par l'utilisateur est dangereux ou non.<br/>
Pour se prémunir de tout problème, n'exécutez **jamais** les éléments provenant de l'utilisateur.

Un exemple risqué ci-dessous (en T-SQL) où _request_body_ vaut ```DROP DATABASE DB_NAME()``` :
```sql
-- #Route("/api/security/example1")
-- #HttpPost("securityExemple1")
CREATE OR ALTER PROCEDURE [web].[p_security_exemple_1]
    @request_body NVARCHAR(MAX) -- Argument injecté via une requếte paramétrée = RAS 
AS 
BEGIN
    -- ...

    EXEC(@request_body); -- Code très risqué car cela va entraîner l'éxecution d'une chaîne litérale non échapée

    -- ...
END
GO
```

Dans notre exemple, bien que caricatural, cela va entraîner la suppression de la base de données.<br/>
L'une des sources majeures de faille est la création dynamique de requêtes prenant en paramètres des données de l'utilisateur. 
Si vous avez besoin de pratiquer cela, veillez à utiliser le mécanisme proposé par votre SGBD pour échapper ces derniers au sein de votre procédure stockée.

Second exemple illustrant ce cas :
```sql
-- #Route("/api/security/example2")
-- #HttpPost("securityExemple2")
CREATE OR ALTER PROCEDURE [web].[p_security_exemple_2]
    @request_body NVARCHAR(MAX) -- Argument injecté via une requếte paramétrée = RAS 
AS 
BEGIN
    DECLARE @data NVARCHAR(MAX);
    DECLARE @table NVARCHAR(100);
    DECLARE @col NVARCHAR(100);
    
    DECLARE @query NVARCHAR(MAX);
    
    -- Récupération de valeurs brutes
    SET @data = JSON_VALUE(@request_body);

    -- Récupération de la colonne et de la table correspondante depuis des données internes (sécurisées)
    
    -- Concaténation sans échappement de valeurs
    SET @query = 'INSERT INTO ' + @table + ' ('+ @col + ') VALUES (' + @data + ')';
    EXEC(@query);

    -- ...
END
GO
```

Dans ce cas, si une personne malintentionnée envoie une requête avec du code SQL dans le champ _data_ de l'objet JSON, celui-ci sera exécuté.

Voici une manière sûre d'exécuter l'exemple ci-dessus en SQL Server :
```sql
-- #Route("/api/security/example2Secure")
-- #HttpPost("securityExemple2Securre")
CREATE OR ALTER PROCEDURE [web].[p_security_exemple_2_secure]
    @request_body NVARCHAR(MAX) -- Argument injecté via une requếte paramétrée = RAS 
AS 
BEGIN
    DECLARE @data NVARCHAR(MAX);
    DECLARE @table NVARCHAR(100);
    DECLARE @col NVARCHAR(100);
    
    DECLARE @query NVARCHAR(MAX);
    
    -- Récupération de valeurs brutes
    SET @data = JSON_VALUE(@request_body, '$.data');

    -- Récupération de la colonne et de la table correspondante depuis des données internes (sécurisées)
    
    -- Paramétrage de la requête créé ci-dessus à partir de données sûres et exécution avec les données de l'utilateurs échapées
    SET @query = 'INSERT INTO ' + @table + ' ('+ @col + ') VALUES (@data_esc)';
    EXEC sp_executesql   
          @query,  
          N'@data_esc NVARCHAR(MAX)',  
          @data_esc = @data
    ;

    -- ...
END
GO
```

## Droits utilisateurs

Pour aller plus loin, il peut être souhaitable de créer un utilisateur, aux droits restreints, destiné à être dédié à SQListe.<br/>
N'utilisez donc pas l'utilisateur _root_ en production, et passez plutôt par un utilisateur aux droits intermédiaires.
