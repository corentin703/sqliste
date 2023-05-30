---
sidebar_position: 7
---

# Security

SQListe is designed to provide you with all the necessary keys for the development of a secure application.<br/>
However, certain practices should be taken into consideration during development.

## SQL Injection

When inspecting the database, SQListe retrieves the list of parameters for each procedure,
which allows it to perform parameterized queries (thus avoiding code injections).<br/>
However, it cannot determine whether the content of a parameter provided by the user is dangerous or not.<br/>
To protect against any issues, **never** execute elements coming from the user.

A risky example below (in T-SQL) where request_body is ```DROP DATABASE DB_NAME()```:
```sql
-- #Route("/api/security/example1")
-- #HttpPost("securityExemple1")
CREATE OR ALTER PROCEDURE [web].[p_security_exemple_1]
    @request_body NVARCHAR(MAX) -- Argument injected via a parameterized query = OK
AS 
BEGIN
    -- ...

    EXEC(@request_body); -- Very risky code as it will execute an unescaped literal string

    -- ...
END
GO
```

In our example, albeit exaggerated, this will result in the deletion of the database.<br/>
One of the major sources of vulnerability is the dynamic creation of queries that take user data as parameters.
If you need to do this, make sure to use the mechanism offered by your DBMS to escape them within your stored procedure.

A second example illustrating this case:
```sql
-- #Route("/api/security/example2")
-- #HttpPost("securityExemple2")
CREATE OR ALTER PROCEDURE [web].[p_security_exemple_2]
    @request_body NVARCHAR(MAX) -- Argument injected via a parameterized query = OK
AS 
BEGIN
    DECLARE @data NVARCHAR(MAX);
    DECLARE @table NVARCHAR(100);
    DECLARE @col NVARCHAR(100);
    
    DECLARE @query NVARCHAR(MAX);
    
    -- Retrieval of raw values
    SET @data = JSON_VALUE(@request_body);

    -- Retrieval of the corresponding column and table from internal (secured) data
    
    -- Concatenation without escaping values
    SET @query = 'INSERT INTO ' + @table + ' ('+ @col + ') VALUES (' + @data + ')';
    EXEC(@query);

    -- ...
END
GO
```

In this case, if a malicious person sends a request with SQL code in the data field of the JSON object, it will be executed.

Here is a secure way to execute the above example in SQL Server:
```sql
-- #Route("/api/security/example2Secure")
-- #HttpPost("securityExemple2Securre")
CREATE OR ALTER PROCEDURE [web].[p_security_exemple_2_secure]
    @request_body NVARCHAR(MAX) -- Argument injected via a parameterized query = OK
AS 
BEGIN
    DECLARE @data NVARCHAR(MAX);
    DECLARE @table NVARCHAR(100);
    DECLARE @col NVARCHAR(100);
    
    DECLARE @query NVARCHAR(MAX);
    
    -- Retrieval of raw values
    SET @data = JSON_VALUE(@request_body, '$.data');

    -- Retrieval of the corresponding column and table from internal (secured) data
    
    -- Setting up the above query using safe data and executing with escaped user data
    SET @query = 'INSERT INTO ' + @table + ' ('+ @col + ') VALUES (@data_esc)';
    EXEC sp_executesql   
          @query,  
          N'@data_esc NVARCHAR(MAX)',  
          @data_esc = @data
    ;

    -- ...
END
GO
```

## User Permissions

To go further, it may be desirable to create a user with restricted rights dedicated to SQListe.<br/>
Therefore, do not use the root user in production, and instead use a user with intermediate rights.
