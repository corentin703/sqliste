"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[581],{9868:(t,e,a)=>{a.d(e,{Zo:()=>m,kt:()=>k});var r=a(6687);function n(t,e,a){return e in t?Object.defineProperty(t,e,{value:a,enumerable:!0,configurable:!0,writable:!0}):t[e]=a,t}function l(t,e){var a=Object.keys(t);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(t);e&&(r=r.filter((function(e){return Object.getOwnPropertyDescriptor(t,e).enumerable}))),a.push.apply(a,r)}return a}function p(t){for(var e=1;e<arguments.length;e++){var a=null!=arguments[e]?arguments[e]:{};e%2?l(Object(a),!0).forEach((function(e){n(t,e,a[e])})):Object.getOwnPropertyDescriptors?Object.defineProperties(t,Object.getOwnPropertyDescriptors(a)):l(Object(a)).forEach((function(e){Object.defineProperty(t,e,Object.getOwnPropertyDescriptor(a,e))}))}return t}function i(t,e){if(null==t)return{};var a,r,n=function(t,e){if(null==t)return{};var a,r,n={},l=Object.keys(t);for(r=0;r<l.length;r++)a=l[r],e.indexOf(a)>=0||(n[a]=t[a]);return n}(t,e);if(Object.getOwnPropertySymbols){var l=Object.getOwnPropertySymbols(t);for(r=0;r<l.length;r++)a=l[r],e.indexOf(a)>=0||Object.prototype.propertyIsEnumerable.call(t,a)&&(n[a]=t[a])}return n}var d=r.createContext({}),o=function(t){var e=r.useContext(d),a=e;return t&&(a="function"==typeof t?t(e):p(p({},e),t)),a},m=function(t){var e=o(t.components);return r.createElement(d.Provider,{value:e},t.children)},u="mdxType",s={inlineCode:"code",wrapper:function(t){var e=t.children;return r.createElement(r.Fragment,{},e)}},N=r.forwardRef((function(t,e){var a=t.components,n=t.mdxType,l=t.originalType,d=t.parentName,m=i(t,["components","mdxType","originalType","parentName"]),u=o(a),N=n,k=u["".concat(d,".").concat(N)]||u[N]||s[N]||l;return a?r.createElement(k,p(p({ref:e},m),{},{components:a})):r.createElement(k,p({ref:e},m))}));function k(t,e){var a=arguments,n=e&&e.mdxType;if("string"==typeof t||n){var l=a.length,p=new Array(l);p[0]=N;var i={};for(var d in e)hasOwnProperty.call(e,d)&&(i[d]=e[d]);i.originalType=t,i[u]="string"==typeof t?t:n,p[1]=i;for(var o=2;o<l;o++)p[o]=a[o];return r.createElement.apply(null,p)}return r.createElement.apply(null,a)}N.displayName="MDXCreateElement"},3663:(t,e,a)=>{a.r(e),a.d(e,{assets:()=>d,contentTitle:()=>p,default:()=>s,frontMatter:()=>l,metadata:()=>i,toc:()=>o});var r=a(8792),n=(a(6687),a(9868));const l={},p="Param\xe8tres standards",i={unversionedId:"index/standard-parameters",id:"index/standard-parameters",title:"Param\xe8tres standards",description:"Voici la liste exhaustive des param\xe8tres standards, leur direction (entr\xe9e / sortie), et \xe0 quoi ils servent.",source:"@site/docs/index/standard-parameters.md",sourceDirName:"index",slug:"/index/standard-parameters",permalink:"/sqliste/docs/index/standard-parameters",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/main/doc/docs/index/standard-parameters.md",tags:[],version:"current",frontMatter:{},sidebar:"docSidebar",previous:{title:"Index",permalink:"/sqliste/docs/category/index"},next:{title:"Tutorial - Basics",permalink:"/sqliste/docs/category/tutorial---basics"}},d={},o=[{value:"\xc0 propos la requ\xeate",id:"\xe0-propos-la-requ\xeate",level:2},{value:"\xc0 propos la r\xe9ponse",id:"\xe0-propos-la-r\xe9ponse",level:2},{value:"\xc0 propos du stockage",id:"\xe0-propos-du-stockage",level:2},{value:"Param\xe8tres sp\xe9cifiques aux intergiciels",id:"param\xe8tres-sp\xe9cifiques-aux-intergiciels",level:2}],m={toc:o},u="wrapper";function s(t){let{components:e,...a}=t;return(0,n.kt)(u,(0,r.Z)({},m,a,{components:e,mdxType:"MDXLayout"}),(0,n.kt)("h1",{id:"param\xe8tres-standards"},"Param\xe8tres standards"),(0,n.kt)("p",null,"Voici la liste exhaustive des param\xe8tres standards, leur direction (entr\xe9e / sortie), et \xe0 quoi ils servent."),(0,n.kt)("admonition",{type:"caution"},(0,n.kt)("p",{parentName:"admonition"},"Les param\xe8tres de sortie sont \xe0 retourner via un SELECT.",(0,n.kt)("br",null),"\nLes arguments de proc\xe9dure en mode sortie (",(0,n.kt)("em",{parentName:"p"},"OUTPUT"),") ne sont pas pris en charge.")),(0,n.kt)("h2",{id:"\xe0-propos-la-requ\xeate"},"\xc0 propos la requ\xeate"),(0,n.kt)("table",null,(0,n.kt)("thead",{parentName:"table"},(0,n.kt)("tr",{parentName:"thead"},(0,n.kt)("th",{parentName:"tr",align:null},"Nom"),(0,n.kt)("th",{parentName:"tr",align:null},"Direction"),(0,n.kt)("th",{parentName:"tr",align:null},"Type .NET"),(0,n.kt)("th",{parentName:"tr",align:null},"Format"),(0,n.kt)("th",{parentName:"tr",align:null},"Type SQL-Server"),(0,n.kt)("th",{parentName:"tr",align:null},"Description"))),(0,n.kt)("tbody",{parentName:"table"},(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"request_content_type")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("a",{parentName:"td",href:"https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types"},"Type MIME")),(0,n.kt)("td",{parentName:"tr",align:null},"VARCHAR(255)"),(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("a",{parentName:"td",href:"https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types"},"Type MIME")," du corp de la requ\xeate, provenant de l'en-t\xeate HTTP ",(0,n.kt)("em",{parentName:"td"},"Content-Type"),".")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"request_body")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"Brut"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Corp de la requ\xeate.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"request_headers")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"JSON"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"En-t\xeates de la requ\xeate, dictionnaire cl\xe9-valeur.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"request_cookies")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"JSON"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Cookie joint \xe0 la requ\xeate, dictionnaire cl\xe9-valeur.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"request_path")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"Brut"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Route de la requ\xeate.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"query_params")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"JSON"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Param\xe8tres de requ\xeate (pr\xe9sent apr\xe8s le ",(0,n.kt)("em",{parentName:"td"},"?")," dans l'URI), dictionnaire cl\xe9-valeur.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"path_params")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"JSON"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Param\xe8tres r\xe9solus depuis la route, dictionnaire cl\xe9-valeur.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"request_model")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"JSON"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Mod\xe8le ",(0,n.kt)("em",{parentName:"td"},"brut")," g\xe9r\xe9 par SQListe, contenant toutes les informations de la requ\xeate.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"error")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"JSON"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},".Mod\xe8le contenant des informations \xe0 propos de la derni\xe8re erreur survenue. Pour en savoir plus, cf. TODO.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"response_model")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"JSON"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Mod\xe8le ",(0,n.kt)("em",{parentName:"td"},"brut")," g\xe9r\xe9 par SQListe, contenant toutes les informations de la r\xe9ponse en l'\xe9tat actuel.")))),(0,n.kt)("h2",{id:"\xe0-propos-la-r\xe9ponse"},"\xc0 propos la r\xe9ponse"),(0,n.kt)("p",null,"Ces param\xe8tres peuvent-\xeatre retourn\xe9 depuis une proc\xe9dure via un SELECT, et viendront alt\xe9rer l'\xe9tat de la r\xe9ponse."),(0,n.kt)("table",null,(0,n.kt)("thead",{parentName:"table"},(0,n.kt)("tr",{parentName:"thead"},(0,n.kt)("th",{parentName:"tr",align:null},"Nom"),(0,n.kt)("th",{parentName:"tr",align:null},"Direction"),(0,n.kt)("th",{parentName:"tr",align:null},"Type .NET"),(0,n.kt)("th",{parentName:"tr",align:null},"Format"),(0,n.kt)("th",{parentName:"tr",align:null},"Type SQL-Server"),(0,n.kt)("th",{parentName:"tr",align:null},"Description"))),(0,n.kt)("tbody",{parentName:"table"},(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"response_content_type")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e / Sortie"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("a",{parentName:"td",href:"https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types"},"Type MIME")),(0,n.kt)("td",{parentName:"tr",align:null},"VARCHAR(255)"),(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("a",{parentName:"td",href:"https://developer.mozilla.org/fr/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types"},"Type MIME")," du corp de la r\xe9ponse, qui sera assign\xe9e \xe0 l'en-t\xeate HTTP ",(0,n.kt)("em",{parentName:"td"},"Content-Type")," si non d\xe9finie.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"response_body")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e / Sortie"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"Brut"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Corp de la r\xe9ponse.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"response_headers")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e / Sortie"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"JSON"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"En-t\xeates de la r\xe9ponse, dictionnaire cl\xe9-valeur.")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"response_cookies")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e / Sortie"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},"JSON"),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Cookie joint \xe0 la r\xe9ponse. Pour en savoir plus sur le format : cf. section... TODO")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"response_status")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e / Sortie"),(0,n.kt)("td",{parentName:"tr",align:null},"int"),(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("a",{parentName:"td",href:"https://developer.mozilla.org/fr/docs/Web/HTTP/Status"},"Statut HTTP")),(0,n.kt)("td",{parentName:"tr",align:null},"INT"),(0,n.kt)("td",{parentName:"tr",align:null},"Code de statut de la r\xe9ponse.")))),(0,n.kt)("admonition",{type:"tip"},(0,n.kt)("p",{parentName:"admonition"},"Ces param\xe8tres peuvent aussi \xeatre pris en argument de proc\xe9dure. ",(0,n.kt)("br",null),"\nCela permet de r\xe9cup\xe9rer l'\xe9tat actuel de la r\xe9ponse, ce qui peut \xeatre utile apr\xe8s un traitement par intergiciel.")),(0,n.kt)("h2",{id:"\xe0-propos-du-stockage"},"\xc0 propos du stockage"),(0,n.kt)("p",null,"Ces param\xe8tres peuvent-\xeatre pris en arguments de proc\xe9dure, ainsi que retourn\xe9 pour alt\xe9rer l'\xe9tat."),(0,n.kt)("table",null,(0,n.kt)("thead",{parentName:"table"},(0,n.kt)("tr",{parentName:"thead"},(0,n.kt)("th",{parentName:"tr",align:null},"Nom"),(0,n.kt)("th",{parentName:"tr",align:null},"Direction"),(0,n.kt)("th",{parentName:"tr",align:null},"Type .NET"),(0,n.kt)("th",{parentName:"tr",align:null},"Format"),(0,n.kt)("th",{parentName:"tr",align:null},"Type SQL-Server"),(0,n.kt)("th",{parentName:"tr",align:null},"Description"))),(0,n.kt)("tbody",{parentName:"table"},(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"request_storage")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e / Sortie"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"JSON*")),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Stockage ayant une dur\xe9e de vie d'une requ\xeate. Il est initialis\xe9 \xe0 un objet JSON vide au d\xe9but d'une requ\xeate ('{}').")),(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"session")),(0,n.kt)("td",{parentName:"tr",align:null},"Entr\xe9e / Sortie"),(0,n.kt)("td",{parentName:"tr",align:null},"string"),(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"JSON*")),(0,n.kt)("td",{parentName:"tr",align:null},"NVARCHAR(MAX)"),(0,n.kt)("td",{parentName:"tr",align:null},"Acc\xe8s \xe0 la session HTTP. Pour en savoir plus : TODO.")))),(0,n.kt)("admonition",{type:"info"},(0,n.kt)("p",{parentName:"admonition"},(0,n.kt)("em",{parentName:"p"},"JSON*")," : Bien que le contenu soit initialis\xe9 avec un JSON vide ('{}') par d\xe9faut, il peut n\xe9anmoins \xeatre chang\xe9 de\nmani\xe8re s\xfbre \xe9tant donn\xe9 que ce contenu ne fait pas l'objet d'un traitement de la part de SQListe.")),(0,n.kt)("h2",{id:"param\xe8tres-sp\xe9cifiques-aux-intergiciels"},"Param\xe8tres sp\xe9cifiques aux intergiciels"),(0,n.kt)("p",null,"Ces param\xe8tres peuvent-\xeatre pris en arguments de proc\xe9dure, ainsi que retourn\xe9 pour alt\xe9rer l'\xe9tat."),(0,n.kt)("table",null,(0,n.kt)("thead",{parentName:"table"},(0,n.kt)("tr",{parentName:"thead"},(0,n.kt)("th",{parentName:"tr",align:null},"Nom"),(0,n.kt)("th",{parentName:"tr",align:null},"Direction"),(0,n.kt)("th",{parentName:"tr",align:null},"Type .NET"),(0,n.kt)("th",{parentName:"tr",align:null},"Format"),(0,n.kt)("th",{parentName:"tr",align:null},"Type SQL-Server"),(0,n.kt)("th",{parentName:"tr",align:null},"Description"))),(0,n.kt)("tbody",{parentName:"table"},(0,n.kt)("tr",{parentName:"tbody"},(0,n.kt)("td",{parentName:"tr",align:null},(0,n.kt)("em",{parentName:"td"},"next")),(0,n.kt)("td",{parentName:"tr",align:null},"Sortie"),(0,n.kt)("td",{parentName:"tr",align:null},"bool"),(0,n.kt)("td",{parentName:"tr",align:null}),(0,n.kt)("td",{parentName:"tr",align:null},"BIT"),(0,n.kt)("td",{parentName:"tr",align:null},"Si retourn\xe9 \xe0 0, interromp la ",(0,n.kt)("em",{parentName:"td"},"pipeline")," de requ\xeate. Est \xe9gal \xe0 1 par d\xe9faut.")))))}s.isMDXComponent=!0}}]);