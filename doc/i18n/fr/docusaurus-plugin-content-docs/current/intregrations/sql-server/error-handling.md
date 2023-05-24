---
sidebar_position: 7
---

# Gestion d'erreurs

Lorsqu'une erreur se produit dans une procédure au cours du _pipeline_, cela l'interrompt et retourne une erreur 500 (_INTERNAL SERVER ERROR_).

Afin de vous permettre d'implémenter une gestion d'erreurs, il est possible de prendre un paramètre standard _error_ dans les procédures du _pipeline_.<br/>
Ceci fait, lorsque qu'une erreur va survenir, SQListe va consulter la liste des arguments dans les procédures du _pipeline_ n'ayant pas été exécutés, et si existant, prendra la première d'entre-elles et exécutera.<br/>

Si cette procédure ne remonte pas d'erreur à son tour, celle-ci sera considérée comme étant gérée.

L'argument _error_ est un JSON répondant au format suivant :
```json lines
{
  "message": "Message de l'erreur", // Cela peut contenir une chaine tout à fait arbitraire *
  "attributes": {
    "state": 1 // État de l'erreur remonté par SQL Server
  }
}
```

Ce mécanisme va vous permettre d'intercepter une erreur, et de modifier la réponse en conséquence.

:::info

Lorsque plusieurs procédures du _pipeline_ remontent des erreurs, le mécanisme s'exécute à chaque fois.

:::

:::note À propos du champ _message_ de l'argument _error_ *

Il y a possibilité de faire passer une chaine structurée (par exemple un JSON) en tant que message dans un _RAISERROR_. 
Vous le retrouverez ici, et pourrez réaliser un traitement sur la réponse avec. 

:::

Cas pratique : un post-intergiciel attrapant toutes les erreurs :

```sql
-- #Middleware(Order = 1000, After = true)
CREATE OR ALTER PROCEDURE [web].[p_middleware_catch_error]
    @request_body NVARCHAR(MAX) = NULL,
    @request_headers NVARCHAR(MAX) = NULL,
    @request_cookies NVARCHAR(MAX) = NULL,
    @request_path NVARCHAR(MAX) = NULL,
    @pipeline_storage NVARCHAR(MAX),
    @error NVARCHAR(MAX) = NULL
AS
BEGIN
    IF (@error IS NULL)
    BEGIN
        RETURN;
    END

    -- On récupère la propriété "message" du JSON d'erreur
    SET @error_message = JSON_VALUE(@error, '$.message');

    -- Si ce n'est pas un JSON, on ne le gère pas (SQListe retournera un HTTP 500)
    IF (ISJSON(@error_message) = 0)
    BEGIN
        RAISERROR(@error_message, 18, 1);
        RETURN;
    END
    
    DECLARE @error_message NVARCHAR(MAX);
    DECLARE @response_body NVARCHAR(MAX);
    DECLARE @response_headers NVARCHAR(MAX) = N'{ "Content-Type": "application/json" }';

    -- On déchiffre le JSON et on réalise notre traitement
    DECLARE
        @message NVARCHAR(MAX),
        @status INT
    ;

    SELECT
        @message = [error_message],
        @status = [http_status]
    FROM OPENJSON(@error_message)
    WITH (
        [error_message] NVARCHAR(MAX),
        [http_status] INT
    );

    SET @response_body = (
        SELECT
            @message AS [message],
            @status AS [status]
        FOR JSON PATH
    );

    SELECT
         JSON_QUERY(@response_body, '$[0]') AS [response_body]
        ,@status AS [response_status]
        ,@response_headers AS [response_headers]
        ,0 AS [next]
    ;
END
GO
```

Dans cet exemple, le message passé au _RAISERROR_ est un JSON, sur lequel nous réalisons un traitement afin d'altérer la réponse de manière adaptée.
