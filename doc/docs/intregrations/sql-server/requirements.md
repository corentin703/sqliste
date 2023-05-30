---
sidebar_position: 1
---

# Prerequisites

## Version

SQListe requires a minimum of SQL Server 2017.<br/>
Azure SQL Edge is supported.<br/>
The SQL Server connector is tested with SQL Server 2017 Express.ess.

## Database

You need to enable the _service broker_ on the database intended to be used with SQListe in order to use the event-driven features of the application.

## JSON Support

Many parameters within SQListe use the JSON format.<br/>
SQL Server provides a relatively comprehensive set of functions for reading and writing data in JSON format.<br/>
[Learn more](https://learn.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server?view=sql-server-ver16).

## Dynamic Query Parameterization

As seen in the examples in the Security section, it is dangerous to execute dynamically composed SQL without escaping its components.
To address this issue, SQL Server provides utilities.<br/>

[Learn more](https://learn.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-executesql-transact-sql?view=sql-server-ver15).
