---
sidebar_position: 2
---

# Structure

## Schemas

After executing the migration, the connected database is organized into 3 schemas.

### Schema sqliste

This schema contains all the procedures necessary for the proper functioning of SQListe.

It includes procedures for introspecting the database, as well as generating and obtaining the OpenAPI JSON.

:::info

The proper functioning of the application cannot be guaranteed if any of these procedures are modified without prior consultation of the documentation.

:::

### Schema _web_

This schema is intended to host procedures defining controllers and middlewares.

:::caution

Any object present in this schema can potentially be exposed via the web API. Do not place your business objects and other sensitive procedures in this schema.

:::

### Schema _dbo_ / Default Schema

The default schema is not used by SQListe. Therefore, you can safely develop in this schema, knowing that nothing can be called from the web without going through a procedure in the _web_ schema.
