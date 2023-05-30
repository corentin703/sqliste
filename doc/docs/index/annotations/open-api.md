OpenAPI
=======

Documentation of routes in OpenAPI is done through annotations. Some of them allow SQListe to deduce information about procedures, and sometimes eliminate the need for certain details.

#Accepts
--------

Defines the content format that the procedure will accept.

| Argument | Type   | Description                                                                                           | Optional | Default Value |
|----------|--------|-------------------------------------------------------------------------------------------------------|----------|---------------|
| Mime     | string | [MIME Type](https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) | No       |               |

Examples:
```
#Accepts("application/json")
#Accepts(Mime = "application/json")
```

#Takes
------

Defines the content type that the procedure will accept. This annotation can be written multiple times for a single procedure to define multiple possible inputs.

| Argument    | Type   | Description                                     | Optional | Default Value |
|-------------|--------|-------------------------------------------------|----------|---------------|
| Type        | string | Name of a type defined in the OpenAPI procedure | Yes      | ""            |
| Required    | bool   | True if required                                | Yes      | True          |
| Description | string | Description of the response                     | Yes      | ""            |

Examples:
```
#Takes("loginModel", true, "User login information")
#Takes(Type = "loginModel", Required = true, Description = "User login information")
```

#Produces
---------

Defines the content format that the procedure will produce.

| Argument | Type   | Description                                                                                           | Optional | Default Value |
|----------|--------|-------------------------------------------------------------------------------------------------------|----------|---------------|
| Mime     | string | [MIME Type](https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) | No       |               |

Examples:
```
#Produces("application/json")
#Produces(Mime = "application/json")
```

:::info

When `#Produces` is defined, you have the option to omit the _response\_content\_type_ parameter. If it is not defined during the pipeline, SQListe will apply the MIME type of the first occurrence of `#Produces` found to the response.

Note: If the _response\_content\_type_ parameter is provided at any point in the pipeline, it will take precedence.

:::

#Responds
---------

Defines the content type that the procedure will produce. This annotation can be written multiple times for a single procedure to define multiple possible outputs.

| Argument    | Type   | Description                                     | Optional | Default Value |
|-------------|--------|-------------------------------------------------|----------|---------------|
| Type        | string | Name of a type defined in the OpenAPI procedure | Yes      | ""            |
| Status      | string | HTTP status of the response                     | Yes      | 200 OK        |
| Description | string | Description of the response                     | Yes      | ""            |

Examples:
```
#Responds("applicationMessage", 200, "Returns a message to display to the user")
#Responds(Type = "applicationMessage", Status = 200, Description = "Returns a message to display to the user")
```

Please note that you can copy and paste this content directly into a Markdown file.

