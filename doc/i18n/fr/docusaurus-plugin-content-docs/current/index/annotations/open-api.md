
# OpenAPI

La documentation des routes dans OpenApi se fait via des annotations. 
Certaines d'entre-elles permettent à SQListe de déduire des informations à propos des procédures, et vous dispense parfois de fournir certains renseignements.

## #Accepts

Défini quel format de contenu la procédure va accepter.

| Arguments | Type   | Description                                                                                        | Optionnel | Valeur par défaut |
|-----------|--------|----------------------------------------------------------------------------------------------------|-----------|-------------------|
| Mime      | string | [Type MIME](https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) | Non       |                   |

Exemples :
```
#Accepts("application/json")
#Accepts(Mime = "application/json")
```

## #Takes

Défini quel type de contenu la procédure va accepter.
Cette annotation peut être écrite plusieurs fois pour une seule procédure afin de définir plusieurs retours possibles.

| Arguments   | Type   | Description                                       | Optionnel | Valeur par défaut |
|-------------|--------|---------------------------------------------------|-----------|-------------------|
| Type        | string | Nom d'un type renseigné dans la procédure OpenAPI | Oui       | ""                |
| Required    | bool   | Vrai si requis.                                   | Oui       | Vrai              |
| Description | string | Description de la réponse                         | Oui       | ""                |

Exemples :
```
#Takes("loginModel", true, "Informations de connexion de l'utilisateur")
#Takes(Type = "loginModel", Required = true, Description = "Informations de connexion de l'utilisateur")
```

## #Produces

Défini quel format de contenu la procédure va produire.

| Arguments | Type   | Description                                                                                        | Optionnel | Valeur par défaut |
|-----------|--------|----------------------------------------------------------------------------------------------------|-----------|-------------------|
| Mime      | string | [Type MIME](https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) | Non       |                   |

Exemples :
```
#Produces("application/json")
#Produces(Mime = "application/json")
```

:::info

Lorsque `#Produces` est défini, vous avez la possibilité d'omettre le paramètre _response_content_type_.<br/>
S'il n'est pas défini durant le _pipeline_, SQListe appliquera le type MIME de la premiere occurrence de `#Produces` trouvée à la réponse.

Nota : si le pramètre _response_content_type_ est fourni à un moment dans le _pipeline_, celui-ci restera prioritaire.

:::


## #Responds

Défini quel type de contenu la procédure va produire.
Cette annotation peut être écrite plusieurs fois pour une seule procédure afin de définir plusieurs retours possibles.

| Arguments   | Type   | Description                                       | Optionnel | Valeur par défaut |
|-------------|--------|---------------------------------------------------|-----------|-------------------|
| Type        | string | Nom d'un type renseigné dans la procédure OpenAPI | Oui       | ""                |
| Status      | string | Statut HTTP de la réponse                         | Oui       | 200 OK            |
| Description | string | Description de la réponse                         | Oui       | ""                |

Exemples :
```
#Responds("applicationMessage", 200, "Retourne un message à afficher à l'utilisateur")
#Responds(Type = "applicationMessage", Status = 200, Description = "Retourne un message à afficher à l'utilisateur")
```
