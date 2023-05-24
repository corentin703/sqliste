---
sidebar_position: 1
---

# Cookies

A cookie is data stored in the browser, typically limited to 4KB, and it will be sent with each request.
They allow, for example, remembering user preferences or linking a user to their HTTP session.

## Reading

Cookies can be read from a key-value dictionary available in the standard parameter request_cookies.
The key corresponds to the name of your cookie, and the value corresponds to the data contained within it.

Here's an example of reading a cookie that counts the number of user requests.

```sql
#Route("/api/example/cookie")
#HttpGet("ExampleCookie")
CREATE OR ALTER PROCEDURE [web].[p_example_cookie]
    @request_cookies NVARCHAR(MAX) 
AS 
BEGIN
    DECLARE @visit_counter BIGINT;
    
    -- Retrieve the value of the "visitCounter" cookie
    SET @visit_counter = ISNULL(JSON_VALUE(@request_cookies, '$.visitCounter'), 0) + 1;

    IF (@visit_counter = 1)
    BEGIN
        SELECT 
             '{ "message": "This is your first visit. Welcome!" }' AS [response_body]
            ,200 AS [response_status]
        ;
        RETURN;
    END 

    SELECT 
         '{ "message": "This is your visit number ' + CAST(@visit_counter AS NVARCHAR(MAX)) + '!" }' AS [response_body]
        ,200 AS [response_status]
    ;
END
GO
```

## Writing

To write or modify a cookie, you need to modify the response_cookies parameter.
Unlike reading, it's not a dictionary but an array of parameters that allows you to define multiple properties for cookies.

Description of a cookie configuration:
```json lines
{
  "name": "name",
  "value": "value",
  "expires": "2023-06-12T00:00:00", // Expiration date in [ISO 8601](https://en.wikipedia.org/wiki/ISO_8601) format
  "domain": "Domain of validity", // By default, the domain corresponds to the origin. You can set your root domain to include all your subdomains. If you specify another domain, keep in mind that cross-origin cookies are being phased out.
  "path": "/api/example", // The cookie will only be sent if the route starts with /api/example
  "secure": true, // The cookie will only be sent when the connection is HTTPS
  "sameSite": "Lax", // Defines the cookie sending policy for requests to other sites
  "httpOnly": false, // Makes the cookie inaccessible via JavaScript
  "maxAge": 3600 // Maximum lifetime of the cookie (in seconds), complementary to the `expires` parameter, which takes a date
}
```

To learn more about cookies and the possibilities of these parameters, you can refer to this [resource](https://developer.mozilla.org/en-US/docs/Web/HTTP/Cookies)  that details the purpose of each parameter. The mandatory parameters in this template are ```name``` and ```value```, along with either ```expires``` or ```maxAge``` (you can choose one).

Let's continue with our previous example by adding the necessary code to set or update the cookie if it doesn't exist.

```sql
#Route("/api/example/cookie")
#HttpGet("ExampleCookie")
CREATE OR ALTER PROCEDURE [web].[p_example_cookie]
    @request_cookies NVARCHAR(MAX),
    @response_cookies NVARCHAR(MAX)
AS 
BEGIN
    DECLARE @visit_counter BIGINT;
    DECLARE @visit_counter_cookie NVARCHAR(MAX);
    
    -- Retrieve the current value
    SET @visit_counter = ISNULL(JSON_VALUE(@request_cookies, '$.visitCounter'), 0) + 1;

    -- Set up our cookie
    SET @visit_counter_cookie = (
        SELECT 
            'visitCounter' AS [name],
            3600 * 24 * 7 AS [maxAge],
            @visit_counter AS [value]
        FOR JSON PATH
    );
    
    -- Since FOR JSON PATH always returns an array, we retrieve the first element (the only one here)
    SET @visit_counter_cookie = JSON_QUERY(@visit_counter_cookie, '$[0]');

    -- Add it to the list of cookies we want to modify
    SET @response_cookies = JSON_MODIFY(@response_cookies, 'append $', @visit_counter_cookie); 

    IF (@visit_counter = 1)
    BEGIN
        SELECT 
             '{ "message": "This is your first visit. Welcome!" }' AS [response_body]
            ,200 AS [response_status]
            ,@response_cookies AS [response_cookies]
        ;
        RETURN;
    END 

    SELECT 
         '{ "message": "This is your visit number ' + CAST(@visit_counter AS NVARCHAR(MAX)) + '!" }' AS [response_body]
        ,200 AS [response_status]
        ,@response_cookies AS [response_cookies]
    ;
END
GO
```
