"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[3880],{9868:(e,t,n)=>{n.d(t,{Zo:()=>u,kt:()=>f});var r=n(6687);function s(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function i(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function a(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?i(Object(n),!0).forEach((function(t){s(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):i(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function o(e,t){if(null==e)return{};var n,r,s=function(e,t){if(null==e)return{};var n,r,s={},i=Object.keys(e);for(r=0;r<i.length;r++)n=i[r],t.indexOf(n)>=0||(s[n]=e[n]);return s}(e,t);if(Object.getOwnPropertySymbols){var i=Object.getOwnPropertySymbols(e);for(r=0;r<i.length;r++)n=i[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(s[n]=e[n])}return s}var l=r.createContext({}),p=function(e){var t=r.useContext(l),n=t;return e&&(n="function"==typeof e?e(t):a(a({},t),e)),n},u=function(e){var t=p(e.components);return r.createElement(l.Provider,{value:t},e.children)},c="mdxType",d={inlineCode:"code",wrapper:function(e){var t=e.children;return r.createElement(r.Fragment,{},t)}},m=r.forwardRef((function(e,t){var n=e.components,s=e.mdxType,i=e.originalType,l=e.parentName,u=o(e,["components","mdxType","originalType","parentName"]),c=p(n),m=s,f=c["".concat(l,".").concat(m)]||c[m]||d[m]||i;return n?r.createElement(f,a(a({ref:t},u),{},{components:n})):r.createElement(f,a({ref:t},u))}));function f(e,t){var n=arguments,s=t&&t.mdxType;if("string"==typeof e||s){var i=n.length,a=new Array(i);a[0]=m;var o={};for(var l in t)hasOwnProperty.call(t,l)&&(o[l]=t[l]);o.originalType=e,o[c]="string"==typeof e?e:s,a[1]=o;for(var p=2;p<i;p++)a[p]=n[p];return r.createElement.apply(null,a)}return r.createElement.apply(null,n)}m.displayName="MDXCreateElement"},515:(e,t,n)=>{n.r(t),n.d(t,{assets:()=>l,contentTitle:()=>a,default:()=>d,frontMatter:()=>i,metadata:()=>o,toc:()=>p});var r=n(8792),s=(n(6687),n(9868));const i={sidebar_position:5},a="Stockage",o={unversionedId:"intregrations/sql-server/storage",id:"intregrations/sql-server/storage",title:"Stockage",description:"Afin de simplifier le d\xe9veloppement, SQListe propose un acc\xe8s \xe0 deux emplacements permettant de stocker des donn\xe9es pour une utilisation ult\xe9rieure.",source:"@site/docs/intregrations/sql-server/storage.md",sourceDirName:"intregrations/sql-server",slug:"/intregrations/sql-server/storage",permalink:"/docs/intregrations/sql-server/storage",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/doc/docs/intregrations/sql-server/storage.md",tags:[],version:"current",sidebarPosition:5,frontMatter:{sidebar_position:5},sidebar:"docSidebar",previous:{title:"D\xe9finir un intergiciel",permalink:"/docs/intregrations/sql-server/define-middleware"},next:{title:"Op\xe9rations sur HTTP",permalink:"/docs/category/op\xe9rations-sur-http"}},l={},p=[{value:"Session",id:"session",level:2},{value:"Stockage du <em>pipeline</em>",id:"stockage-du-pipeline",level:2}],u={toc:p},c="wrapper";function d(e){let{components:t,...n}=e;return(0,s.kt)(c,(0,r.Z)({},u,n,{components:t,mdxType:"MDXLayout"}),(0,s.kt)("h1",{id:"stockage"},"Stockage"),(0,s.kt)("p",null,"Afin de simplifier le d\xe9veloppement, SQListe propose un acc\xe8s \xe0 deux emplacements permettant de stocker des donn\xe9es pour une utilisation ult\xe9rieure."),(0,s.kt)("p",null,"Le format recommand\xe9 pour ces deux emplacements est le JSON. Ceux-ci sont initialis\xe9s avec un JSON vide ",(0,s.kt)("inlineCode",{parentName:"p"},"{}")," et font partie int\xe9grante de l'\xe9tat de la r\xe9ponse."),(0,s.kt)("admonition",{type:"note"},(0,s.kt)("p",{parentName:"admonition"},"Le contenu de ces stockages est seulement initialis\xe9 par SQListe.",(0,s.kt)("br",null),"\n\xc9tant donn\xe9 qu'ils ne font l'objet d'aucun traitement particulier, vous pouvez tout \xe0 fait d\xe9finir un autre format en \xe9crasant la valuer par d\xe9faut.")),(0,s.kt)("admonition",{type:"info"},(0,s.kt)("p",{parentName:"admonition"},"Le client n'a pas acc\xe8s au contenu de ces emplacements de stockage : ils restent interne \xe0 SQListe.",(0,s.kt)("br",null),"\nVous pouvez donc y stocker des donn\xe9es sensibles sereinement.  ")),(0,s.kt)("h2",{id:"session"},"Session"),(0,s.kt)("p",null,"Le premier d'entre-eux donne acc\xe8s \xe0 la session HTTP.",(0,s.kt)("br",null),"\nLa session est un emplacement de stockage g\xe9n\xe9ralement r\xe9solu \xe0 partir d'un jeton enregistr\xe9 dans un cookie HTTP (ce qui est notre cas).\nSa dur\xe9e de vie est de 20 minutes par d\xe9faut (configurable dans le fichier ",(0,s.kt)("em",{parentName:"p"},"appsettings.json"),")."),(0,s.kt)("p",null,"Exemple d'utilisation :"),(0,s.kt)("pre",null,(0,s.kt)("code",{parentName:"pre",className:"language-sql"},"#Route(\"/api/exemple/session\")\n#HttpGet(\"ExempleSession\")\nCREATE OR ALTER PROCEDURE [web].[p_exemple_session]\n    @session NVARCHAR(MAX) \nAS \nBEGIN\n    IF (JSON_VALUE(@session, '$.calculSavant' IS NULL))\n    BEGIN\n        DECLARE @calcul_savant INT;\n        SET @calcul_savant = 1 + 1;\n    \n        SET @session = JSON_MODIFY(@session, '$.calculSavant', @calcul_savant);\n    END\n    \n    -- Autres traitements...\n\n    SELECT \n         @session AS [session]\n    ;\nEND\nGO\n")),(0,s.kt)("p",null,"Le calcul tr\xe8s savant et co\xfbteux fait dans l'exemple ci-dessus ne sera ex\xe9cut\xe9 que si le r\xe9sultat n'est pas pr\xe9sent dans la session."),(0,s.kt)("admonition",{type:"info"},(0,s.kt)("p",{parentName:"admonition"},"La session HTTP n'est r\xe9ellement alt\xe9r\xe9e qu'\xe0 la fin du traitement du ",(0,s.kt)("em",{parentName:"p"},"pipeline"),".")),(0,s.kt)("admonition",{title:"Initialisation",type:"note"},(0,s.kt)("p",{parentName:"admonition"},"La session (et le cookie correspondant) ne sera initialis\xe9e que si elle est demand\xe9e par une proc\xe9dure.")),(0,s.kt)("h2",{id:"stockage-du-pipeline"},"Stockage du ",(0,s.kt)("em",{parentName:"h2"},"pipeline")),(0,s.kt)("p",null,"Afin de transmettre des informations au sein d'un ",(0,s.kt)("em",{parentName:"p"},"pipeline"),", SQListe propose un stockage ayant une dur\xe9e de vie d'une requ\xeate."),(0,s.kt)("p",null,"Exemple d'utilisation :"),(0,s.kt)("pre",null,(0,s.kt)("code",{parentName:"pre",className:"language-sql"},"-- #Middleware(Order = 1, After = false)\nCREATE OR ALTER PROCEDURE [web].[p_middleware_log_request_start]\n    @request_headers NVARCHAR(MAX),\n    @pipeline_storage NVARCHAR(MAX) \nAS\nBEGIN \n    DECLARE @username NVARCHAR(50);\n    \n    -- Lecture du jetton, v\xe9rification et r\xe9cup\xe9ration de l'utilisateur...\n    \n    SET @pipeline_storage = JSON_MODIFY(@pipeline_storage, '$.username', @username);\n\n    SELECT \n         @pipeline_storage AS [pipeline_storage]\n    ;\nEND\nGO\n")),(0,s.kt)("p",null,"Dans cet exemple, nous r\xe9cup\xe9rons le nom de l'utilisateur et l'ins\xe9rons dans le stockage du ",(0,s.kt)("em",{parentName:"p"},"pipeline"),".",(0,s.kt)("br",null),"\nCela permet \xe0 un intergiciel ult\xe9rieur, ou \xe0 un contr\xf4leur d'acc\xe9der \xe0 cette donn\xe9e sans r\xe9aliser de traitement suppl\xe9mentaire."),(0,s.kt)("admonition",{title:"Utilisation",type:"note"},(0,s.kt)("p",{parentName:"admonition"},"Ces deux stockages s'utilisent de la m\xeame mani\xe8re, la seule diff\xe9rence \xe9tant la dur\xe9e de vie.",(0,s.kt)("br",null),"\nPar cons\xe9quent, la session sera plus appropri\xe9e pour stocker des donn\xe9es co\xfbteuses \xe0 calculer / mise en cache, tandis que le stockage du ",(0,s.kt)("em",{parentName:"p"},"pipeline"),"\nsera plus destin\xe9 \xe0 l'enregistrement de donn\xe9es changeant souvent, et dont l'obtention est rapide.")))}d.isMDXComponent=!0}}]);