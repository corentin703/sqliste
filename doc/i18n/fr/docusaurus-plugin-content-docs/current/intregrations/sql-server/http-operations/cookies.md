---
sidebar_position: 1
---

# Cookies

Un cookie est une donnée stockée dans le navigateur, généralement limitée à 4ko, et qui sera jointe à chaque requête.<br/>
Ils permettent par exemple de mémoriser des préférences utilisateurs, ou encore de faire le lien entre un utilisateur et sa session HTTP.

## Lecture

Les cookies peuvent-être lu dans un dictionnaire clé/valeur disponible dans le paramètre standard _request_cookies_.<br/>
La clé correspondra au nom de votre cookie, et la valeur à la donnée contenue dedans.

Exemple de lecture : un cookie comptant le nombre de requêtes de l'utilisateur.

```sql
#Route("/api/exemple/cookie")
#HttpGet("ExempleCookie")
CREATE OR ALTER PROCEDURE [web].[p_exemple_cookie]
    @request_cookies NVARCHAR(MAX) 
AS 
BEGIN
    DECLARE @visit_counter BIGINT;
    
    -- On récupère la valeur du cookie nommé "visitCounter"
    SET @visit_counter = ISNULL(JSON_VALUE(@request_cookies, '$.visitCounter'), 0) + 1;

    IF (@visit_counter = 1)
    BEGIN
        SELECT 
             '{ "message": "C''est votre première visite. Soyez le bienvenu !" }' AS [response_body]
            ,200 AS [response_status]
        ;
        RETURN;
    END 

    SELECT 
         '{ "message": "C''est votre visite n°' + @visit_counter + ' !" }' AS [response_body]
        ,200 AS [response_status]
    ;
END
GO
```

## Écriture

Pour écrire, ou altérer un cookie, il faut altérer le paramètre _response_cookies_.<br/>
Contrairement à la lecture, il ne s'agit pas d'un dictionnaire, mais d'un tableau de paramètres, permettant de définir plusieurs propriétés aux cookies.

Description d'un paramétrage de cookie :
```json lines
{
  "name": "nom",
  "value": "valeur",
  "expires": "2023-06-12T00:00:00", // Date d'expiration au format [ISO 8601](https://fr.wikipedia.org/wiki/ISO_8601)
  "domain": "Domaine de validité", // Par défaut le domaine correspondant à l'origine. Vous pouvez définir votre domaine racine pour inclure tous vos sous-domaines. Si vous mettez un autre domaine, gardez en tête que les cookies _cross-origin_ étant en passe de disparaitre.
  "path": "/api/exemple", // Le cookie ne sera envoyé que si la route commence par /api/exemple
  "secure": true, // Le cookie ne sera envoyé que lorsque la connexion sera en HTTPS
  "sameSite": "Lax", // Permet de définir la politique d'envoie du cookie lors de requêtes vers d'autres sites
  "httpOnly": false, // Rend le cookie inaccessible via du JavaScript
  "maxAge": 3600 // Durée de vie maximum du cookie (exprimé en secondes), complémentaire du paramètre _expires_, qui lui prend une date
}
```

Pour en savoir plus sur les cookies, ainsi que les possibilités de ces paramètres, vous pouvez consulter cette [ressource](https://developer.mozilla.org/fr/docs/Web/HTTP/Cookies) qui détaille l'utilité de chacun.<br/>
Les paramètres obligatoires dans ce modèle sont ```name``` et ```value``` ainsi que ```expires``` ou ```maxAge``` au choix.

Reprenons notre exemple précédent, en y ajoutant le code nécessaire pour définir le cookie s'il n'existe pas, ou le mettre à jour.

```sql
#Route("/api/exemple/cookie")
#HttpGet("ExempleCookie")
CREATE OR ALTER PROCEDURE [web].[p_exemple_cookie]
    @request_cookies NVARCHAR(MAX),
    @response_cookies NVARCHAR(MAX)
AS 
BEGIN
    DECLARE @visit_counter BIGINT;
    DECLARE @visit_counter_cookie NVARCHAR(MAX);
    
    -- On récupère la valeur actuelle
    SET @visit_counter = ISNULL(JSON_VALUE(@request_cookies, '$.visitCounter'), 0) + 1;

    -- On paramètre notre cookie
    SET @visit_counter_cookie = (
        SELECT 
            'visitCounter' AS [name],
            3600 * 24 * 7 AS [maxAge],
            @visit_counter AS [value]
        FOR JSON PATH
    );
    
    -- FOR JSON PATH retournant systèmatiquement un tableau, on récupère le premier élément (le seul existant ici)
    SET @visit_counter_cookie = JSON_QUERY(@visit_counter_cookie, '$[0]');

    -- On l'ajoute à la liste des cookies que l'on souhaite altérer
    SET @response_cookies = JSON_MODIFY(@response_cookies, 'append $', @visit_counter_cookie); 

    IF (@visit_counter = 1)
    BEGIN
        SELECT 
             '{ "message": "C''est votre première visite. Soyez le bienvenu !" }' AS [response_body]
            ,200 AS [response_status]
            ,@response_cookies AS [response_cookies]
        ;
        RETURN;
    END 

    SELECT 
         '{ "message": "C''est votre visite n°' + @visit_counter + ' !" }' AS [response_body]
        ,200 AS [response_status]
        ,@response_cookies AS [response_cookies]
    ;
END
GO
```
