---
sidebar_position: 3
---

# Définir un contrôleur

Un contrôleur prend la forme d'une procédure stockée située dans le schéma _web_.

Voici un exemple de contrôleur basique :
```sql
-- #Route("/api/helloWorld")
-- #HttpGet("HelloWorld")
CREATE OR ALTER PROCEDURE [web].[p_hello_world] 
AS 
BEGIN
    SELECT 
         'Salutation tout le beau monde !' AS [response_body] -- On retourne le corp de la réponse
        ,'text/plain' AS [response_content_type] -- Le type MIME du corp
        ,200 AS [response_status] -- Le statut HTTP de la réponse
    ;
END
GO
```

## Paramètres d'URI

Un paramètre d'URI est un argument fourni comme suit dans un URI : ```/api/exemple?name=Corentin```<br/>
Pour en récupérer la valeur, il suffit de le prendre en argument de procédure :
```sql
-- On défini le paramètre 'name' dans la route entre accolades
-- #Route("/api/helloWorld")
-- #HttpGet("HelloWorld")
CREATE OR ALTER PROCEDURE [web].[p_hello_world] 
    @name NVARCHAR(100) = NULL -- On récupère le paramètre d'URI 'name'
AS 
BEGIN
    DECLARE @response_body NVARCHAR(MAX);
    
    IF (@name IS NULL)
        SET @response_body = 'Salutation tout le beau monde !';
    ELSE    
        SELECT @response_body = 'Salutation ' + @name + ' !';

    SELECT 
         @response_body AS [response_body]
        ,'text/plain' AS [response_content_type]
        ,200 AS [response_status]
    ;
END
GO
```

Un paramètre d'URI est toujours considéré comme étant facultatif, et doit être de type NVARCHAR (charge à la procédure de le caster si un autre type est attendu).

:::caution

Il est recommandé de définir une valeur par défaut pour les paramètres d'URI, ainsi que pour les paramètres de route facultatifs dans les arguments de procédure :
s'il n'y en a pas et que le paramètre n'est pas fourni, le SGBD ne pourra pas exécuter la procédure et remontera une erreur.

:::

## Paramètres de route

Comme la plupart des _frameworks_ web, SQListe permet de définir des modèles de route.
Cela permet de passer des arguments directement dans la route plutôt que dans les paramètres d'URI, ou encore dans le corp de la requête.

Exemple :

```sql
-- On défini le paramètre 'name' dans la route entre accolades
-- #Route("/api/helloWorld/{name}")
-- #HttpGet("HelloWorld")
CREATE OR ALTER PROCEDURE [web].[p_hello_world] 
    @name NVARCHAR(100) -- On récupère le paramètre avec le même nom en argument de procédure
AS 
BEGIN
    DECLARE @response_body NVARCHAR(MAX);
    
    SELECT @response_body = 'Salutation ' + @name + ' !';

    SELECT 
         @response_body AS [response_body]
        ,'text/plain' AS [response_content_type]
        ,200 AS [response_status]
    ;
END
GO
```

:::note

Il est tout à fait possible de définir plusieurs paramètres dans la route, avec un nom différent.

Comme pour les paramètres d'URI, les paramètres de route seront injectés avec le type NVARCHAR.

:::

:::caution

Si un nom de paramètre est identique à un nom de paramètre standard, ce dernier prendra le dessus. 

:::

Dans notre exemple ci-dessus, le paramètre est considéré comme requis : s'il n'est pas fourni, SQListe ne fera pas la correspondance avec cette route.<br/>
Pour définir un paramètre optionnel, nous pouvons faire comme suit :
```sql
-- On défini le paramètre 'name' dans la route entre accolades
-- #Route("/api/helloWorld/{name?}")
-- #HttpGet("HelloWorld")
CREATE OR ALTER PROCEDURE [web].[p_hello_world] 
    @name NVARCHAR(100) = NULL -- On récupère le paramètre avec le même nom en argument de procédure, en mettant une valeur NULL par défaut (utile si le paramètre n'est pas fourni).
AS 
BEGIN
    DECLARE @response_body NVARCHAR(MAX);
    
    IF (@name IS NULL)
        SET @response_body = 'Salutation tout le monde !';
    ELSE    
        SELECT @response_body = 'Salutation ' + @name + ' !';

    SELECT 
         @response_body AS [response_body]
        ,'text/plain' AS [response_content_type]
        ,200 AS [response_status]
    ;
END
GO
```

Un paramètre facultatif **doit** figurer en fin de route.
Exemple : 
- ```/api/say/hello/{name?}/{age?}``` => Valide.
- ```/api/say/hello/{name}/withAge/{age?}``` => Valide.
- ```/api/say/hello/{name?}/withAge/{age?}``` => Invalide : le bon fonctionnement ne sera pas garanti.

Lorsque l'on a plusieurs paramètres facultatifs, il vaut souvent mieux utiliser des paramètres d'URI, comme décrit précédemment.

:::warning Limitations

La taille d'un URI étant limitée à maximum 2048 caractères, des problèmes de lecture peuvent survenir en cas de paramètres trop longs.

_Certains serveurs permettent d'augmenter cette limite, cependant il convient de vérifier que le client HTTP / navigateur prenne cette augmentation en charge._

:::
