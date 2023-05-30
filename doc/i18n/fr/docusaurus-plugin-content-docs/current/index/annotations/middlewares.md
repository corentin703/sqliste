
# Intergiciels

Un intergiciel se définie grâce à l'annotation ```#Middleware```.

| Arguments               | Type            | Description                                                                                 | Optionnel | Valeur par défaut |
|-------------------------|-----------------|---------------------------------------------------------------------------------------------|-----------|-------------------|
| Order                   | number          | Ordre d'exécution par rapport aux autres intergiciels.                                      | Oui       | 1                 |
| PathStarts              | string          | Permet de cibler un sous ensemble de route concernées par l'exécution de cet intergiciel.   | Oui       | /                 |
| After                   | boolean         | Si faux, l'intergiciel sera exécuté avant la procédure. Si vrai, il sera exécuté après.     | Oui       | faux              |

Exemples : 
```
#Middleware(Order = 1, PathStarts = "/api")
#Middleware(Order = 1, PathStarts = "/api", After = true)
```

