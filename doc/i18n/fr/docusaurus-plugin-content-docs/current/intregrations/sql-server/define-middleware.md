---
sidebar_position: 4
---

# Définir un intergiciel

Un intergiciel prend la forme d'une procédure stockée située dans le schéma _web_ et ayant une annotation ```#Middleware```.

Cette annotation comporte plusieurs paramètres :
- After : défini s'il s'agit d'un post-intergiciel ou un pré-intergiciel. Par défaut, il s'agit d'un pré-intergiciel.
- Order : ordre d'exécution de l'intergiciel lorsqu'il y en a plusieurs 
- PathStarts : Permet de cibler un groupe de route. Par exemple, un intergiciel avec un ```PathStart``` valant ```/api/books``` ne sera exécuté que si contrôleur correspondant à la requête déclare une route commençant par ```/api/books```.

Tout comme dans les contrôleurs, il est possible d'altérer l'état de la réponse, d'interrompre le traitement.
La différence étant que vous n'avez pas obligations sur la réponse, comme dans l'exemple ci-dessous.

Exemple : un intergiciel de journalisation

```sql
    -- #Middleware(Order = 1, After = false)
    CREATE OR ALTER PROCEDURE [web].[p_middleware_log_request_start]
        @request_path NVARCHAR(MAX)
    AS
    BEGIN 
        INSERT INTO app_logs ([level], [message])
        SELECT 
             'info' AS [level]
            ,'Starting request at route ' + @request_path AS [message]
        ;
    END
    GO
```

Dans certains cas, il peut être nécessaire d'interrompre l'exécution du _pipeline_ de procédure et de retourner la réponse en l'état 
(par exemple si un utilisateur anonyme tente d'accéder à une ressource protégée).<br/>
Pour se faire, il suffit de retourner le paramètre standard ```next``` avec la valeur 0 comme dans l'exemple suivant.

Exemple : contrôle (très simplifié) de l'authentification de l'utilisateur

```sql
    -- #Middleware(Order = 2, After = false)
    CREATE OR ALTER PROCEDURE [web].[p_middleware_log_request_start]
        @request_headers NVARCHAR(MAX)
    AS
    BEGIN 
        DECLARE @auth_token NVARCHAR(MAX);

        IF (JSON_VALUE(@request_headers, '$.Authorization') IS NULL)
        BEGIN
            SELECT 
                 401 AS [response_status]
                ,0 AS [next]
            ;
        END
    END
    GO
```

Dans l'exemple ci-dessus, si aucun jeton d'authentification n'a été fourni par le client, on renvoie un statut 401, et on interrompt le pipeline de procédure.<br/>
_Dans une vraie application, il conviendra de vérifier la validité du jeton._

