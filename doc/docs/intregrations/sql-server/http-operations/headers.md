---
sidebar_position: 2
---

# En-tête HTTP

Les en-têtes HTTP permettent au client, comme au serveur, de joindre des informations supplémentaires à la requête ou à la réponse.<br/>
La plupart de ces informations sont des paramètres, pouvant modifier le comportement du navigateur lorsque joints à une réponse HTTP. 

:::note

L'en-tête HTTP d'une requête sera relativement différente de celle d'une réponse, les paramètres exprimés ayant souvent un sens différent selon le cas.<br/>
Certains d'entre eux n'ont de sens que dans une requête ou que dans une réponse.

:::

Pour en savoir plus sur les valeurs standards, et leur utilisation, vous pouvez consulter cette [ressource](https://developer.mozilla.org/fr/docs/Web/HTTP/Headers).

:::info

Il est tout à fait possible de définir vos paramètres personnalisés, cependant gardez à l'esprit que l'en-tête HTTP est limitée en taille (maximum 8192 octets, tous paramètres confondus).

:::

## Lecture

La lecture d'une donnée dans l'en-tête est similaire à la lecture d'un cookie : nous disposons aussi d'un dictionnaire clé/valeur où la clé correspond au nom de l'information, et la valeur à la donnée.

Exemple : lecture d'un jeton d'authentification dans l'en-tête (paramètre _Authorization_)

```sql
#Route("/api/exemple/headers/read")
#HttpGet("ExempleLectureHeader")
CREATE OR ALTER PROCEDURE [web].[p_exemple_cookie]
    @request_headers NVARCHAR(MAX) 
AS 
BEGIN
    DECLARE @auth_token NVARCHAR(MAX);
    DECLARE @username NVARCHAR(500);
    
    -- Récupération du de la valeur contenu dans le paramètre "Authorization"
    SET @auth_token = JSON_VALUE(@request_headers, '$.Authorization');

    -- Vérification du jeton et récupération de l'utilisateur...

    SELECT 
         '{ "message": "Bienvenu ' + @username  + ' !" }' AS [response_body]
        ,200 AS [response_status]
    ;
END
GO
```

## Écriture

À la différence des cookies, l'écriture d'une information dans l'en-tête se fait aussi via un dictionnaire clé/valeur répondant à la même logique que lors de la lecture.

Exemple : on définit le paramètre _Max-Age_ afin que notre ressource soit mise en cache dans le navigateur, et re-demandée uniquement lorsqu'elle sera expirée.

```sql
#Route("/api/exemple/headers/write")
#HttpGet("ExempleLectureHeader")
CREATE OR ALTER PROCEDURE [web].[p_exemple_cookie]
    @request_headers NVARCHAR(MAX) 
AS 
BEGIN
    DECLARE @push_notification_token NVARCHAR(MAX);
    
    -- Récupération du jeton permettant de recevoir des notifications push... 
    
    -- On définit l'en-tête "Max-Age"
    SET @request_headers = JSON_MODIFY(@request_headers, '$."Max-Age"', 21600); -- 60 * 24 * 15 = 21600 secondes = 15 jours
    
    SELECT 
         '{ "token": "' + @push_notification_token + '" }' AS [response_body]
        ,200 AS [response_status]
        ,@request_headers AS [request_headers]
    ;
END
GO
```
