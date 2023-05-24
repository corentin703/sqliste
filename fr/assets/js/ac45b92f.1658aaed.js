"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[2646],{9868:(e,t,r)=>{r.d(t,{Zo:()=>u,kt:()=>f});var n=r(6687);function s(e,t,r){return t in e?Object.defineProperty(e,t,{value:r,enumerable:!0,configurable:!0,writable:!0}):e[t]=r,e}function a(e,t){var r=Object.keys(e);if(Object.getOwnPropertySymbols){var n=Object.getOwnPropertySymbols(e);t&&(n=n.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),r.push.apply(r,n)}return r}function o(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{};t%2?a(Object(r),!0).forEach((function(t){s(e,t,r[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(r)):a(Object(r)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(r,t))}))}return e}function i(e,t){if(null==e)return{};var r,n,s=function(e,t){if(null==e)return{};var r,n,s={},a=Object.keys(e);for(n=0;n<a.length;n++)r=a[n],t.indexOf(r)>=0||(s[r]=e[r]);return s}(e,t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);for(n=0;n<a.length;n++)r=a[n],t.indexOf(r)>=0||Object.prototype.propertyIsEnumerable.call(e,r)&&(s[r]=e[r])}return s}var c=n.createContext({}),l=function(e){var t=n.useContext(c),r=t;return e&&(r="function"==typeof e?e(t):o(o({},t),e)),r},u=function(e){var t=l(e.components);return n.createElement(c.Provider,{value:t},e.children)},p="mdxType",d={inlineCode:"code",wrapper:function(e){var t=e.children;return n.createElement(n.Fragment,{},t)}},m=n.forwardRef((function(e,t){var r=e.components,s=e.mdxType,a=e.originalType,c=e.parentName,u=i(e,["components","mdxType","originalType","parentName"]),p=l(r),m=s,f=p["".concat(c,".").concat(m)]||p[m]||d[m]||a;return r?n.createElement(f,o(o({ref:t},u),{},{components:r})):n.createElement(f,o({ref:t},u))}));function f(e,t){var r=arguments,s=t&&t.mdxType;if("string"==typeof e||s){var a=r.length,o=new Array(a);o[0]=m;var i={};for(var c in t)hasOwnProperty.call(t,c)&&(i[c]=t[c]);i.originalType=e,i[p]="string"==typeof e?e:s,o[1]=i;for(var l=2;l<a;l++)o[l]=r[l];return n.createElement.apply(null,o)}return n.createElement.apply(null,r)}m.displayName="MDXCreateElement"},6973:(e,t,r)=>{r.r(t),r.d(t,{assets:()=>c,contentTitle:()=>o,default:()=>d,frontMatter:()=>a,metadata:()=>i,toc:()=>l});var n=r(8792),s=(r(6687),r(9868));const a={sidebar_position:2},o="Structure",i={unversionedId:"intregrations/sql-server/structure",id:"intregrations/sql-server/structure",title:"Structure",description:"Sch\xe9mas",source:"@site/i18n/fr/docusaurus-plugin-content-docs/current/intregrations/sql-server/structure.md",sourceDirName:"intregrations/sql-server",slug:"/intregrations/sql-server/structure",permalink:"/sqliste/fr/docs/intregrations/sql-server/structure",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/main/doc/docs/intregrations/sql-server/structure.md",tags:[],version:"current",sidebarPosition:2,frontMatter:{sidebar_position:2},sidebar:"docSidebar",previous:{title:"Pr\xe9requis",permalink:"/sqliste/fr/docs/intregrations/sql-server/requirements"},next:{title:"D\xe9finir un contr\xf4leur",permalink:"/sqliste/fr/docs/intregrations/sql-server/define-controller"}},c={},l=[{value:"Sch\xe9mas",id:"sch\xe9mas",level:2},{value:"Sch\xe9ma <em>sqliste</em>",id:"sch\xe9ma-sqliste",level:3},{value:"Sch\xe9ma <em>web</em>",id:"sch\xe9ma-web",level:3},{value:"Sch\xe9ma <em>dbo</em> / sch\xe9ma par d\xe9faut",id:"sch\xe9ma-dbo--sch\xe9ma-par-d\xe9faut",level:3}],u={toc:l},p="wrapper";function d(e){let{components:t,...r}=e;return(0,s.kt)(p,(0,n.Z)({},u,r,{components:t,mdxType:"MDXLayout"}),(0,s.kt)("h1",{id:"structure"},"Structure"),(0,s.kt)("h2",{id:"sch\xe9mas"},"Sch\xe9mas"),(0,s.kt)("p",null,"La base de donn\xe9e connect\xe9e se retrouve organis\xe9e sur 3 sch\xe9mas apr\xe8s ex\xe9cution de la migration."),(0,s.kt)("h3",{id:"sch\xe9ma-sqliste"},"Sch\xe9ma ",(0,s.kt)("em",{parentName:"h3"},"sqliste")),(0,s.kt)("p",null,"Ce sch\xe9ma contient toutes les proc\xe9dures n\xe9cessaires au bon fonctionnement de SQListe."),(0,s.kt)("p",null,"On y retrouve les proc\xe9dures permettant l'introspection de la base de donn\xe9es, ainsi que la g\xe9n\xe9ration et l'obtention du JSON OpenAPI."),(0,s.kt)("admonition",{type:"info"},(0,s.kt)("p",{parentName:"admonition"},"Le bon fonctionnement de l'applicatif ne saurait-\xeatre garanti en cas de modification d'une de ces proc\xe9dures sans consultation pr\xe9alable de la documentation.")),(0,s.kt)("h3",{id:"sch\xe9ma-web"},"Sch\xe9ma ",(0,s.kt)("em",{parentName:"h3"},"web")),(0,s.kt)("p",null,"Ce sch\xe9ma est destin\xe9 \xe0 accueillir les proc\xe9dures d\xe9finissant des contr\xf4leurs et des middlewares.   "),(0,s.kt)("admonition",{type:"caution"},(0,s.kt)("p",{parentName:"admonition"},"Tout objet pr\xe9sent dans ce sch\xe9ma peut potentiellement \xeatre expos\xe9 via l'API web : n'y mettez pas vos objets m\xe9tiers et autres proc\xe9dures sensibles.")),(0,s.kt)("h3",{id:"sch\xe9ma-dbo--sch\xe9ma-par-d\xe9faut"},"Sch\xe9ma ",(0,s.kt)("em",{parentName:"h3"},"dbo")," / sch\xe9ma par d\xe9faut"),(0,s.kt)("p",null,"Le sch\xe9ma par d\xe9faut n'est pas utilis\xe9 par SQListe : vous pouvez donc y d\xe9velopper sereinement en ayant la\ngarantie que rien ne pourra \xeatre appel\xe9 depuis le web sans passer par une proc\xe9dure du sch\xe9ma ",(0,s.kt)("em",{parentName:"p"},"web"),"."))}d.isMDXComponent=!0}}]);