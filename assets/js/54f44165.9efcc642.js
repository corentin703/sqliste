"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[152],{9868:(e,t,n)=>{n.d(t,{Zo:()=>c,kt:()=>f});var r=n(6687);function i(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function a(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function o(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?a(Object(n),!0).forEach((function(t){i(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):a(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function s(e,t){if(null==e)return{};var n,r,i=function(e,t){if(null==e)return{};var n,r,i={},a=Object.keys(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||(i[n]=e[n]);return i}(e,t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(i[n]=e[n])}return i}var l=r.createContext({}),u=function(e){var t=r.useContext(l),n=t;return e&&(n="function"==typeof e?e(t):o(o({},t),e)),n},c=function(e){var t=u(e.components);return r.createElement(l.Provider,{value:t},e.children)},p="mdxType",d={inlineCode:"code",wrapper:function(e){var t=e.children;return r.createElement(r.Fragment,{},t)}},m=r.forwardRef((function(e,t){var n=e.components,i=e.mdxType,a=e.originalType,l=e.parentName,c=s(e,["components","mdxType","originalType","parentName"]),p=u(n),m=i,f=p["".concat(l,".").concat(m)]||p[m]||d[m]||a;return n?r.createElement(f,o(o({ref:t},c),{},{components:n})):r.createElement(f,o({ref:t},c))}));function f(e,t){var n=arguments,i=t&&t.mdxType;if("string"==typeof e||i){var a=n.length,o=new Array(a);o[0]=m;var s={};for(var l in t)hasOwnProperty.call(t,l)&&(s[l]=t[l]);s.originalType=e,s[p]="string"==typeof e?e:i,o[1]=s;for(var u=2;u<a;u++)o[u]=n[u];return r.createElement.apply(null,o)}return r.createElement.apply(null,n)}m.displayName="MDXCreateElement"},8363:(e,t,n)=>{n.r(t),n.d(t,{assets:()=>l,contentTitle:()=>o,default:()=>d,frontMatter:()=>a,metadata:()=>s,toc:()=>u});var r=n(8792),i=(n(6687),n(9868));const a={sidebar_position:1},o="Installation",s={unversionedId:"getting-started/installation",id:"getting-started/installation",title:"Installation",description:"SQListe est une application ASP.NET Core se connectant \xe0 votre base de donn\xe9es,",source:"@site/docs/getting-started/installation.md",sourceDirName:"getting-started",slug:"/getting-started/installation",permalink:"/docs/getting-started/installation",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/doc/docs/getting-started/installation.md",tags:[],version:"current",sidebarPosition:1,frontMatter:{sidebar_position:1},sidebar:"docSidebar",previous:{title:"D\xe9marrage",permalink:"/docs/category/d\xe9marrage"},next:{title:"D\xe9ploiement",permalink:"/docs/getting-started/deployment"}},l={},u=[{value:"Configuration",id:"configuration",level:2},{value:"Journalisation",id:"journalisation",level:3},{value:"Planificateur de t\xe2ches",id:"planificateur-de-t\xe2ches",level:3},{value:"Session",id:"session",level:3},{value:"Base de donn\xe9es",id:"base-de-donn\xe9es",level:3}],c={toc:u},p="wrapper";function d(e){let{components:t,...n}=e;return(0,i.kt)(p,(0,r.Z)({},c,n,{components:t,mdxType:"MDXLayout"}),(0,i.kt)("h1",{id:"installation"},"Installation"),(0,i.kt)("p",null,"SQListe est une application ASP.NET Core se connectant \xe0 votre base de donn\xe9es,\net exposant ses proc\xe9dures stock\xe9es \xe0 travers une API web de type ",(0,i.kt)("a",{parentName:"p",href:"https://fr.wikipedia.org/wiki/Representational_state_transfer"},"REST"),"."),(0,i.kt)("h2",{id:"configuration"},"Configuration"),(0,i.kt)("p",null,"La configuration se fait depuis le fichier ",(0,i.kt)("em",{parentName:"p"},"appsettings.json"),", situ\xe9 dans le m\xeame r\xe9pertoire que l'ex\xe9cutable.\nCelui-ci prend la forme suivante."),(0,i.kt)("pre",null,(0,i.kt)("code",{parentName:"pre",className:"language-json",metastring:"lines",lines:!0},'{\n  "Serilog": {\n    "MinimumLevel": {\n      "Default": "Information",\n      "Override": {\n        "Microsoft": "Warning",\n        "Microsoft.Hosting.Lifetime": "Information",\n        "Microsoft.AspNetCore": "Warning"\n      }\n    },\n    "Enrich": [\n      "FromLogContext",\n      "WithMachineName"\n    ],\n    "Properties": {\n      "ApplicationName": "SQListe"\n    }\n    "WriteTo": [\n      {\n        "Name": "File",\n        "Args": { "path":  "./logs/log-.txt", "rollingInterval": "Day" }\n      }\n    ]\n  },\n  "AllowedHosts": "*",\n  "AllowedOrigins": ["*"],\n  "Coravel": {\n    "Queue": {\n      "ConsummationDelay": 1\n    }\n  },\n  "Session": {\n    "IdleTimeout": 1440 // Session valable durant 24h\n  },\n  "Database": {\n    "ConnectionString": "Data Source=MonServeurSQL; Database=MaBDD; User ID=MonUtilisateur; Password=MotDePasseSuperFort!; App=SQListe; TrustServerCertificate=true;",\n    "Migration": {\n      "Enable": true\n    }\n  }\n}\n')),(0,i.kt)("p",null,"Nous y retrouvons 3 sections :"),(0,i.kt)("h3",{id:"journalisation"},"Journalisation"),(0,i.kt)("p",null,"SQListe repose sur ",(0,i.kt)("a",{parentName:"p",href:"https://serilog.net/"},"Serilog"),".",(0,i.kt)("br",null),"\nCette partie du fichier de configuration permet de d\xe9finir notamment le niveau de logs souhait\xe9,\nainsi que l'emplacement des fichiers de logs si souhait\xe9."),(0,i.kt)("p",null,"Pour en savoir plus sur la configuration de Serilog, vous pouvez consulter la ",(0,i.kt)("a",{parentName:"p",href:"https://github.com/serilog/serilog-settings-configuration"},"doc associ\xe9e"),"."),(0,i.kt)("h3",{id:"planificateur-de-t\xe2ches"},"Planificateur de t\xe2ches"),(0,i.kt)("p",null,"Cette section correspond aux param\xe8tres du planificateur de t\xe2che (SQListe utilise ",(0,i.kt)("a",{parentName:"p",href:"https://docs.coravel.net/"},"Coravel"),").",(0,i.kt)("br",null),"\nLe param\xe8tre ",(0,i.kt)("em",{parentName:"p"},"ConsummationDelay")," d\xe9finit le d\xe9lai de traitement lorsqu'une nouvelle t\xe2che a \xe9t\xe9 ajout\xe9 \xe0 la file d'attente. "),(0,i.kt)("h3",{id:"session"},"Session"),(0,i.kt)("p",null,"Donne un acc\xe8s \xe0 certains param\xe8tres de la session, notamment sa dur\xe9e de vie (exprim\xe9e en minutes, 20 par d\xe9faut)."),(0,i.kt)("h3",{id:"base-de-donn\xe9es"},"Base de donn\xe9es"),(0,i.kt)("p",null,"Cette section peut varier selon le SGBD que vous utilisez (cf. la section correspondante).",(0,i.kt)("br",null),"\nCependant, nous retrouverons toujours le type de SGBD, la cha\xeene de connexion \xe0 celui-ci, ainsi que les param\xe8tres de migration."))}d.isMDXComponent=!0}}]);