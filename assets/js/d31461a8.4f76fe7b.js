"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[1027],{9868:(e,t,r)=>{r.d(t,{Zo:()=>p,kt:()=>f});var n=r(6687);function a(e,t,r){return t in e?Object.defineProperty(e,t,{value:r,enumerable:!0,configurable:!0,writable:!0}):e[t]=r,e}function i(e,t){var r=Object.keys(e);if(Object.getOwnPropertySymbols){var n=Object.getOwnPropertySymbols(e);t&&(n=n.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),r.push.apply(r,n)}return r}function o(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{};t%2?i(Object(r),!0).forEach((function(t){a(e,t,r[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(r)):i(Object(r)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(r,t))}))}return e}function l(e,t){if(null==e)return{};var r,n,a=function(e,t){if(null==e)return{};var r,n,a={},i=Object.keys(e);for(n=0;n<i.length;n++)r=i[n],t.indexOf(r)>=0||(a[r]=e[r]);return a}(e,t);if(Object.getOwnPropertySymbols){var i=Object.getOwnPropertySymbols(e);for(n=0;n<i.length;n++)r=i[n],t.indexOf(r)>=0||Object.prototype.propertyIsEnumerable.call(e,r)&&(a[r]=e[r])}return a}var s=n.createContext({}),d=function(e){var t=n.useContext(s),r=t;return e&&(r="function"==typeof e?e(t):o(o({},t),e)),r},p=function(e){var t=d(e.components);return n.createElement(s.Provider,{value:t},e.children)},c="mdxType",u={inlineCode:"code",wrapper:function(e){var t=e.children;return n.createElement(n.Fragment,{},t)}},m=n.forwardRef((function(e,t){var r=e.components,a=e.mdxType,i=e.originalType,s=e.parentName,p=l(e,["components","mdxType","originalType","parentName"]),c=d(r),m=a,f=c["".concat(s,".").concat(m)]||c[m]||u[m]||i;return r?n.createElement(f,o(o({ref:t},p),{},{components:r})):n.createElement(f,o({ref:t},p))}));function f(e,t){var r=arguments,a=t&&t.mdxType;if("string"==typeof e||a){var i=r.length,o=new Array(i);o[0]=m;var l={};for(var s in t)hasOwnProperty.call(t,s)&&(l[s]=t[s]);l.originalType=e,l[c]="string"==typeof e?e:a,o[1]=l;for(var d=2;d<i;d++)o[d]=r[d];return n.createElement.apply(null,o)}return n.createElement.apply(null,r)}m.displayName="MDXCreateElement"},4628:(e,t,r)=>{r.r(t),r.d(t,{assets:()=>s,contentTitle:()=>o,default:()=>u,frontMatter:()=>i,metadata:()=>l,toc:()=>d});var n=r(8792),a=(r(6687),r(9868));const i={sidebar_position:4},o="Define a Middleware",l={unversionedId:"intregrations/sql-server/define-middleware",id:"intregrations/sql-server/define-middleware",title:"Define a Middleware",description:"A middleware takes the form of a stored procedure located in the web schema and annotated with `#Middleware`.",source:"@site/docs/intregrations/sql-server/define-middleware.md",sourceDirName:"intregrations/sql-server",slug:"/intregrations/sql-server/define-middleware",permalink:"/sqliste/docs/intregrations/sql-server/define-middleware",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/main/doc/docs/intregrations/sql-server/define-middleware.md",tags:[],version:"current",sidebarPosition:4,frontMatter:{sidebar_position:4},sidebar:"docSidebar",previous:{title:"Set up a Controller",permalink:"/sqliste/docs/intregrations/sql-server/define-controller"},next:{title:"Storage",permalink:"/sqliste/docs/intregrations/sql-server/storage"}},s={},d=[],p={toc:d},c="wrapper";function u(e){let{components:t,...r}=e;return(0,a.kt)(c,(0,n.Z)({},p,r,{components:t,mdxType:"MDXLayout"}),(0,a.kt)("h1",{id:"define-a-middleware"},"Define a Middleware"),(0,a.kt)("p",null,"A middleware takes the form of a stored procedure located in the ",(0,a.kt)("em",{parentName:"p"},"web")," schema and annotated with ",(0,a.kt)("inlineCode",{parentName:"p"},"#Middleware"),"."),(0,a.kt)("p",null,"This annotation has several parameters:"),(0,a.kt)("ul",null,(0,a.kt)("li",{parentName:"ul"},"After : Specifies whether it is a post-middleware or pre-middleware. By default, it is a pre-middleware."),(0,a.kt)("li",{parentName:"ul"},"Order : Specifies the execution order of the middleware when there are multiple middlewares."),(0,a.kt)("li",{parentName:"ul"},"PathStarts: Allows targeting a group of routes. For example, a middleware with a ",(0,a.kt)("inlineCode",{parentName:"li"},"PathStart")," value of ",(0,a.kt)("inlineCode",{parentName:"li"},"/api/books")," will only be executed if the corresponding controller for the request declares a route starting with ",(0,a.kt)("inlineCode",{parentName:"li"},"/api/books"),".")),(0,a.kt)("p",null,"Similar to controllers, it is possible to alter the response state and interrupt processing.\nThe difference is that you don't have obligations regarding the response, as shown in the example below."),(0,a.kt)("p",null,"Example: a logging middleware"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- #Middleware(Order = 1, After = false)\nCREATE OR ALTER PROCEDURE [web].[p_middleware_log_request_start]\n    @request_path NVARCHAR(MAX)\nAS\nBEGIN \n    INSERT INTO app_logs ([level], [message])\n    SELECT \n         'info' AS [level]\n        ,'Starting request at route ' + @request_path AS [message]\n    ;\nEND\nGO\n")),(0,a.kt)("p",null,"In some cases, it may be necessary to interrupt the execution of the procedure pipeline and return the response as is\n(for example, if an anonymous user tries to access a protected resource).\nTo do this, simply return the standard parameter ",(0,a.kt)("inlineCode",{parentName:"p"},"next")," with a value of 0, as shown in the following example."),(0,a.kt)("p",null,"Example: (very) simplified authentication control"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- #Middleware(Order = 2, After = false)\nCREATE OR ALTER PROCEDURE [web].[p_middleware_authentication_control]\n    @request_headers NVARCHAR(MAX)\nAS\nBEGIN \n    DECLARE @auth_token NVARCHAR(MAX);\n\n    IF (JSON_VALUE(@request_headers, '$.Authorization') IS NULL)\n    BEGIN\n        SELECT \n             401 AS [response_status]\n            ,0 AS [next]\n        ;\n    END\nEND\nGO\n")),(0,a.kt)("p",null,"In the above example, if no authentication token is provided by the client, a 401 status is returned, and the procedure pipeline is interrupted.",(0,a.kt)("br",null),"\n",(0,a.kt)("em",{parentName:"p"},"In a real world application, you would need to check the validity of the token.")))}u.isMDXComponent=!0}}]);