"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[4839],{9868:(e,n,t)=>{t.d(n,{Zo:()=>u,kt:()=>f});var r=t(6687);function a(e,n,t){return n in e?Object.defineProperty(e,n,{value:t,enumerable:!0,configurable:!0,writable:!0}):e[n]=t,e}function o(e,n){var t=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);n&&(r=r.filter((function(n){return Object.getOwnPropertyDescriptor(e,n).enumerable}))),t.push.apply(t,r)}return t}function l(e){for(var n=1;n<arguments.length;n++){var t=null!=arguments[n]?arguments[n]:{};n%2?o(Object(t),!0).forEach((function(n){a(e,n,t[n])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(t)):o(Object(t)).forEach((function(n){Object.defineProperty(e,n,Object.getOwnPropertyDescriptor(t,n))}))}return e}function s(e,n){if(null==e)return{};var t,r,a=function(e,n){if(null==e)return{};var t,r,a={},o=Object.keys(e);for(r=0;r<o.length;r++)t=o[r],n.indexOf(t)>=0||(a[t]=e[t]);return a}(e,n);if(Object.getOwnPropertySymbols){var o=Object.getOwnPropertySymbols(e);for(r=0;r<o.length;r++)t=o[r],n.indexOf(t)>=0||Object.prototype.propertyIsEnumerable.call(e,t)&&(a[t]=e[t])}return a}var i=r.createContext({}),p=function(e){var n=r.useContext(i),t=n;return e&&(t="function"==typeof e?e(n):l(l({},n),e)),t},u=function(e){var n=p(e.components);return r.createElement(i.Provider,{value:n},e.children)},d="mdxType",m={inlineCode:"code",wrapper:function(e){var n=e.children;return r.createElement(r.Fragment,{},n)}},c=r.forwardRef((function(e,n){var t=e.components,a=e.mdxType,o=e.originalType,i=e.parentName,u=s(e,["components","mdxType","originalType","parentName"]),d=p(t),c=a,f=d["".concat(i,".").concat(c)]||d[c]||m[c]||o;return t?r.createElement(f,l(l({ref:n},u),{},{components:t})):r.createElement(f,l({ref:n},u))}));function f(e,n){var t=arguments,a=n&&n.mdxType;if("string"==typeof e||a){var o=t.length,l=new Array(o);l[0]=c;var s={};for(var i in n)hasOwnProperty.call(n,i)&&(s[i]=n[i]);s.originalType=e,s[d]="string"==typeof e?e:a,l[1]=s;for(var p=2;p<o;p++)l[p]=t[p];return r.createElement.apply(null,l)}return r.createElement.apply(null,t)}c.displayName="MDXCreateElement"},485:(e,n,t)=>{t.r(n),t.d(n,{assets:()=>i,contentTitle:()=>l,default:()=>m,frontMatter:()=>o,metadata:()=>s,toc:()=>p});var r=t(8792),a=(t(6687),t(9868));const o={sidebar_position:3},l="D\xe9finir un contr\xf4leur",s={unversionedId:"intregrations/sql-server/define-controller",id:"intregrations/sql-server/define-controller",title:"D\xe9finir un contr\xf4leur",description:"Un contr\xf4leur prend la forme d'une proc\xe9dure stock\xe9e situ\xe9e dans le sch\xe9ma web.",source:"@site/docs/intregrations/sql-server/define-controller.md",sourceDirName:"intregrations/sql-server",slug:"/intregrations/sql-server/define-controller",permalink:"/sqliste/docs/intregrations/sql-server/define-controller",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/doc/docs/intregrations/sql-server/define-controller.md",tags:[],version:"current",sidebarPosition:3,frontMatter:{sidebar_position:3},sidebar:"docSidebar",previous:{title:"Structure",permalink:"/sqliste/docs/intregrations/sql-server/structure"},next:{title:"D\xe9finir un intergiciel",permalink:"/sqliste/docs/intregrations/sql-server/define-middleware"}},i={},p=[{value:"Param\xe8tres d&#39;URI",id:"param\xe8tres-duri",level:2},{value:"Param\xe8tres de route",id:"param\xe8tres-de-route",level:2}],u={toc:p},d="wrapper";function m(e){let{components:n,...t}=e;return(0,a.kt)(d,(0,r.Z)({},u,t,{components:n,mdxType:"MDXLayout"}),(0,a.kt)("h1",{id:"d\xe9finir-un-contr\xf4leur"},"D\xe9finir un contr\xf4leur"),(0,a.kt)("p",null,"Un contr\xf4leur prend la forme d'une proc\xe9dure stock\xe9e situ\xe9e dans le sch\xe9ma ",(0,a.kt)("em",{parentName:"p"},"web"),"."),(0,a.kt)("p",null,"Voici un exemple de contr\xf4leur basique :"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- #Route(\"/api/helloWorld\")\n-- #HttpGet(\"HelloWorld\")\nCREATE OR ALTER PROCEDURE [web].[p_hello_world] \nAS \nBEGIN\n    SELECT \n         'Salutation tout le beau monde !' AS [response_body] -- On retourne le corp de la r\xe9ponse\n        ,'text/plain' AS [response_content_type] -- Le type MIME du corp\n        ,200 AS [response_status] -- Le statut HTTP de la r\xe9ponse\n    ;\nEND\nGO\n")),(0,a.kt)("h2",{id:"param\xe8tres-duri"},"Param\xe8tres d'URI"),(0,a.kt)("p",null,"Un param\xe8tre d'URI est un argument fourni comme suit dans un URI : ",(0,a.kt)("inlineCode",{parentName:"p"},"/api/exemple?name=Corentin"),(0,a.kt)("br",null),"\nPour en r\xe9cup\xe9rer la valeur, il suffit de le prendre en argument de proc\xe9dure :"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- On d\xe9fini le param\xe8tre 'name' dans la route entre accolades\n-- #Route(\"/api/helloWorld\")\n-- #HttpGet(\"HelloWorld\")\nCREATE OR ALTER PROCEDURE [web].[p_hello_world] \n    @name NVARCHAR(100) = NULL -- On r\xe9cup\xe8re le param\xe8tre d'URI 'name'\nAS \nBEGIN\n    DECLARE @response_body NVARCHAR(MAX);\n    \n    IF (@name IS NULL)\n        SET @response_body = 'Salutation tout le beau monde !';\n    ELSE    \n        SELECT @response_body = 'Salutation ' + @name + ' !';\n\n    SELECT \n         @response_body AS [response_body]\n        ,'text/plain' AS [response_content_type]\n        ,200 AS [response_status]\n    ;\nEND\nGO\n")),(0,a.kt)("p",null,"Un param\xe8tre d'URI est toujours consid\xe9r\xe9 comme \xe9tant facultatif, et doit \xeatre de type NVARCHAR (charge \xe0 la proc\xe9dure de le caster si un autre type est attendu)."),(0,a.kt)("admonition",{type:"caution"},(0,a.kt)("p",{parentName:"admonition"},"Il est recommand\xe9 de d\xe9finir une valeur par d\xe9faut pour les param\xe8tres d'URI, ainsi que pour les param\xe8tres de route facultatifs dans les arguments de proc\xe9dure :\ns'il n'y en a pas et que le param\xe8tre n'est pas fourni, le SGBD ne pourra pas ex\xe9cuter la proc\xe9dure et remontera une erreur.")),(0,a.kt)("h2",{id:"param\xe8tres-de-route"},"Param\xe8tres de route"),(0,a.kt)("p",null,"Comme la plupart des ",(0,a.kt)("em",{parentName:"p"},"frameworks")," web, SQListe permet de d\xe9finir des mod\xe8les de route.\nCela permet de passer des arguments directement dans la route plut\xf4t que dans les param\xe8tres d'URI, ou encore dans le corp de la requ\xeate."),(0,a.kt)("p",null,"Exemple :"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- On d\xe9fini le param\xe8tre 'name' dans la route entre accolades\n-- #Route(\"/api/helloWorld/{name}\")\n-- #HttpGet(\"HelloWorld\")\nCREATE OR ALTER PROCEDURE [web].[p_hello_world] \n    @name NVARCHAR(100) -- On r\xe9cup\xe8re le param\xe8tre avec le m\xeame nom en argument de proc\xe9dure\nAS \nBEGIN\n    DECLARE @response_body NVARCHAR(MAX);\n    \n    SELECT @response_body = 'Salutation ' + @name + ' !';\n\n    SELECT \n         @response_body AS [response_body]\n        ,'text/plain' AS [response_content_type]\n        ,200 AS [response_status]\n    ;\nEND\nGO\n")),(0,a.kt)("admonition",{type:"note"},(0,a.kt)("p",{parentName:"admonition"},"Il est tout \xe0 fait possible de d\xe9finir plusieurs param\xe8tres dans la route, avec un nom diff\xe9rent."),(0,a.kt)("p",{parentName:"admonition"},"Comme pour les param\xe8tres d'URI, les param\xe8tres de route seront inject\xe9s avec le type NVARCHAR.")),(0,a.kt)("admonition",{type:"caution"},(0,a.kt)("p",{parentName:"admonition"},"Si un nom de param\xe8tre est identique \xe0 un nom de param\xe8tre standard, ce dernier prendra le dessus. ")),(0,a.kt)("p",null,"Dans notre exemple ci-dessus, le param\xe8tre est consid\xe9r\xe9 comme requis : s'il n'est pas fourni, SQListe ne fera pas la correspondance avec cette route.",(0,a.kt)("br",null),"\nPour d\xe9finir un param\xe8tre optionnel, nous pouvons faire comme suit :"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- On d\xe9fini le param\xe8tre 'name' dans la route entre accolades\n-- #Route(\"/api/helloWorld/{name?}\")\n-- #HttpGet(\"HelloWorld\")\nCREATE OR ALTER PROCEDURE [web].[p_hello_world] \n    @name NVARCHAR(100) = NULL -- On r\xe9cup\xe8re le param\xe8tre avec le m\xeame nom en argument de proc\xe9dure, en mettant une valeur NULL par d\xe9faut (utile si le param\xe8tre n'est pas fourni).\nAS \nBEGIN\n    DECLARE @response_body NVARCHAR(MAX);\n    \n    IF (@name IS NULL)\n        SET @response_body = 'Salutation tout le monde !';\n    ELSE    \n        SELECT @response_body = 'Salutation ' + @name + ' !';\n\n    SELECT \n         @response_body AS [response_body]\n        ,'text/plain' AS [response_content_type]\n        ,200 AS [response_status]\n    ;\nEND\nGO\n")),(0,a.kt)("p",null,"Un param\xe8tre facultatif ",(0,a.kt)("strong",{parentName:"p"},"doit")," figurer en fin de route.\nExemple : "),(0,a.kt)("ul",null,(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("inlineCode",{parentName:"li"},"/api/say/hello/{name?}/{age?}")," => Valide."),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("inlineCode",{parentName:"li"},"/api/say/hello/{name}/withAge/{age?}")," => Valide."),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("inlineCode",{parentName:"li"},"/api/say/hello/{name?}/withAge/{age?}")," => Invalide : le bon fonctionnement ne sera pas garanti.")),(0,a.kt)("p",null,"Lorsque l'on a plusieurs param\xe8tres facultatifs, il vaut souvent mieux utiliser des param\xe8tres d'URI, comme d\xe9crit pr\xe9c\xe9demment."),(0,a.kt)("admonition",{title:"Limitations",type:"warning"},(0,a.kt)("p",{parentName:"admonition"},"La taille d'un URI \xe9tant limit\xe9e \xe0 maximum 2048 caract\xe8res, des probl\xe8mes de lecture peuvent survenir en cas de param\xe8tres trop longs."),(0,a.kt)("p",{parentName:"admonition"},(0,a.kt)("em",{parentName:"p"},"Certains serveurs permettent d'augmenter cette limite, cependant il convient de v\xe9rifier que le client HTTP / navigateur prenne cette augmentation en charge."))))}m.isMDXComponent=!0}}]);