"use strict";(self.webpackChunksqliste_doc=self.webpackChunksqliste_doc||[]).push([[4839],{9868:(e,n,t)=>{t.d(n,{Zo:()=>m,kt:()=>f});var r=t(6687);function a(e,n,t){return n in e?Object.defineProperty(e,n,{value:t,enumerable:!0,configurable:!0,writable:!0}):e[n]=t,e}function o(e,n){var t=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);n&&(r=r.filter((function(n){return Object.getOwnPropertyDescriptor(e,n).enumerable}))),t.push.apply(t,r)}return t}function i(e){for(var n=1;n<arguments.length;n++){var t=null!=arguments[n]?arguments[n]:{};n%2?o(Object(t),!0).forEach((function(n){a(e,n,t[n])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(t)):o(Object(t)).forEach((function(n){Object.defineProperty(e,n,Object.getOwnPropertyDescriptor(t,n))}))}return e}function l(e,n){if(null==e)return{};var t,r,a=function(e,n){if(null==e)return{};var t,r,a={},o=Object.keys(e);for(r=0;r<o.length;r++)t=o[r],n.indexOf(t)>=0||(a[t]=e[t]);return a}(e,n);if(Object.getOwnPropertySymbols){var o=Object.getOwnPropertySymbols(e);for(r=0;r<o.length;r++)t=o[r],n.indexOf(t)>=0||Object.prototype.propertyIsEnumerable.call(e,t)&&(a[t]=e[t])}return a}var s=r.createContext({}),p=function(e){var n=r.useContext(s),t=n;return e&&(t="function"==typeof e?e(n):i(i({},n),e)),t},m=function(e){var n=p(e.components);return r.createElement(s.Provider,{value:n},e.children)},d="mdxType",u={inlineCode:"code",wrapper:function(e){var n=e.children;return r.createElement(r.Fragment,{},n)}},c=r.forwardRef((function(e,n){var t=e.components,a=e.mdxType,o=e.originalType,s=e.parentName,m=l(e,["components","mdxType","originalType","parentName"]),d=p(t),c=a,f=d["".concat(s,".").concat(c)]||d[c]||u[c]||o;return t?r.createElement(f,i(i({ref:n},m),{},{components:t})):r.createElement(f,i({ref:n},m))}));function f(e,n){var t=arguments,a=n&&n.mdxType;if("string"==typeof e||a){var o=t.length,i=new Array(o);i[0]=c;var l={};for(var s in n)hasOwnProperty.call(n,s)&&(l[s]=n[s]);l.originalType=e,l[d]="string"==typeof e?e:a,i[1]=l;for(var p=2;p<o;p++)i[p]=t[p];return r.createElement.apply(null,i)}return r.createElement.apply(null,t)}c.displayName="MDXCreateElement"},485:(e,n,t)=>{t.r(n),t.d(n,{assets:()=>s,contentTitle:()=>i,default:()=>u,frontMatter:()=>o,metadata:()=>l,toc:()=>p});var r=t(8792),a=(t(6687),t(9868));const o={sidebar_position:3},i="Set up a Controller",l={unversionedId:"intregrations/sql-server/define-controller",id:"intregrations/sql-server/define-controller",title:"Set up a Controller",description:"A controller takes the form of a stored procedure located in the web schema.",source:"@site/docs/intregrations/sql-server/define-controller.md",sourceDirName:"intregrations/sql-server",slug:"/intregrations/sql-server/define-controller",permalink:"/sqliste/docs/intregrations/sql-server/define-controller",draft:!1,editUrl:"https://github.com/corentin703/sqliste/tree/main/doc/docs/intregrations/sql-server/define-controller.md",tags:[],version:"current",sidebarPosition:3,frontMatter:{sidebar_position:3},sidebar:"docSidebar",previous:{title:"Structure",permalink:"/sqliste/docs/intregrations/sql-server/structure"},next:{title:"Define a Middleware",permalink:"/sqliste/docs/intregrations/sql-server/define-middleware"}},s={},p=[{value:"URI parameters",id:"uri-parameters",level:2},{value:"Route Parameters",id:"route-parameters",level:2},{value:"Consuming a Form",id:"consuming-a-form",level:2},{value:"Retrieving a File",id:"retrieving-a-file",level:3},{value:"File Download",id:"file-download",level:2}],m={toc:p},d="wrapper";function u(e){let{components:n,...t}=e;return(0,a.kt)(d,(0,r.Z)({},m,t,{components:n,mdxType:"MDXLayout"}),(0,a.kt)("h1",{id:"set-up-a-controller"},"Set up a Controller"),(0,a.kt)("p",null,"A controller takes the form of a stored procedure located in the ",(0,a.kt)("inlineCode",{parentName:"p"},"web")," schema."),(0,a.kt)("p",null,"Here is an example of a basic controller:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- #Route(\"/api/helloWorld\")\n-- #HttpGet(\"HelloWorld\")\nCREATE OR ALTER PROCEDURE [web].[p_hello_world] \nAS \nBEGIN\n    SELECT \n         'Greetings, everyone!' AS [response_body] -- Returning the response body\n        ,'text/plain' AS [response_content_type] -- MIME type of the response body\n        ,200 AS [response_status] -- HTTP status of the response\n    ;\nEND\nGO\n")),(0,a.kt)("h2",{id:"uri-parameters"},"URI parameters"),(0,a.kt)("p",null,"A URI parameter is an argument provided as follows in a URI: ",(0,a.kt)("inlineCode",{parentName:"p"},"/api/exemple?name=Corentin"),(0,a.kt)("br",null),"\nTo retrieve its value, simply include it as an argument in the stored procedure:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- Define the 'name' parameter in the route using curly braces\n-- #Route(\"/api/helloWorld\")\n-- #HttpGet(\"HelloWorld\")\nCREATE OR ALTER PROCEDURE [web].[p_hello_world] \n    @name NVARCHAR(100) = NULL -- Retrieve the 'name' URI parameter\nAS \nBEGIN\n    DECLARE @response_body NVARCHAR(MAX);\n    \n    IF (@name IS NULL)\n        SET @response_body = 'Greetings, everyone!';\n    ELSE    \n        SELECT @response_body = 'Greetings, ' + @name + '!';\n        \n    SELECT \n         @response_body AS [response_body]\n        ,'text/plain' AS [response_content_type]\n        ,200 AS [response_status]\n    ;\nEND\nGO\n")),(0,a.kt)("p",null,"A URI parameter is always considered optional and should be of type ",(0,a.kt)("em",{parentName:"p"},"NVARCHAR")," (you can cast it to another type within the stored procedure if needed)."),(0,a.kt)("admonition",{type:"caution"},(0,a.kt)("p",{parentName:"admonition"},"It is recommended to provide a default value for URI parameters and optional route parameters in the stored procedure arguments. If no default value is provided and the parameter is not provided in the URI, the database will not be able to execute the procedure and will return an error.")),(0,a.kt)("h2",{id:"route-parameters"},"Route Parameters"),(0,a.kt)("p",null,"Like most web frameworks, SQListe allows you to define route patterns. This allows you to pass arguments directly in the route instead of using URI parameters or request body."),(0,a.kt)("p",null,"Exemple:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- Define the 'name' parameter in the route using curly braces\n-- #Route(\"/api/helloWorld/{name}\")\n-- #HttpGet(\"HelloWorld\")\nCREATE OR ALTER PROCEDURE [web].[p_hello_world] \n    @name NVARCHAR(100) -- Retrieve the parameter with the same name as the stored procedure argument\nAS \nBEGIN\n    DECLARE @response_body NVARCHAR(MAX);\n    \n    SELECT @response_body = 'Greetings, ' + @name + '!';\n        \n    SELECT \n         @response_body AS [response_body]\n        ,'text/plain' AS [response_content_type]\n        ,200 AS [response_status]\n    ;\nEND\nGO\n")),(0,a.kt)("admonition",{type:"note"},(0,a.kt)("p",{parentName:"admonition"},"It is possible to define multiple parameters in the route with different names."),(0,a.kt)("p",{parentName:"admonition"},"Like URI parameters, route parameters will be injected as NVARCHAR type.")),(0,a.kt)("admonition",{type:"caution"},(0,a.kt)("p",{parentName:"admonition"},"If a parameter name matches a reserved keyword, the reserved keyword takes precedence.")),(0,a.kt)("p",null,"In the above example, the parameter is considered required: if it is not provided, SQListe will not match this route.\nTo define an optional parameter, you can do the following:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- Define the 'name' parameter in the route using curly braces\n-- #Route(\"/api/helloWorld/{name?}\")\n-- #HttpGet(\"HelloWorld\")\nCREATE OR ALTER PROCEDURE [web].[p_hello_world] \n    @name NVARCHAR(100) = NULL -- Retrieve the parameter with the same name as the stored procedure argument, with a default value of NULL (useful if the parameter is not provided).\nAS \nBEGIN\n    DECLARE @response_body NVARCHAR(MAX);\n    \n    IF (@name IS NULL)\n        SET @response_body = 'Greetings, everyone!';\n    ELSE    \n        SELECT @response_body = 'Greetings, ' + @name + '!';\n        \n    SELECT \n         @response_body AS [response_body]\n        ,'text/plain' AS [response_content_type]\n        ,200 AS [response_status]\n    ;\nEND\nGO\n")),(0,a.kt)("p",null,"An optional parameter must be placed at the end of the route.\nFor example:"),(0,a.kt)("ul",null,(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("inlineCode",{parentName:"li"},"/api/say/hello/{name?}/{age?}")," => Valid."),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("inlineCode",{parentName:"li"},"/api/say/hello/{name}/withAge/{age?}")," => Valid."),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("inlineCode",{parentName:"li"},"/api/say/hello/{name?}/withAge/{age?}")," => Invalid: correct functionality cannot be guaranteed.")),(0,a.kt)("p",null,"When dealing with multiple optional parameters, it is often better to use URI parameters as described earlier."),(0,a.kt)("admonition",{title:"Limitations",type:"warning"},(0,a.kt)("p",{parentName:"admonition"},"The length of a URI is limited to a maximum of 2048 characters. Problems may occur when reading parameters that exceed this limit."),(0,a.kt)("p",{parentName:"admonition"},(0,a.kt)("em",{parentName:"p"},"Some servers allow increasing this limit, but it is important to check whether the HTTP client/browser supports this increase."))),(0,a.kt)("h2",{id:"consuming-a-form"},"Consuming a Form"),(0,a.kt)("p",null,"Some Content-Type are used to handle form data (especially for the ",(0,a.kt)("inlineCode",{parentName:"p"},"<form />")," tag in HTML).\nIn such cases, the Content-Type will be either ",(0,a.kt)("inlineCode",{parentName:"p"},"application/x-www-form-urlencoded")," or ",(0,a.kt)("inlineCode",{parentName:"p"},"multipart/form-data"),"."),(0,a.kt)("p",null,"When a request of this type is sent to SQListe, it will not inject the data into the ",(0,a.kt)("em",{parentName:"p"},"request_body")," parameter but into the ",(0,a.kt)("em",{parentName:"p"},"request_form")," parameter."),(0,a.kt)("p",null,"The content of this parameter will be a JSON in the following format:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-json",metastring:"lines",lines:!0},'[\n  {\n    "name": "field name",\n    "value": "field value" // Always a string type\n  },\n  {\n    "name": "second field name",\n    "value": "second field value" // Always a string type\n  },\n  // ...\n]\n')),(0,a.kt)("admonition",{type:"info"},(0,a.kt)("p",{parentName:"admonition"},"When the request_form parameter is not equal to NULL, the request_body parameter is.")),(0,a.kt)("h3",{id:"retrieving-a-file"},"Retrieving a File"),(0,a.kt)("p",null,"Form requests allow direct file uploads without any specific encoding (such as base64).",(0,a.kt)("br",null),"\nWhen an element corresponds to a file, SQListe will inject a JSON object in the ",(0,a.kt)("em",{parentName:"p"},"request_form")," parameter for the respective field in the following format:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-json",metastring:"lines",lines:!0},'[\n  {\n    "name": "name of a file field",\n    "headers": {\n      "Content-Type": "image/png", // MIME type corresponding to the file\n      // ...\n    },\n    "length": 128000, // File size (in bytes)\n    "contentType": "image/png", // Shortcut to access the "Content-Type" header of the file\n    "contentDisposition": "form-data; name=\\"fieldName\\"; filename=\\"filename.png\\"", // Shortcut to access the "Content-Disposition" header of the file\n    "fileName": "filename.png" // File name extracted from "Content-Disposition"\n  },\n  {\n    "name": "name of a regular field",\n    "value": "value of the regular field" // Always a string type\n  },\n  // ...\n]\n')),(0,a.kt)("p",null,"To access the content of the file, you need to retrieve a procedure parameter whose name follows the format\n",(0,a.kt)("inlineCode",{parentName:"p"},"request_form_file_<nomDuChamp>")," and of type VARBINARY."),(0,a.kt)("p",null,"Example: Let's retrieve a form to save a music album with its cover photo."),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},"-- #Route(\"/api/sampleForm\")\n-- #HttpPost(\"sampleForm\")\nCREATE OR ALTER PROCEDURE [web].[p_sample_form] \n    @request_form NVARCHAR(MAX), -- The user-entered information will be stored here\n    @request_form_file_cover VARBINARY(MAX) -- We retrieve the cover as binary data, as sent by the user\nAS \nBEGIN\n    DECLARE @form_data TABLE (\n         [name] NVARCHAR(500)\n        ,[value] NVARCHAR(500)\n        ,[file_name] NVARCHAR(500)\n        ,[length] BIGINT\n    );\n    \n    INSERT INTO @form_data\n    SELECT \n         [name]\n        ,[value]\n        ,[fileName] AS [file_name]\n        ,[length]\n    FROM OPENJSON(@request_form)\n    WITH (\n         [name] NVARCHAR(500)\n        ,[value] NVARCHAR(500)\n        ,[fileName] NVARCHAR(500)\n        ,[length] BIGINT\n    );\n    \n    DECLARE @artist NVARCHAR(1000);\n    DECLARE @title NVARCHAR(1000);\n    DECLARE @cover_file_name NVARCHAR(1000);\n    DECLARE @cover_length BIGINT;\n    \n    SELECT TOP 1 @artist = [value] FROM @form_data\n    WHERE [name] = 'artist';\n    \n    SELECT TOP 1 @title = [value] FROM @form_data\n    WHERE [name] = 'title';\n    \n    SELECT TOP 1 \n         @cover_file_name = [file_name] \n        ,@cover_length = [length] / 1000 -- Obtaining the result in KB \n    FROM @form_data\n    WHERE [name] = 'cover';\n    \n    -- Checking the image size\n    IF (@cover_length > 500)\n    BEGIN\n        SELECT\n             '{ \"message\": \"The image you provided must be smaller than 500KB\" }' AS [response_body]\n            ,'application/json' AS [response_content_type] \n            ,413 AS [response_status] -- 413 = Payload Too Large\n        ;\n    END\n\n    INSERT INTO [dbo].[albums] ([artist], [title], [cover], [cover_file_name])\n    VALUES (\n         @artist\n        ,@title\n        ,@request_form_file_cover\n        ,@cover_file_name\n    );\n    \n    SELECT \n        204 AS [response_status]\n    ;\nEND\nGO\n")),(0,a.kt)("admonition",{type:"caution"},(0,a.kt)("p",{parentName:"admonition"},"Make sure to check the file sizes that you archive.",(0,a.kt)("br",null),"\nThe transfer of large files ",(0,a.kt)("strong",{parentName:"p"},"is not")," guaranteed.")),(0,a.kt)("h2",{id:"file-download"},"File Download"),(0,a.kt)("p",null,"To send files to the user, SQListe provides 3 output parameters:"),(0,a.kt)("ul",null,(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("em",{parentName:"li"},"response_file")," : used to define the content of the returned file (type ",(0,a.kt)("em",{parentName:"li"},"VARBINARY"),")"),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("em",{parentName:"li"},"response_file_name")," : indicates the file name (type ",(0,a.kt)("em",{parentName:"li"},"NVARCHAR"),")"),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("em",{parentName:"li"},"response_file_inline")," : ",(0,a.kt)("em",{parentName:"li"},"BIT")," to open the file in the browser's viewer (if available, otherwise it will be downloaded directly). False by default.")),(0,a.kt)("p",null,"Example:"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-sql"},'-- #Route("/api/sampleFileDownload")\n-- #HttpPost("sampleFileDownload")\nCREATE OR ALTER PROCEDURE [web].[p_sample_file_download] \nAS \nBEGIN\n    DECLARE @file VARBINARY(MAX);\n    DECLARE @file_name NVARCHAR(255);\n    DECLARE @file_mime NVARCHAR(255);\n    DECLARE @file_inline BIT = 0;\n    \n    -- Retrieving the file and its information\n    -- ...\n\n    -- If it\'s a PDF, request to display it in the browser, otherwise download directly\n    IF (@file_mime = \'application/pdf\')\n        SET @file_inline = 1;\n\n    SELECT \n         @file AS [response_file]\n        ,@file_name AS [response_file_name]\n        ,@file_inline AS [response_file_inline]\n        ,@file_mime AS [response_content_type]\n        ,200 AS [response_status]\n    ;\nEND\nGO\n')))}u.isMDXComponent=!0}}]);