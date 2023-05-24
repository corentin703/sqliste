"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[53],{1109:e=>{e.exports=JSON.parse('{"pluginId":"default","version":"current","label":"Procha\xeene","banner":null,"badge":false,"noIndex":false,"className":"docs-version-current","isLast":true,"docsSidebars":{"docSidebar":[{"type":"link","label":"Introduction","href":"/sqliste/fr/docs/intro","docId":"intro"},{"type":"link","label":"Concepts","href":"/sqliste/fr/docs/concepts","docId":"concepts"},{"type":"category","label":"D\xe9marrage","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"Installation","href":"/sqliste/fr/docs/getting-started/installation","docId":"getting-started/installation"},{"type":"link","label":"D\xe9ploiement","href":"/sqliste/fr/docs/getting-started/deployment","docId":"getting-started/deployment"}],"href":"/sqliste/fr/docs/category/getting-started"},{"type":"category","label":"Annotations","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"HTTP","href":"/sqliste/fr/docs/annotations/http","docId":"annotations/http"},{"type":"link","label":"Intergiciels","href":"/sqliste/fr/docs/annotations/middlewares","docId":"annotations/middlewares"},{"type":"link","label":"OpenApi","href":"/sqliste/fr/docs/annotations/open-api","docId":"annotations/open-api"}],"href":"/sqliste/fr/docs/category/annotations"},{"type":"category","label":"Int\xe9grations","collapsible":true,"collapsed":true,"items":[{"type":"category","label":"SQL Server","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"Pr\xe9requis","href":"/sqliste/fr/docs/intregrations/sql-server/requirements","docId":"intregrations/sql-server/requirements"},{"type":"link","label":"Structure","href":"/sqliste/fr/docs/intregrations/sql-server/structure","docId":"intregrations/sql-server/structure"},{"type":"link","label":"D\xe9finir un contr\xf4leur","href":"/sqliste/fr/docs/intregrations/sql-server/define-controller","docId":"intregrations/sql-server/define-controller"},{"type":"link","label":"D\xe9finir un intergiciel","href":"/sqliste/fr/docs/intregrations/sql-server/define-middleware","docId":"intregrations/sql-server/define-middleware"},{"type":"link","label":"Stockage","href":"/sqliste/fr/docs/intregrations/sql-server/storage","docId":"intregrations/sql-server/storage"},{"type":"category","label":"Op\xe9rations sur HTTP","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"Cookies","href":"/sqliste/fr/docs/intregrations/sql-server/http-operations/cookies","docId":"intregrations/sql-server/http-operations/cookies"},{"type":"link","label":"En-t\xeate HTTP","href":"/sqliste/fr/docs/intregrations/sql-server/http-operations/headers","docId":"intregrations/sql-server/http-operations/headers"}],"href":"/sqliste/fr/docs/category/http-operations"},{"type":"link","label":"Gestion d\'erreurs","href":"/sqliste/fr/docs/intregrations/sql-server/error-handling","docId":"intregrations/sql-server/error-handling"},{"type":"link","label":"OpenAPI","href":"/sqliste/fr/docs/intregrations/sql-server/openapi","docId":"intregrations/sql-server/openapi"}],"href":"/sqliste/fr/docs/category/sql-server"}],"href":"/sqliste/fr/docs/category/integrations"},{"type":"category","label":"Index","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"Param\xe8tres standards","href":"/sqliste/fr/docs/index/standard-parameters","docId":"index/standard-parameters"}],"href":"/sqliste/fr/docs/category/index"}]},"docs":{"annotations/http":{"id":"annotations/http","title":"HTTP","description":"Ces annotations servent \xe0 d\xe9finir les routes, ainsi que les op\xe9rations qui leur sont associ\xe9es.","sidebar":"docSidebar"},"annotations/middlewares":{"id":"annotations/middlewares","title":"Intergiciels","description":"Un intergiciel se d\xe9finie gr\xe2ce \xe0 l\'annotation `#Middleware`.","sidebar":"docSidebar"},"annotations/open-api":{"id":"annotations/open-api","title":"OpenApi","description":"Arrive bient\xf4t...","sidebar":"docSidebar"},"concepts":{"id":"concepts","title":"Concepts","description":"Dans SQListe, l\'id\xe9e principale est de permettre \xe0 un d\xe9veloppeur SQL de d\xe9finir un","sidebar":"docSidebar"},"getting-started/deployment":{"id":"getting-started/deployment","title":"D\xe9ploiement","description":"Arrive bient\xf4t...","sidebar":"docSidebar"},"getting-started/installation":{"id":"getting-started/installation","title":"Installation","description":"SQListe est une application ASP.NET Core se connectant \xe0 votre base de donn\xe9es,","sidebar":"docSidebar"},"index/standard-parameters":{"id":"index/standard-parameters","title":"Param\xe8tres standards","description":"Voici la liste exhaustive des param\xe8tres standards, leur direction (entr\xe9e / sortie), et \xe0 quoi ils servent.","sidebar":"docSidebar"},"intregrations/sql-server/define-controller":{"id":"intregrations/sql-server/define-controller","title":"D\xe9finir un contr\xf4leur","description":"Un contr\xf4leur prend la forme d\'une proc\xe9dure stock\xe9e situ\xe9e dans le sch\xe9ma web.","sidebar":"docSidebar"},"intregrations/sql-server/define-middleware":{"id":"intregrations/sql-server/define-middleware","title":"D\xe9finir un intergiciel","description":"Un intergiciel prend la forme d\'une proc\xe9dure stock\xe9e situ\xe9e dans le sch\xe9ma web et ayant une annotation `#Middleware`.","sidebar":"docSidebar"},"intregrations/sql-server/error-handling":{"id":"intregrations/sql-server/error-handling","title":"Gestion d\'erreurs","description":"Lorsqu\'une erreur se produit dans une proc\xe9dure au cours du pipeline, cela l\'interrompt et retourne une erreur 500 (INTERNAL SERVER ERROR).","sidebar":"docSidebar"},"intregrations/sql-server/http-operations/cookies":{"id":"intregrations/sql-server/http-operations/cookies","title":"Cookies","description":"Un cookie est une donn\xe9e stock\xe9e dans le navigateur, g\xe9n\xe9ralement limit\xe9e \xe0 4ko, et qui sera jointe \xe0 chaque requ\xeate.","sidebar":"docSidebar"},"intregrations/sql-server/http-operations/headers":{"id":"intregrations/sql-server/http-operations/headers","title":"En-t\xeate HTTP","description":"Les en-t\xeates HTTP permettent au client, comme au serveur, de joindre des informations suppl\xe9mentaires \xe0 la requ\xeate ou \xe0 la r\xe9ponse.","sidebar":"docSidebar"},"intregrations/sql-server/openapi":{"id":"intregrations/sql-server/openapi","title":"OpenAPI","description":"Arrive bient\xf4t...","sidebar":"docSidebar"},"intregrations/sql-server/requirements":{"id":"intregrations/sql-server/requirements","title":"Pr\xe9requis","description":"Version","sidebar":"docSidebar"},"intregrations/sql-server/storage":{"id":"intregrations/sql-server/storage","title":"Stockage","description":"Afin de simplifier le d\xe9veloppement, SQListe propose un acc\xe8s \xe0 deux emplacements permettant de stocker des donn\xe9es pour une utilisation ult\xe9rieure.","sidebar":"docSidebar"},"intregrations/sql-server/structure":{"id":"intregrations/sql-server/structure","title":"Structure","description":"Sch\xe9mas","sidebar":"docSidebar"},"intro":{"id":"intro","title":"Introduction","description":"SQListe est un framework \xe0 destination des d\xe9veloppeurs SQL souhaitant r\xe9aliser un","sidebar":"docSidebar"}}}')}}]);