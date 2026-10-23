### Intro to ASP
- ASP stands for Active Server Pages.
- It is development framework for building web pages.
- It supports different developement models : (Classical ASP, ASP.Net Web Forms, ASP.NET MVC, ASP.NET Web Pages, ASP.NET API, APS.NET Core).

- ASP and ASP.NET are server side technologies.
- Both technologies enable computer code to be executed by an Internet server.
- When a browser requests an ASP or ASP.NET file, the ASP engine reads the file, executes any code in the file, and returns the result to the browser.
- ASP (aka Classic ASP) was introduced in 1998 as Microsoft's first server side scripting language.
- Classic ASP pages have the file extension .asp and are normally written in VBScript.

### Intro to ASP.NET
- Released in 2002 as a successor to Classic ASP.
- Have the extension .aspx and are normally written in C# (C sharp).
- ASP.NET 4.6 is the latest official version.
- ASP.NET 5 was expected to be an important redesign of ASP.NET. However, the development of ASP.NET 5 was stopped in favor of ASP.NET Core.
- .Foundation: NET Framework (Windows only)
- Performance: Mature, but less optimized
- Architecture: Monolithic, heavy
- Status: Legacy (Maintenance mode)
- relies heavily on Windows Internet Information Services (IIS) for hosting.
- foundation for older web technologies like WebForms (event-driven) and the original ASP.NET MVC.
- Use Case: Primarily for maintaining older, existing enterprise applications built years ago. Microsoft no longer adds new features to this framework; it receives only security and reliability updates.

### ASP.NET Web Pages
- ASP.NET Web Pages is an SPA application model (Single Page Application).
- The SPA model is quite similar to PHP and Classic ASP.
- It is being merged into the new ASP.NET Core.

### ASP.NET MVC
- ASP.NET MVC is an MVC application model (Model-View-Controller).
- ASP.NET MVC is being merged into the new ASP.NET Core.

### ASP.NET Web API
- An  API application model that is mergerd into new Asp.net Core.

### ASP.NET Web Forms
- An event driven application model.
- It is not part of ASP.NET Core.

### ASP.NET Core
- released in 2016.
- merges ASP.NET MVC, ASP.NET Web API, and ASP.NET Web Pages into one application framework.
- Modern, Cross-platform (Windows, Linux, macOS)
- Highly optimized, industry-leading speed
- Architecture: Modular, lightweight, cloud-ready
- complete redesign and re-implementation of ASP.NET
- ideal for containerization (Docker) and modern cloud environments
- Consistently ranked among the fastest web frameworks in the industry. It uses the high-performance Kestrel web server.
- It is lightweight, Modular. You only include the specific components your application needs, which reduces overhead and increases speed.
- Unified Development: Combines what used to be distinct technologies (MVC and Web API) into a single, cohesive programming model.
- Modern Features: Includes built-in support for dependency injection, asynchronous programming, environment-based configuration, and modern frontend integration (Blazor, React, Angular).
