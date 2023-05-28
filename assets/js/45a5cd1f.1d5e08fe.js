"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[3365],{9868:(e,t,r)=>{r.d(t,{Zo:()=>d,kt:()=>f});var n=r(6687);function o(e,t,r){return t in e?Object.defineProperty(e,t,{value:r,enumerable:!0,configurable:!0,writable:!0}):e[t]=r,e}function a(e,t){var r=Object.keys(e);if(Object.getOwnPropertySymbols){var n=Object.getOwnPropertySymbols(e);t&&(n=n.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),r.push.apply(r,n)}return r}function i(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{};t%2?a(Object(r),!0).forEach((function(t){o(e,t,r[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(r)):a(Object(r)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(r,t))}))}return e}function s(e,t){if(null==e)return{};var r,n,o=function(e,t){if(null==e)return{};var r,n,o={},a=Object.keys(e);for(n=0;n<a.length;n++)r=a[n],t.indexOf(r)>=0||(o[r]=e[r]);return o}(e,t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);for(n=0;n<a.length;n++)r=a[n],t.indexOf(r)>=0||Object.prototype.propertyIsEnumerable.call(e,r)&&(o[r]=e[r])}return o}var l=n.createContext({}),p=function(e){var t=n.useContext(l),r=t;return e&&(r="function"==typeof e?e(t):i(i({},t),e)),r},d=function(e){var t=p(e.components);return n.createElement(l.Provider,{value:t},e.children)},c="mdxType",u={inlineCode:"code",wrapper:function(e){var t=e.children;return n.createElement(n.Fragment,{},t)}},m=n.forwardRef((function(e,t){var r=e.components,o=e.mdxType,a=e.originalType,l=e.parentName,d=s(e,["components","mdxType","originalType","parentName"]),c=p(r),m=o,f=c["".concat(l,".").concat(m)]||c[m]||u[m]||a;return r?n.createElement(f,i(i({ref:t},d),{},{components:r})):n.createElement(f,i({ref:t},d))}));function f(e,t){var r=arguments,o=t&&t.mdxType;if("string"==typeof e||o){var a=r.length,i=new Array(a);i[0]=m;var s={};for(var l in t)hasOwnProperty.call(t,l)&&(s[l]=t[l]);s.originalType=e,s[c]="string"==typeof e?e:o,i[1]=s;for(var p=2;p<a;p++)i[p]=r[p];return n.createElement.apply(null,i)}return n.createElement.apply(null,r)}m.displayName="MDXCreateElement"},854:(e,t,r)=>{r.r(t),r.d(t,{assets:()=>l,contentTitle:()=>i,default:()=>u,frontMatter:()=>a,metadata:()=>s,toc:()=>p});var n=r(8792),o=(r(6687),r(9868));const a={sidebar_position:2},i="Concepts",s={unversionedId:"concepts",id:"concepts",title:"Concepts",description:"In SQListe, the main idea is to allow a SQL developer to define a web service powered by the DBMS through stored procedures.",source:"@site/docs/concepts.md",sourceDirName:".",slug:"/concepts",permalink:"/sqliste/docs/concepts",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/main/doc/docs/concepts.md",tags:[],version:"current",sidebarPosition:2,frontMatter:{sidebar_position:2},sidebar:"docSidebar",previous:{title:"Introduction",permalink:"/sqliste/docs/intro"},next:{title:"Getting started",permalink:"/sqliste/docs/category/getting-started"}},l={},p=[{value:"Input/Output of Procedures",id:"inputoutput-of-procedures",level:2},{value:"Procedure Pipeline",id:"procedure-pipeline",level:2},{value:"Controllers",id:"controllers",level:2},{value:"Middlewares",id:"middlewares",level:2},{value:"Annotations",id:"annotations",level:2}],d={toc:p},c="wrapper";function u(e){let{components:t,...r}=e;return(0,o.kt)(c,(0,n.Z)({},d,r,{components:t,mdxType:"MDXLayout"}),(0,o.kt)("h1",{id:"concepts"},"Concepts"),(0,o.kt)("p",null,"In SQListe, the main idea is to allow a SQL developer to define a web service powered by the DBMS through stored procedures."),(0,o.kt)("h2",{id:"inputoutput-of-procedures"},"Input/Output of Procedures"),(0,o.kt)("p",null,"A SQListe procedure takes a defined list of standard parameters (all optional). It then returns these parameters using a ",(0,o.kt)("em",{parentName:"p"},"SELECT")," statement with the same name."),(0,o.kt)("p",null,"In the case of a pipeline, the parameters retrieved at the output of step ",(0,o.kt)("em",{parentName:"p"},"x")," become the parameters of step ",(0,o.kt)("em",{parentName:"p"},"x+1"),".\nIf a procedure doesn't return a parameter that was injected, its value will remain unchanged when passed to the next procedure."),(0,o.kt)("admonition",{type:"note"},(0,o.kt)("p",{parentName:"admonition"},"This applies only to standard parameters.",(0,o.kt)("br",null),"\nCustom parameters, such as those from the route, are not affected.")),(0,o.kt)("h2",{id:"procedure-pipeline"},"Procedure Pipeline"),(0,o.kt)("p",null,"A pipeline is a sequence of steps that are executed in a defined order to reach a final state."),(0,o.kt)("p",null,"In our case, this corresponds to the execution of procedures that act as middleware ",(0,o.kt)("strong",{parentName:"p"},"before")," and ",(0,o.kt)("strong",{parentName:"p"},"after"),"\nthe procedure that actually performs the request's purpose."),(0,o.kt)("p",null,"It can be summarized by the following diagram:"),(0,o.kt)("mermaid",{value:"flowchart TD\n    request[Request] --\x3e before-middleware1\n    \n    subgraph before-midleware[Pre-Middleware]\n        before-middleware1[/Logging/] --\x3e before-middleware2[/Authentication/]\n    end\n    \n    before-middleware2 --\x3e controller[/Controller/]\n    controller --\x3e post-middleware1\n\n    subgraph after-midleware[Post-Middleware]\n        post-middleware1[/Error Handling/]\n    end\n\n    post-middleware1 --\x3e response[Response]"}),(0,o.kt)("p",null,"Two types of procedures emerge from this diagram: controllers and middleware."),(0,o.kt)("h2",{id:"controllers"},"Controllers"),(0,o.kt)("p",null,"In our case, a controller is a component responsible for orchestrating the business logic required to transform an HTTP request into an HTTP response."),(0,o.kt)("h2",{id:"middlewares"},"Middlewares"),(0,o.kt)("p",null,"Middleware is a component that allows for the common logic to be shared among several more specific components.",(0,o.kt)("br",null),"\nWithin SQListe, middleware can be executed before (pre-middleware) or after (post-middleware) a controller."),(0,o.kt)("p",null,"Some examples of middleware usage:"),(0,o.kt)("ul",null,(0,o.kt)("li",{parentName:"ul"},"Verify if the user is authenticated, has the right to access certain resources, and return an error with a 401 or 403 status code accordingly."),(0,o.kt)("li",{parentName:"ul"},"Log a request."),(0,o.kt)("li",{parentName:"ul"},"Catch unhandled errors and format the HTTP response accordingly.")),(0,o.kt)("h2",{id:"annotations"},"Annotations"),(0,o.kt)("p",null,"Annotations in SQListe are keywords preceded by a ",(0,o.kt)("em",{parentName:"p"},"#")," character, commented above a procedure, with optional arguments. They are used to configure the processing that SQListe will apply to a procedure."),(0,o.kt)("p",null,"Example: "),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-sql"},'-- Annotations can coexist with regular comments ;)\n-- #Route("/api/helloWorld")\n-- #HttpGet\nCREATE OR ALTER PROCEDURE [web].[p_proc]...\n')),(0,o.kt)("p",null,"Arguments can be passed to annotations in two ways:"),(0,o.kt)("ul",null,(0,o.kt)("li",{parentName:"ul"},'Ordered : #Annotation("1", false, 3) where the order of the parameters is important and none of them can be omitted.'),(0,o.kt)("li",{parentName:"ul"},'Named : #Annotation(Param2 = 2, Param1 = "1") where the order of the parameters is arbitrary, and parameters with default values can be omitted.')),(0,o.kt)("admonition",{type:"info"},(0,o.kt)("p",{parentName:"admonition"},"Passing parameters in ordered mode is recommended when the annotation has only a few parameters. However, if it has more than two parameters, it is better to use named mode.")),(0,o.kt)("admonition",{type:"tip"},(0,o.kt)("p",{parentName:"admonition"},"Annotations are processed independently, so their order doesn't matter.")))}u.isMDXComponent=!0}}]);