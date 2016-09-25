# NBlog

Blog on ASP.NET MVC 4 with c#.

Implemented features:
  * Form authentication.
  * Expression visitor. https://github.com/GeorgeMyzor/NBlog/blob/master/DAL/Modifier.cs
  * Generic logger (Currently used NLog, but can be used any). https://github.com/GeorgeMyzor/NBlog/tree/master/LoggingModule
  * Two version of pagination: first - with Ajax and Html helpers(articles); second - with Html helper and javascript(users).
  * CLient side and server side validation with custom attributes.
  * Live search with javascript(Ajax).
  * Custom routing system. URL schema can be found at the end of the file: https://github.com/GeorgeMyzor/NBlog/blob/master/MVCNBlog/App_Start/RouteConfig.cs
  * Many to many relationship between users and roles. Added bonus role VipUser.
  * EF for work with db. Repositories and UnitOfWork patterns.
  * XML Comments.
  * And a lot mote ASP.NET MVC 4 features such as ModelBinders, custom Html helpers, child actions, partial view. 
