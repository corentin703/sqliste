# Standard Parameters

Here is the exhaustive list of standard parameters, their direction (input / output), and what they are used for.

:::caution

Output parameters are returned via a SELECT statement.
Procedure arguments in output mode (_OUTPUT_) are not supported.

:::

## Request-related Parameters

| Name                   | Direction | .NET Type | Format                                                                                                | SQL Server Type | Description                                                                                                                                     |
|------------------------|-----------|-----------|-------------------------------------------------------------------------------------------------------|-----------------|-------------------------------------------------------------------------------------------------------------------------------------------------|
| _request_content_type_ | Input     | string    | [MIME Type](https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) | VARCHAR(255)    | The MIME type of the request body, extracted from the HTTP _Content-Type_ header.                                                               |
| _request_body_         | Input     | string    | Raw                                                                                                   | NVARCHAR(MAX)   | The request body.                                                                                                                               |
| _request_form_         | Input     | string    | JSON                                                                                                  | NVARCHAR(MAX)   | A JSON that represents the form witch has been submitted by the user through a _form_ content type. When it isn't null, _request_body_ is null. |
| _request_headers_      | Input     | string    | JSON                                                                                                  | NVARCHAR(MAX)   | The request headers, as a key-value dictionary.                                                                                                 |
| _request_cookies_      | Input     | string    | JSON                                                                                                  | NVARCHAR(MAX)   | The cookies attached to the request, as a key-value dictionary.                                                                                 |
| _request_path_         | Input     | string    | Raw                                                                                                   | NVARCHAR(MAX)   | The request path.                                                                                                                               |
| _query_params_         | Input     | string    | JSON                                                                                                  | NVARCHAR(MAX)   | The query parameters (present after the '?' in the URI), as a key-value dictionary.                                                             |
| _path_params_          | Input     | string    | JSON                                                                                                  | NVARCHAR(MAX)   | The parameters resolved from the route, as a key-value dictionary.                                                                              |
| _request_model_        | Input     | string    | JSON                                                                                                  | NVARCHAR(MAX)   | The raw model managed by SQListe, containing all the information about the request.                                                             |
| _error_                | Input     | string    | JSON                                                                                                  | NVARCHAR(MAX)   | A model containing information about the last error that occurred. For more information, refer to TODO.                                         |
| _response_model_       | Input     | string    | JSON                                                                                                  | NVARCHAR(MAX)   | The raw model managed by SQListe, containing all the information about the response in its current state.                                       |

## Response-related Parameters

These parameters can be returned from a procedure via a SELECT statement and will modify the state of the response.

| Name                    | Direction      | .NET Type | Format                                                                                                | SQL Server Type | Description                                                                                                  |
|-------------------------|----------------|-----------|-------------------------------------------------------------------------------------------------------|-----------------|--------------------------------------------------------------------------------------------------------------|
| _response_content_type_ | Input / Output | string    | [MIME Type](https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types) | VARCHAR(255)    | The MIME type of the response body, which will be assigned to the _Content-Type_ HTTP header if not defined. |
| _response_body_         | Input / Output | string    | Raw                                                                                                   | NVARCHAR(MAX)   | The response body.                                                                                           |
| _response_headers_      | Input / Output | string    | JSON                                                                                                  | NVARCHAR(MAX)   | The response headers, as a key-value dictionary.                                                             |
| _response_cookies_      | Input / Output | string    | JSON                                                                                                  | NVARCHAR(MAX)   | The cookie attached to the response. For more information on the format, refer to the... TODO section.       |
| _response_status_       | Input / Output | int       | [HTTP Status](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status)                               | INT             | The response status code.                                                                                    |

:::tip

These parameters can also be passed as procedure arguments.
This allows retrieving the current state of the response, which can be useful after processing by middleware.

:::

## Storage-related Parameters

These parameters can be passed as procedure arguments and returned to modify the state.

| Name              | Direction      | .NET Type | Format  | SQL Server Type | Description                                                                                                             |
|-------------------|----------------|-----------|---------|-----------------|-------------------------------------------------------------------------------------------------------------------------|
| _request_storage_ | Input / Output | string    | _JSON*_ | NVARCHAR(MAX)   | Storage with a lifespan of a request. It is initialized with an empty JSON object at the beginning of a request ('{}'). |
| _session_         | Input / Output | string    | _JSON*_ | NVARCHAR(MAX)   | Access to the HTTP session. For more information, refer to... TODO section.                                             |

:::info

_JSON*_ : Although the content is initially initialized with an empty JSON ('{}') by default, it can be safely changed as this content is not processed by SQListe.

:::

## Middleware-specific Parameters

These parameters can be passed as procedure arguments and returned to modify the state.

| Name   | Direction | .NET Type | Format | SQL Server Type | Description                                                             |
|--------|-----------|-----------|--------|-----------------|-------------------------------------------------------------------------|
| _next_ | Output    | bool      |        | BIT             | If returned as 0, it interrupts the request pipeline. It defaults to 1. |
