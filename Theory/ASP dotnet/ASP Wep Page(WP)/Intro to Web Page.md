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
