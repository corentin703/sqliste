
# HTTP

Ces annotations servent à définir les routes, ainsi que les opérations qui leur sont associées.

## #Route

| Arguments               | Type            | Description                              | Optionnel | Valeur par défaut |
|-------------------------|-----------------|------------------------------------------|-----------|-------------------|
| Path                    | string          | URI à laquelle la procédure sera résolue | Non       |                   |

Exemples :
```
#Route("/route")
#Route(Path = "/route")
```

## Opérations et verbes HTTP

Ces annotations permettent de définir la méthode HTTP avec laquelle la procédure stockée sera résolue. 

| Arguments               | Type            | Description                               | Optionnel | Valeur par défaut |
|-------------------------|-----------------|-------------------------------------------|-----------|-------------------|
| Id                      | string          | Nom de l'opération (pour le JSON OpenAPI) | Oui       |                   |

### #HttpGet

Associe le verbe HTTP GET.

Exemples :
```
#HttpGet("GetBook")
#HttpGet(Id = "GetBook")
```

### #HttpPost

Associe le verbe HTTP POST.

Exemples :
```
#HttpPost("CreateBook")
#HttpPost(Id = "CreateBook")
```

### #HttpPut

Associe le verbe HTTP PUT.

Exemples :
```
#HttpPost("UpdateBook")
#HttpPost(Id = "UpdateBook")
```

### #HttpPatch

Associe le verbe HTTP PATCH.

Exemples :
```
#HttpPatch("UpdateBook")
#HttpPatch(Id = "UpdateBook")
```

### #HttpDelete

Associe le verbe HTTP DELETE.

Exemples :
```
#HttpDelete("DeleteBook")
#HttpDelete(Id = "DeleteBook")
```

:::tip

Il est possible de définir plusieurs verbes pour une même procédure.

:::

:::caution

Définir un nom d'opération est nécessaire pour que le JSON OpenAPI soit valide.

:::
