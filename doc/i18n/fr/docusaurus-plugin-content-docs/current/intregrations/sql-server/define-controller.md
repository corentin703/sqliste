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

## Consommer le corp d'une requête

Vous pouvez récupérer les données envoyées comme ceci grâce au paramètre standard _request_body_.<br/>
Le format de cet élément est libre et doit correspondre au _Content-Type_ renseigné dans l'en-tête de la requête.

Prenons l'exemple d'une requête sur une route permettant l'authentification d'un utilisateur, ou nous avons besoin de
récupérer le courriel de l'utilisateur, ainsi que son mot de passe :
```sql
-- #Route("/api/account/login")
-- #HttpPost("AccountLogin")
CREATE OR ALTER PROCEDURE [web].[p_account_login] 
    @request_body NVARCHAR(MAX), -- Les informations saisies par l'utilisateur seront stockées ici
    @request_content_type NVARCHAR(255)
AS 
BEGIN
    IF (@request_content_type <> 'application/json')
    BEGIN
        SELECT 
            ,@request_content_type AS [response_content_type]
            ,415 AS [response_status] -- 415 = Unsupported Media Type
        ;
        RETURN;   
    END
    
    DECLARE @email NVARCHAR(1000);
    DECLARE @password NVARCHAR(1000);
    
    SELECT 
         @email = [email]
        ,@password = [password]
    FOR OPENJSON(@request_body)
    WITH (
         [email] NVARCHAR(1000)
        ,[password] NVARCHAR(1000)
    );
    
    IF (@email = 'example@email.com' AND @password = 'SuperPa$$word!!!')
    BEGIN
        SELECT 
             '{ "message": "Vous êtes connecté !" }' AS [response_body]
            ,'application/json' AS [response_content_type]
            ,200 AS [response_status]
        ;
    END
    ELSE    
    BEGIN
        SELECT 
             '{ "message": "Une erreur est survenue durant la connexion" }' AS [response_body]
            ,'application/json' AS [response_content_type]
            ,401 AS [response_status]
        ;
    END
END
GO
```

:::caution

Un corp de requête ne peut pas être joint à une requête _GET_ ou _HEAD_.<br/>
[Pour en savoir plus](https://developer.mozilla.org/en-US/docs/Web/API/Request/body).

:::

## Consommer un formulaire

Certains _Content-Type_ servent à matérialiser un formulaire (notamment pour la balise `<form />` en html).
Dans ces cas-là, le _Content-Type_ vaudra soit `application/x-www-form-urlencoded` soit `multipart/form-data`.

Lorsqu'une requête de ce type est transmise à SQListe, celui-ci ne va pas injecter ses données dans le paramètre 
_request_body_, mais dans le paramètre _request_form_.

Le contenu de ce paramètre sera un JSON de ce format-là :
```json lines
[
  {
    "name": "nom du champ",
    "value": "valeur du champ" // Toujours de type string
  },
  {
    "name": "nom du 2nd champ",
    "value": "valeur du 2nd champ" // Toujours de type string
  },
  // ...
]
```

:::info 

Lorsque le paramètre _request_form_ n'est pas égal à _NULL_, le paramètre _request_body_ l'est.

:::

### Récupérer un fichier

Les requêtes de type _form_, permettent d'envoyer directement des fichiers sans encodage particulier (tel que le base64).<br/>
Lorsqu'un élément correspond à un fichier, SQListe va injecter un objet JSON au format ci-dessous dans le paramètre _request_form_ pour le champ concerné.
```json lines
[
  {
    "name": "nom d'un champ de type fichier",
    "headers": {
      "Content-Type": "image/png", // Type MIME correspondant au fichier
      // ...
    },
    "length": 128000, // Taille du fichier (en octets)
    "contentType": "image/png", // Raccourci pour accéder à l'en-tête "Content-Type" du fichier
    "contentDisposition": "form-data; name=\"fieldName\"; filename=\"filename.png\"", // Raccourci pour accéder à l'en-tête "Content-Disposition" du fichier
    "fileName": "filename.png" // Nom de fichier, extrait du "Content-Disposition"
  },
  {
    "name": "nom d'un champ classique",
    "value": "valeur du champ classique" // Toujours de type string
  },
  // ...
]
```

Pour accéder au contenu du fichier, il faut récupérer un paramètre de procédure dont le nom répond au format 
`request_form_file_<nomDuChamp>` et de type VARBINARY.

Exemple : nous allons récupérer un formulaire permettant d'enregistrer un album de musique, avec la photo de sa couverture.
```sql
-- #Route("/api/sampleForm")
-- #HttpPost("sampleForm")
CREATE OR ALTER PROCEDURE [web].[p_sample_form] 
    @request_form NVARCHAR(MAX), -- Les informations saisies par l'utilisateur seront stockées ici
    @request_form_file_cover VARBINARY(MAX) -- Nous récupérons la couverture au format binaire, tel qu'envoyé par l'utilisateur
AS 
BEGIN
    DECLARE @form_data TABLE (
         [name] NVARCHAR(500)
        ,[value] NVARCHAR(500)
        ,[file_name] NVARCHAR(500)
        ,[length] BIGINT
    );
    
    INSERT INTO @form_data
    SELECT 
         [name]
        ,[value]
        ,[fileName] AS [file_name]
        ,[length]
    FROM OPENJSON(@request_form)
    WITH (
         [name] NVARCHAR(500)
        ,[value] NVARCHAR(500)
        ,[fileName] NVARCHAR(500)
        ,[length] BIGINT
    );
    
    DECLARE @artist NVARCHAR(1000);
    DECLARE @title NVARCHAR(1000);
    DECLARE @cover_file_name NVARCHAR(1000);
    DECLARE @cover_length BIGINT;
    
    SELECT TOP 1 @artist = [value] FROM @form_data
    WHERE [name] = 'artist';
    
    SELECT TOP 1 @title = [value] FROM @form_data
    WHERE [name] = 'title';
    
    SELECT TOP 1 
         @cover_file_name = [file_name] 
        ,@cover_length = [length] / 1000 -- Obtention du résultat en ko 
    FROM @form_data
    WHERE [name] = 'cover';
    
    -- On vérifie le poids de l'image
    IF (@cover_length > 500)
    BEGIN
        SELECT
             '{ "message": "Le poids de l''image que vous avez fourni doit être inférieur à 500ko" }' AS [response_body]
            ,'application/json' AS [response_content_type] 
            ,413 AS [response_status] -- 413 = Payload Too Large
        ;
    END

    INSERT INTO [dbo].[albums] ([artist], [title], [cover], [cover_file_name])
    VALUES (
         @artist
        ,@title
        ,@request_form_file_cover
        ,@cover_file_name
    );
    
    SELECT 
        204 AS [response_status]
    ;
END
GO
```

:::caution

Pensez à bien vérifier le poids des fichiers que vous archivez.<br/>
Le transfert de gros fichiers **n'est pas** garanti.

:::

## Téléchargement de fichiers

Afin de transmettre des fichiers à l'utilisateur, SQListe met à votre disposition 3 paramètres de sortie :
- _response_file_ : permet de définir le contenu du fichier retourné (type _VARBINARY_)
- _response_file_name_ : indique le nom du fichier (type NVARCHAR)
- _response_file_inline_ : _BIT_ permettant d'ouvrir le fichier la visionneuse du navigateur (si disponible, sinon télécharge directement). Faux par défaut. 

Exemple : 
```sql
-- #Route("/api/sampleFileDownload")
-- #HttpPost("sampleFileDownload")
CREATE OR ALTER PROCEDURE [web].[p_sample_file_download] 
AS 
BEGIN
    DECLARE @file VARBINARY(MAX);
    DECLARE @file_name NVARCHAR(255);
    DECLARE @file_mime NVARCHAR(255);
    DECLARE @file_inline BIT = 0;
    
    -- Récupération du fichier et de ses informations
    -- ...

    -- Si c'est un PDF, demande l'affichage préalable dans le navigateur, sinon télécharge directement
    IF (@file_mime = 'application/pdf')
        SET @file_inline = 1;

    SELECT 
         @file AS [response_file]
        ,@file_name AS [response_file_name]
        ,@file_inline AS [response_file_inline]
        ,@file_mime AS [response_content_type]
        ,200 AS [response_status]
    ;
END
GO
```
