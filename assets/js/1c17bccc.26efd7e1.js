"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[3880],{9868:(e,t,n)=>{n.d(t,{Zo:()=>c,kt:()=>m});var r=n(6687);function i(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function o(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function a(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?o(Object(n),!0).forEach((function(t){i(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):o(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function s(e,t){if(null==e)return{};var n,r,i=function(e,t){if(null==e)return{};var n,r,i={},o=Object.keys(e);for(r=0;r<o.length;r++)n=o[r],t.indexOf(n)>=0||(i[n]=e[n]);return i}(e,t);if(Object.getOwnPropertySymbols){var o=Object.getOwnPropertySymbols(e);for(r=0;r<o.length;r++)n=o[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(i[n]=e[n])}return i}var l=r.createContext({}),p=function(e){var t=r.useContext(l),n=t;return e&&(n="function"==typeof e?e(t):a(a({},t),e)),n},c=function(e){var t=p(e.components);return r.createElement(l.Provider,{value:t},e.children)},d="mdxType",u={inlineCode:"code",wrapper:function(e){var t=e.children;return r.createElement(r.Fragment,{},t)}},f=r.forwardRef((function(e,t){var n=e.components,i=e.mdxType,o=e.originalType,l=e.parentName,c=s(e,["components","mdxType","originalType","parentName"]),d=p(n),f=i,m=d["".concat(l,".").concat(f)]||d[f]||u[f]||o;return n?r.createElement(m,a(a({ref:t},c),{},{components:n})):r.createElement(m,a({ref:t},c))}));function m(e,t){var n=arguments,i=t&&t.mdxType;if("string"==typeof e||i){var o=n.length,a=new Array(o);a[0]=f;var s={};for(var l in t)hasOwnProperty.call(t,l)&&(s[l]=t[l]);s.originalType=e,s[d]="string"==typeof e?e:i,a[1]=s;for(var p=2;p<o;p++)a[p]=n[p];return r.createElement.apply(null,a)}return r.createElement.apply(null,n)}f.displayName="MDXCreateElement"},515:(e,t,n)=>{n.r(t),n.d(t,{assets:()=>l,contentTitle:()=>a,default:()=>u,frontMatter:()=>o,metadata:()=>s,toc:()=>p});var r=n(8792),i=(n(6687),n(9868));const o={sidebar_position:5},a="Storage",s={unversionedId:"intregrations/sql-server/storage",id:"intregrations/sql-server/storage",title:"Storage",description:"To simplify development, SQListe provides access to two storage locations for storing data for later use.",source:"@site/docs/intregrations/sql-server/storage.md",sourceDirName:"intregrations/sql-server",slug:"/intregrations/sql-server/storage",permalink:"/sqliste/docs/intregrations/sql-server/storage",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/main/doc/docs/intregrations/sql-server/storage.md",tags:[],version:"current",sidebarPosition:5,frontMatter:{sidebar_position:5},sidebar:"docSidebar",previous:{title:"Define a Middleware",permalink:"/sqliste/docs/intregrations/sql-server/define-middleware"},next:{title:"HTTP operations",permalink:"/sqliste/docs/category/http-operations"}},l={},p=[{value:"Session",id:"session",level:2},{value:"Pipeline Storage",id:"pipeline-storage",level:2}],c={toc:p},d="wrapper";function u(e){let{components:t,...n}=e;return(0,i.kt)(d,(0,r.Z)({},c,n,{components:t,mdxType:"MDXLayout"}),(0,i.kt)("h1",{id:"storage"},"Storage"),(0,i.kt)("p",null,"To simplify development, SQListe provides access to two storage locations for storing data for later use."),(0,i.kt)("p",null,"The recommended format for these storage locations is JSON. They are initialized with an empty JSON object ",(0,i.kt)("inlineCode",{parentName:"p"},"{}")," and are an integral part of the response state."),(0,i.kt)("admonition",{type:"note"},(0,i.kt)("p",{parentName:"admonition"},"The content of these storages is only initialized by SQListe.",(0,i.kt)("br",null),"\nSince they are not subject to any specific processing, you can define a different format by overwriting the default value.")),(0,i.kt)("admonition",{type:"info"},(0,i.kt)("p",{parentName:"admonition"},"The client does not have access to the content of these storage locations; they remain internal to SQListe.",(0,i.kt)("br",null),"\nTherefore, you can safely store sensitive data in them.")),(0,i.kt)("h2",{id:"session"},"Session"),(0,i.kt)("p",null,"The first storage location provides access to the HTTP session.",(0,i.kt)("br",null),"\nThe session is a storage location typically resolved from a token stored in an HTTP cookie (which is our case).\nIts lifespan is 20 minutes by default (configurable in the ",(0,i.kt)("em",{parentName:"p"},"appsettings.json")," file)."),(0,i.kt)("p",null,"Example usage:"),(0,i.kt)("pre",null,(0,i.kt)("code",{parentName:"pre",className:"language-sql"},"#Route(\"/api/example/session\")\n#HttpGet(\"ExampleSession\")\nCREATE OR ALTER PROCEDURE [web].[p_example_session]\n    @session NVARCHAR(MAX) \nAS \nBEGIN\n    IF (JSON_VALUE(@session, '$.fancyCalculation' IS NULL))\n    BEGIN\n        DECLARE @fancy_calculation INT;\n        SET @fancy_calculation = 1 + 1;\n    \n        SET @session = JSON_MODIFY(@session, '$.fancyCalculation', @fancy_calculation);\n    END\n    \n    -- Other processing...\n\n    SELECT \n         @session AS [session]\n    ;\nEND\nGO\n")),(0,i.kt)("p",null,"The very fancy and expensive calculation in the example above will only be executed if the result is not present in the session."),(0,i.kt)("admonition",{type:"info"},(0,i.kt)("p",{parentName:"admonition"},"The HTTP session is only modified at the end of the pipeline processing.")),(0,i.kt)("admonition",{title:"Initialization",type:"note"},(0,i.kt)("p",{parentName:"admonition"},"The session (and the corresponding cookie) will only be initialized if it is requested by a procedure.")),(0,i.kt)("h2",{id:"pipeline-storage"},"Pipeline Storage"),(0,i.kt)("p",null,"To transmit information within a pipeline, SQListe provides a storage with a lifespan of one request."),(0,i.kt)("p",null,"Example usage:"),(0,i.kt)("pre",null,(0,i.kt)("code",{parentName:"pre",className:"language-sql"},"-- #Middleware(Order = 1, After = false)\nCREATE OR ALTER PROCEDURE [web].[p_middleware_log_request_start]\n    @request_headers NVARCHAR(MAX),\n    @pipeline_storage NVARCHAR(MAX) \nAS\nBEGIN \n    DECLARE @username NVARCHAR(50);\n    \n    -- Read the token, verify and retrieve the user...\n    \n    SET @pipeline_storage = JSON_MODIFY(@pipeline_storage, '$.username', @username);\n\n    SELECT \n         @pipeline_storage AS [pipeline_storage]\n    ;\nEND\nGO\n")),(0,i.kt)("p",null,"In this example, we retrieve the username and insert it into the pipeline storage.",(0,i.kt)("br",null),"\nThis allows a subsequent middleware or controller to access this data without performing additional processing."),(0,i.kt)("admonition",{title:"Usage",type:"note"},(0,i.kt)("p",{parentName:"admonition"},"These two storages are used in the same way, with the only difference being their lifespan.",(0,i.kt)("br",null),"\nTherefore, the session is more appropriate for storing data that is expensive to compute or cache, while the pipeline storage\nis more suitable for storing data that changes frequently and can be obtained quickly.")))}u.isMDXComponent=!0}}]);