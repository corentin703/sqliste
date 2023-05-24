# Paramètres standards

Voici la liste exhaustive des paramètres standards, leur direction (entrée / sortie), et à quoi ils servent.

:::caution

Les paramètres de sortie sont à retourner via un SELECT.<br/> 
Les arguments de procédure en mode sortie (_OUTPUT_) ne sont pas pris en charge.

:::

## À propos la requête

| Nom                    | Direction | Type .NET | Format                                                                                             | Type SQL-Server | Description                                                                                                                                                           |
|------------------------|-----------|-----------|----------------------------------------------------------------------------------------------------|-----------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| _request_content_type_ | Entrée    | string    | [Type MIME](https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) | VARCHAR(255)    | [Type MIME](https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) du corp de la requête, provenant de l'en-tête HTTP _Content-Type_. |
| _request_body_         | Entrée    | string    | Brut                                                                                               | NVARCHAR(MAX)   | Corp de la requête.                                                                                                                                                   |
| _request_headers_      | Entrée    | string    | JSON                                                                                               | NVARCHAR(MAX)   | En-têtes de la requête, dictionnaire clé-valeur.                                                                                                                      |
| _request_cookies_      | Entrée    | string    | JSON                                                                                               | NVARCHAR(MAX)   | Cookie joint à la requête, dictionnaire clé-valeur.                                                                                                                   |
| _request_path_         | Entrée    | string    | Brut                                                                                               | NVARCHAR(MAX)   | Route de la requête.                                                                                                                                                  |
| _query_params_         | Entrée    | string    | JSON                                                                                               | NVARCHAR(MAX)   | Paramètres de requête (présent après le _?_ dans l'URI), dictionnaire clé-valeur.                                                                                     |
| _path_params_          | Entrée    | string    | JSON                                                                                               | NVARCHAR(MAX)   | Paramètres résolus depuis la route, dictionnaire clé-valeur.                                                                                                          |
| _request_model_        | Entrée    | string    | JSON                                                                                               | NVARCHAR(MAX)   | Modèle _brut_ géré par SQListe, contenant toutes les informations de la requête.                                                                                      |
| _error_                | Entrée    | string    | JSON                                                                                               | NVARCHAR(MAX)   | .Modèle contenant des informations à propos de la dernière erreur survenue. Pour en savoir plus, cf. TODO.                                                            |
| _response_model_       | Entrée    | string    | JSON                                                                                               | NVARCHAR(MAX)   | Modèle _brut_ géré par SQListe, contenant toutes les informations de la réponse en l'état actuel.                                                                     |

## À propos la réponse

Ces paramètres peuvent-être retourné depuis une procédure via un SELECT, et viendront altérer l'état de la réponse.

| Nom                     | Direction       | Type .NET | Format                                                                                             | Type SQL-Server | Description                                                                                                                                                                                 |
|-------------------------|-----------------|-----------|----------------------------------------------------------------------------------------------------|-----------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| _response_content_type_ | Entrée / Sortie | string    | [Type MIME](https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) | VARCHAR(255)    | [Type MIME](https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) du corp de la réponse, qui sera assignée à l'en-tête HTTP _Content-Type_ si non définie. |
| _response_body_         | Entrée / Sortie | string    | Brut                                                                                               | NVARCHAR(MAX)   | Corp de la réponse.                                                                                                                                                                         |
| _response_headers_      | Entrée / Sortie | string    | JSON                                                                                               | NVARCHAR(MAX)   | En-têtes de la réponse, dictionnaire clé-valeur.                                                                                                                                            |
| _response_cookies_      | Entrée / Sortie | string    | JSON                                                                                               | NVARCHAR(MAX)   | Cookie joint à la réponse. Pour en savoir plus sur le format : cf. section... TODO                                                                                                          |
| _response_status_       | Entrée / Sortie | int       | [Statut HTTP](https://developer.mozilla.org/fr/docs/Web/HTTP/Status)                               | INT             | Code de statut de la réponse.                                                                                                                                                               |

:::tip

Ces paramètres peuvent aussi être pris en argument de procédure. <br/>
Cela permet de récupérer l'état actuel de la réponse, ce qui peut être utile après un traitement par intergiciel.

:::

## À propos du stockage

Ces paramètres peuvent-être pris en arguments de procédure, ainsi que retourné pour altérer l'état.

| Nom               | Direction       | Type .NET | Format  | Type SQL-Server | Description                                                                                                          |
|-------------------|-----------------|-----------|---------|-----------------|----------------------------------------------------------------------------------------------------------------------|
| _request_storage_ | Entrée / Sortie | string    | _JSON*_ | NVARCHAR(MAX)   | Stockage ayant une durée de vie d'une requête. Il est initialisé à un objet JSON vide au début d'une requête ('{}'). |
| _session_         | Entrée / Sortie | string    | _JSON*_ | NVARCHAR(MAX)   | Accès à la session HTTP. Pour en savoir plus : TODO.                                                                 |

:::info

_JSON*_ : Bien que le contenu soit initialisé avec un JSON vide ('{}') par défaut, il peut néanmoins être changé de 
manière sûre étant donné que ce contenu ne fait pas l'objet d'un traitement de la part de SQListe.

:::

## Paramètres spécifiques aux intergiciels

Ces paramètres peuvent-être pris en arguments de procédure, ainsi que retourné pour altérer l'état.

| Nom    | Direction | Type .NET | Format | Type SQL-Server | Description                                                                   |
|--------|-----------|-----------|--------|-----------------|-------------------------------------------------------------------------------|
| _next_ | Sortie    | bool      |        | BIT             | Si retourné à 0, interromp la _pipeline_ de requête. Est égal à 1 par défaut. |
