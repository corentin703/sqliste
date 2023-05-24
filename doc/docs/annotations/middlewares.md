# Middleware

A middleware is defined using the `#Middleware` annotation.

| Arguments    | Type    | Description                                                    | Optional | Default Value |
|--------------|---------|----------------------------------------------------------------|----------|---------------|
| Order        | number  | Execution order relative to other middlewares.                 | Yes      | 1             |
| PathStarts   | string  | Targets a subset of routes for the execution of this middleware. | Yes      | /             |
| After        | boolean | If false, the middleware will be executed before the procedure. If true, it will be executed after. | Yes | false |

Examples:
```
#Middleware(Order = 1, PathStarts = '/api')
#Middleware(Order = 1, PathStarts = '/api', After = true)
```

