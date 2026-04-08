### 1. Intro
- one of many programming models for creating ASP.NET web sites and web applications.
- provides an easy way to combine HTML, CSS, and server code
- Easy to learn, understand, and use
- Uses an SPA application model (Single Page Application)
- Similar to PHP and Classic ASP
- VB (Visual Basic) or C# (C sharp) scripting languages
- In addition, Web Pages applications are easily extendable with programmable helpers for databases, videos, graphics, social networking and more.

### Examples
  ```
<html>
  <body>
       <h1>Hello Web Pages</h1>
       <p>The time is @DateTime.Now</p>
  </body>
  </html>
  ```

---

### 2. Adding Razor Code
- ASP.NET Web Pages use Razor markup with C# or VB code
- Razor is a simple markup syntax for embedding server code (C# or VB) into ASP.NET web pages.
- Example:
```
<!DOCTYPE html>

<html lang="en">
<head>
     <meta charset="utf-8" />
     <title>Web Pages Demo</title>
</head>
<body>
     <h1>Hello Web Pages</h1>
     <p>The time is @DateTime.Now</p>
</body>
</html>
```

#### 2.1 Razor Syntax for C#
-C# code blocks are enclosed in @{ ... }
- Inline expressions (variables or functions) start with @
- Code statements end with semicolon
- Variables are declared with the var keyword, or the datatype (int, string, etc.)
- Strings are enclosed with quotation marks
- C# code is case sensitive
- C# files have the extension .cshtml
- Example:
```
<!-- Single statement block -->
@{ var message = "Hello World"; }

<!-- Inline expression or variable -->
<p>The value of myMessage is: @message</p>
```

```
<!-- Multi-statement block -->
@{
var greeting = "Welcome to our site!";
var weekDay = DateTime.Now.DayOfWeek;
var greetingMessage = greeting + " Today is: " + weekDay;
}
<p>The greeting is: @greetingMessage</p>
```

### 3. Page Layout
- With Web Pages, consistent look and feel, Every page have the same header, footer, style and layout, this can be done very efficiently.
- You can have reusable blocks of content (content blocks), like headers and footers, in separate files.
- You can also define a consistent layout for all your pages, using a layout template (layout file).

#### 3.1 Content Blocks
- Content block (from another file) can be imported anywhere in a web page, and can contain text, markup, and code, just like any regular web page.
- use the @RenderPage() method to import content from separate files.
- Using common headers and footers as an example, this saves you a lot of work. You don't have to write the same content in every page, and when you change the header or footer files, the content is updated in all your pages.
- Example:
```
<html>
<body>
@RenderPage("header.cshtml")
<h1>Hello Web Pages</h1>
<p>This is a paragraph</p>
@RenderPage("footer.cshtml")
</body>
</html>
```

#### 3.2 Using a Layout Page
- Another approach to creating a consistent look is to use a layout page.
- A layout page contains the structure, but not the content, of a web page.
- When a web page (content page) is linked to a layout page, it will be displayed according to the layout page (template).
- The layout page is just like a normal web page, except from a call to the @RenderBody() method where the content page will be included.
- Each content page must start with a Layout directive.
- Example:
```
<!-- This is Layout.cshtml-->
<html>
<body>
<p>This is header text</p>
@RenderBody()
<p>&copy; 2026 Learning from W3Schools. All rights reserved.</p>
</body>
</html>
```

```
<!-- Any web pagge -->
@{Layout="Layout.cshtml";}

<h1>Welcome</h1>

<p>
This is text.</p>
````

#### 3.4 D.R.Y. - Don't Repeat Yourself
- With two ASP.NET tools, Content Blocks and Layout Pages, you can give your web applications a consistent look.
- These tools also save you a lot of work, since you don't have to repeat the same information on all pages.
- Centralizing markup, style, and code makes web applications much more manageable and easier to maintain.

#### 3.5 Preventing Files from Being Browsed
- With ASP.NET, files with a name that starts with an underscore cannot be browsed from the web.
- If you want to prevent your content blocks or layout files from being viewed by your users, rename the files to: _header.cshtml, _footer.cshtml, _Layout.cshtml

### 3.6 Hiding Sensitive Information
- With ASP.NET, the common way to hide sensitive information (database passwords, email passwords, etc.) is to keep the information in a separate file named "_AppStart".
- Example:
```
_AppStart.cshtmml
@{
WebMail.SmtpServer = "mailserver.example.com";
WebMail.EnableSsl = true;
WebMail.UserName = "username@example.com";
WebMail.Password = "your-password";
WebMail.From = "your-name-here@example.com";
}
```

### 4. Folders:

<img width="494" height="113" alt="image" src="https://github.com/user-attachments/assets/4a45ffa8-dd4c-4b5c-89eb-485d2522973c" />

- The "Account" folder contains logon and security files
- The "App_Data" folder contains databases and data files
- The "Images" folder contains images
- The "Scripts" folder contains browser scripts
- The "Shared" folder contains common files (like layout and style files)

- The virtual name of a web picture might be "Images/pic31.jpg".
- But the physical name is "C:\Johnny\Documents\MyWebSites\Demo\Images\pic31.jpg"

#### 4.1 URLs and Paths
- URLs are used to access files from the web: https://www.w3schools.com/html/html5_intro.asp

- The URL corresponds to a physical file on a server: C:\MyWebSites\w3schools\html\html5_intro.asp

- A virtual path is shorthand to represent physical paths. If you use virtual paths, you can move your pages to a different domain (or server) without having to update the paths.

- URL	https://www.w3schools.com/html/html5_intro.asp
- Server name	w3schools
-Virtual path	/html/html5_intro.asp
- Physical path	C:\MyWebSites\w3schools\html\html5_intro.asp

- ASP.NET has 3 tools for working with folder paths: the ~ operator, the Server.MapPath method, and the Href method.

  #### 4.2 The ~ Operator
- To specify the virtual root in programming code, use the ~ operator.

- If you use the ~ operator, instead of a path, you can move your website to a different folder or location without changing any code:
```
var myImagesFolder = "~/images";
var myStyleSheet = "~/styles/StyleSheet.css";
```

#### 4.3 The Server.MapPath Method
- The Server.MapPath method converts a virtual path (/default.cshtml) to a physical path that the server can understand (C:\Johnny\MyWebSited\Demo\default.cshtml).

You will use this method when you need to open data files located on the server (data files can only be accessed with a full physical path):
```
var pathName = "~/dataFile.txt";
var fileName = Server.MapPath(pathName);
```

#### 4.4 The Href Method
- The Href method converts a path used in the code to a path that the browser can understand (the browser cannot understand the ~ operator).

- You use the Href method to create paths to resources like image files, and CSS files.

- You will often use this method in HTML <a>, <img>, and <link> elements:
```
@{var myStyleSheet = "~/Shared/Site.css";}

<!-- This creates a link to the CSS file. -->
<link rel="stylesheet" type="text/css" href="@Href(myStyleSheet)" />

<!-- Same as : -->
<link rel="stylesheet" type="text/css" href="/Shared/Site.css" />
```
- The Href method is a method of the WebPage Object.

### 5. Global Pages
- AppStart and PageStart are global pages.

#### 5.1 _AppStart (Before web startup)
- Most server side code are written inside individual web pages. For example, if a web page contains an input form, the web page typically contains server code for reading the data.
- However, by creating a page named _AppStart in the root of your site, you can have startup code executed before the site starts. If this page exists, ASP.NET runs it the first time any page in the site is requested.
- Typical use for _AppStart is startup code and initialization of global values like counters and global names.
- Note 1: _AppStart should have the same file extension as your web pages, like: _AppStart.cshtml. 
- Note 2: _AppStart has an underscore prefix. Because of this, the files cannot be browsed directly.

#### 5.2 _PageStart (Before Every Page)
- Just like _AppStart runs before your site starts, you can write code that runs before any page in each folder.
- For each folder in your web, you can add a file named _PageStart.
- Typical use for _PageStart is setting the layout page for all pages in a folder, or checking that a user is logged in before running a page.

<img width="630" height="316" alt="image" src="https://github.com/user-attachments/assets/91e77b00-d176-491a-86b3-1db37a1e7ace" />

- When a request comes in, ASP.NET checks whether _AppStart exists. If so, and this is the first request to the site, _AppStart runs.

- Then ASP.NET checks whether _PageStart exists. If so, _PageStart runs, before the requested page.

- If you include a call to RunPage() inside _PageStart you specify where you want the requested page to run. If not, the _PageStart runs before the requested page.
