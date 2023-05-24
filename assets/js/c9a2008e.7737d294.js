"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[6393],{9868:(t,e,n)=>{n.d(e,{Zo:()=>u,kt:()=>m});var r=n(6687);function a(t,e,n){return e in t?Object.defineProperty(t,e,{value:n,enumerable:!0,configurable:!0,writable:!0}):t[e]=n,t}function o(t,e){var n=Object.keys(t);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(t);e&&(r=r.filter((function(e){return Object.getOwnPropertyDescriptor(t,e).enumerable}))),n.push.apply(n,r)}return n}function l(t){for(var e=1;e<arguments.length;e++){var n=null!=arguments[e]?arguments[e]:{};e%2?o(Object(n),!0).forEach((function(e){a(t,e,n[e])})):Object.getOwnPropertyDescriptors?Object.defineProperties(t,Object.getOwnPropertyDescriptors(n)):o(Object(n)).forEach((function(e){Object.defineProperty(t,e,Object.getOwnPropertyDescriptor(n,e))}))}return t}function p(t,e){if(null==t)return{};var n,r,a=function(t,e){if(null==t)return{};var n,r,a={},o=Object.keys(t);for(r=0;r<o.length;r++)n=o[r],e.indexOf(n)>=0||(a[n]=t[n]);return a}(t,e);if(Object.getOwnPropertySymbols){var o=Object.getOwnPropertySymbols(t);for(r=0;r<o.length;r++)n=o[r],e.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(t,n)&&(a[n]=t[n])}return a}var i=r.createContext({}),s=function(t){var e=r.useContext(i),n=e;return t&&(n="function"==typeof t?t(e):l(l({},e),t)),n},u=function(t){var e=s(t.components);return r.createElement(i.Provider,{value:e},t.children)},d="mdxType",c={inlineCode:"code",wrapper:function(t){var e=t.children;return r.createElement(r.Fragment,{},e)}},h=r.forwardRef((function(t,e){var n=t.components,a=t.mdxType,o=t.originalType,i=t.parentName,u=p(t,["components","mdxType","originalType","parentName"]),d=s(n),h=a,m=d["".concat(i,".").concat(h)]||d[h]||c[h]||o;return n?r.createElement(m,l(l({ref:e},u),{},{components:n})):r.createElement(m,l({ref:e},u))}));function m(t,e){var n=arguments,a=e&&e.mdxType;if("string"==typeof t||a){var o=n.length,l=new Array(o);l[0]=h;var p={};for(var i in e)hasOwnProperty.call(e,i)&&(p[i]=e[i]);p.originalType=t,p[d]="string"==typeof t?t:a,l[1]=p;for(var s=2;s<o;s++)l[s]=n[s];return r.createElement.apply(null,l)}return r.createElement.apply(null,n)}h.displayName="MDXCreateElement"},7183:(t,e,n)=>{n.r(e),n.d(e,{assets:()=>i,contentTitle:()=>l,default:()=>c,frontMatter:()=>o,metadata:()=>p,toc:()=>s});var r=n(8792),a=(n(6687),n(9868));const o={},l="HTTP",p={unversionedId:"annotations/http",id:"annotations/http",title:"HTTP",description:"These annotations are used to define routes and their associated operations.",source:"@site/docs/annotations/http.md",sourceDirName:"annotations",slug:"/annotations/http",permalink:"/sqliste/docs/annotations/http",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/main/doc/docs/annotations/http.md",tags:[],version:"current",frontMatter:{},sidebar:"docSidebar",previous:{title:"Annotations",permalink:"/sqliste/docs/category/annotations"},next:{title:"Middleware",permalink:"/sqliste/docs/annotations/middlewares"}},i={},s=[{value:"#Route",id:"route",level:2},{value:"Operations and HTTP Verbs",id:"operations-and-http-verbs",level:2},{value:"#HttpGet",id:"httpget",level:3},{value:"#HttpPost",id:"httppost",level:3},{value:"#HttpPut",id:"httpput",level:3},{value:"#HttpPatch",id:"httppatch",level:3},{value:"#HttpDelete",id:"httpdelete",level:3}],u={toc:s},d="wrapper";function c(t){let{components:e,...n}=t;return(0,a.kt)(d,(0,r.Z)({},u,n,{components:e,mdxType:"MDXLayout"}),(0,a.kt)("h1",{id:"http"},"HTTP"),(0,a.kt)("p",null,"These annotations are used to define routes and their associated operations."),(0,a.kt)("h2",{id:"route"},"#Route"),(0,a.kt)("table",null,(0,a.kt)("thead",{parentName:"table"},(0,a.kt)("tr",{parentName:"thead"},(0,a.kt)("th",{parentName:"tr",align:null},"Arguments"),(0,a.kt)("th",{parentName:"tr",align:null},"Type"),(0,a.kt)("th",{parentName:"tr",align:null},"Description"),(0,a.kt)("th",{parentName:"tr",align:null},"Optional"),(0,a.kt)("th",{parentName:"tr",align:null},"Default Value"))),(0,a.kt)("tbody",{parentName:"table"},(0,a.kt)("tr",{parentName:"tbody"},(0,a.kt)("td",{parentName:"tr",align:null},"Path"),(0,a.kt)("td",{parentName:"tr",align:null},"string"),(0,a.kt)("td",{parentName:"tr",align:null},"The URI at which the procedure is resolved"),(0,a.kt)("td",{parentName:"tr",align:null},"No"),(0,a.kt)("td",{parentName:"tr",align:null})))),(0,a.kt)("p",null,"Examples:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre"},'#Route("/route")\n#Route(Path = "/route")\n')),(0,a.kt)("h2",{id:"operations-and-http-verbs"},"Operations and HTTP Verbs"),(0,a.kt)("p",null,"These annotations are used to define the HTTP method with which the stored procedure will be resolved."),(0,a.kt)("h3",{id:"httpget"},"#HttpGet"),(0,a.kt)("p",null,"Associates the HTTP GET verb."),(0,a.kt)("p",null,"Examples:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre"},'#HttpGet("GetBook")\n#HttpGet(Id = "GetBook")\n')),(0,a.kt)("h3",{id:"httppost"},"#HttpPost"),(0,a.kt)("p",null,"Associates the HTTP POST verb."),(0,a.kt)("p",null,"Examples:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre"},'#HttpPost("CreateBook")\n#HttpPost(Id = "CreateBook")\n')),(0,a.kt)("h3",{id:"httpput"},"#HttpPut"),(0,a.kt)("p",null,"Associates the HTTP PUT verb."),(0,a.kt)("p",null,"Examples:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre"},'#HttpPost("UpdateBook")\n#HttpPost(Id = "UpdateBook")\n')),(0,a.kt)("h3",{id:"httppatch"},"#HttpPatch"),(0,a.kt)("p",null,"Associates the HTTP PATCH verb."),(0,a.kt)("p",null,"Examples:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre"},'#HttpPatch("UpdateBook")\n#HttpPatch(Id = "UpdateBook")\n')),(0,a.kt)("h3",{id:"httpdelete"},"#HttpDelete"),(0,a.kt)("p",null,"Associates the HTTP DELETE verb."),(0,a.kt)("p",null,"Examples:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre"},'#HttpDelete("DeleteBook")\n#HttpDelete(Id = "DeleteBook")\n')),(0,a.kt)("admonition",{type:"tip"},(0,a.kt)("p",{parentName:"admonition"},"It is possible to define multiple verbs for the same procedure.")),(0,a.kt)("admonition",{type:"caution"},(0,a.kt)("p",{parentName:"admonition"},"Defining an operation name is necessary for the JSON OpenAPI to be valid.")))}c.isMDXComponent=!0}}]);