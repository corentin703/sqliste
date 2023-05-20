---
sidebar_position: 5
---

# Stockage

Afin de simplifier le développement, SQListe propose un accès à deux emplacements permettant de stocker des données pour une utilisation ultérieure.

Le format recommandé pour ces deux emplacements est le JSON. Ceux-ci sont initialisés avec un JSON vide ```{}``` et font partie intégrante de l'état de la réponse.

:::note

Le contenu de ces stockages est seulement initialisé par SQListe.<br/>
Étant donné qu'ils ne font l'objet d'aucun traitement particulier, vous pouvez tout à fait définir un autre format en écrasant la valuer par défaut.

:::

:::info

Le client n'a pas accès au contenu de ces emplacements de stockage : ils restent interne à SQListe.<br/>
Vous pouvez donc y stocker des données sensibles sereinement.  

:::

## Session

Le premier d'entre-eux donne accès à la session HTTP.<br/>
La session est un emplacement de stockage généralement résolu à partir d'un jeton enregistré dans un cookie HTTP (ce qui est notre cas). 
Sa durée de vie est de 20 minutes par défaut (configurable dans le fichier _appsettings.json_).

Exemple d'utilisation :

```sql
#Route("/api/exemple/session")
#HttpGet("ExempleSession")
CREATE OR ALTER PROCEDURE [web].[p_exemple_session]
    @session NVARCHAR(MAX) 
AS 
BEGIN
    IF (JSON_VALUE(@session, '$.calculSavant' IS NULL))
    BEGIN
        DECLARE @calcul_savant INT;
        SET @calcul_savant = 1 + 1;
    
        SET @session = JSON_MODIFY(@session, '$.calculSavant', @calcul_savant);
    END
    
    -- Autres traitements...

    SELECT 
         @session AS [session]
    ;
END
GO
```

Le calcul très savant et coûteux fait dans l'exemple ci-dessus ne sera exécuté que si le résultat n'est pas présent dans la session.

:::info

La session HTTP n'est réellement altérée qu'à la fin du traitement du _pipeline_.

:::

:::note Initialisation

La session (et le cookie correspondant) ne sera initialisée que si elle est demandée par une procédure.

:::

## Stockage du _pipeline_

Afin de transmettre des informations au sein d'un _pipeline_, SQListe propose un stockage ayant une durée de vie d'une requête.

Exemple d'utilisation :

```sql
-- #Middleware(Order = 1, After = false)
CREATE OR ALTER PROCEDURE [web].[p_middleware_log_request_start]
    @request_headers NVARCHAR(MAX),
    @pipeline_storage NVARCHAR(MAX) 
AS
BEGIN 
    DECLARE @username NVARCHAR(50);
    
    -- Lecture du jetton, vérification et récupération de l'utilisateur...
    
    SET @pipeline_storage = JSON_MODIFY(@pipeline_storage, '$.username', @username);

    SELECT 
         @pipeline_storage AS [pipeline_storage]
    ;
END
GO
```

Dans cet exemple, nous récupérons le nom de l'utilisateur et l'insérons dans le stockage du _pipeline_.<br/>
Cela permet à un intergiciel ultérieur, ou à un contrôleur d'accéder à cette donnée sans réaliser de traitement supplémentaire.

:::note Utilisation

Ces deux stockages s'utilisent de la même manière, la seule différence étant la durée de vie.<br/>
Par conséquent, la session sera plus appropriée pour stocker des données coûteuses à calculer / mise en cache, tandis que le stockage du _pipeline_ 
sera plus destiné à l'enregistrement de données changeant souvent, et dont l'obtention est rapide.

:::
