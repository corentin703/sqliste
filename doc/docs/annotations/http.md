
# HTTP

These annotations are used to define routes and their associated operations.

## #Route

| Arguments    | Type    | Description                           | Optional | Default Value |
|--------------|---------|---------------------------------------|----------|---------------|
| Path         | string  | The URI at which the procedure is resolved | No       |               |

Examples:
```
#Route("/route")
#Route(Path = "/route")
```


## Operations and HTTP Verbs

These annotations are used to define the HTTP method with which the stored procedure will be resolved.

### #HttpGet

Associates the HTTP GET verb.

Examples:
```
#HttpGet("GetBook")
#HttpGet(Id = "GetBook")
```

### #HttpPost

Associates the HTTP POST verb.

Examples:
```
#HttpPost("CreateBook")
#HttpPost(Id = "CreateBook")
```

### #HttpPut

Associates the HTTP PUT verb.

Examples:
```
#HttpPost("UpdateBook")
#HttpPost(Id = "UpdateBook")
```

### #HttpPatch

Associates the HTTP PATCH verb.

Examples:
```
#HttpPatch("UpdateBook")
#HttpPatch(Id = "UpdateBook")
```

### #HttpDelete

Associates the HTTP DELETE verb.

Examples:
```
#HttpDelete("DeleteBook")
#HttpDelete(Id = "DeleteBook")
```

:::tip

It is possible to define multiple verbs for the same procedure.

:::

:::caution

Defining an operation name is necessary for the JSON OpenAPI to be valid.

:::
