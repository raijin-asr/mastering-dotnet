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

### 6. HTML Forms
- A form is a section of an HTML document where you put input controls (text boxes, check boxes, radio buttons, and pull-down lists).
```
<html>
<body> 
@{
if (IsPost) { 
string companyname = Request["CompanyName"]; 
string contactname = Request["ContactName"]; 
<p>You entered: <br />
Company Name: @companyname <br />
Contact Name: @contactname </p>
}
else
{
<form method="post" action="">
Company Name:<br />
<input type="text" name="CompanyName" value="" /><br />
Contact Name:<br />
<input type="text" name="ContactName" value="" /><br /><br />
<input type="submit" value="Submit" class="submit" />
</form>
}
} 
</body> 
</html>
```

#### 6.1 Displaying Images
- Razor code to display image as given:
  ```<img src="images/Photo1.jpg" alt="Sample" />```
- Example below shows how to display a selected picture which the user selects from a drop-down list:  
```
@{
var imagePath="";
if (Request["Choice"] != null)
   {imagePath="images/" + Request["Choice"];}
}
<!DOCTYPE html>
<html>
<body>
<h1>Display Images</h1>
<form method="post" action="">
I want to see:
<select name="Choice">
  <option value="Photo1.jpg">Photo 1</option>
  <option value="Photo2.jpg">Photo 2</option>
  <option value="Photo3.jpg">Photo 3</option>
</select>
<input type="submit" value="Submit" />
@if (imagePath != "")
{
<p>    
<img src="@imagePath" alt="Sample" />
</p>
} 
</form>
</body>
</html>
```

- Explaining above code:
  1. The server creates a variable called `imagePath`.
  2. The HTML page has a drop-down list (a `<select>` element) named `Choice`. It lets the user select a friendly name (like Photo 1), and passes a file name (like `Photo1.jpg`) when the page is submitted to the web server.
  3. The Razor code reads the value of `Choice` by `Request["Choice"]`. If it has a value, the code constructs a path to the image `images/Photo1.jpg`, and stores it in the variable `imagePath`.
  4. In the HTML page, there is an `<img>` element to display the image. The `src` attribute is set to the value of the `imagePath` variable when the page displays.
  5. The `<img>` element is in an `if` block to prevent trying to display an image with no name (like the first time the page is displayed).

---

### 7. Web Page Objects

- Web Page is about objects. 
- **Example:** `@RenderPage("header.cshtml")`, `@RenderBody()` are page object methods, and `IsPost` in `if(IsPost)` and `Request` in `if(Request["Choice"] != null)` are page objects.

#### 7.1 Some Page Object Methods

| Method | Description |
| :--- | :--- |
| `href` | Builds a URL using the specified parameters |
| `RenderBody()` | Renders the portion of a content page that is not within a named section (In layout pages) |
| `RenderPage(page)` | Renders the content of one page within another page |
| `RenderSection(section)` | Renders the content of a named section (In layout pages) |
| `Write(object)` | Writes the object as an HTML-encoded string |
| `WriteLiteral` | Writes an object without HTML-encoding it first. |

#### 7.2 Some Page Object Properties

| Property | Description |
| :--- | :--- |
| `IsPost` | Returns true if the HTTP data transfer method used by the client is a POST request |
| `Layout` | Gets or sets the path of a layout page |
| `Page` | Provides property-like access to data shared between pages and layout pages |
| `Request` | Gets the `HttpRequest` object for the current HTTP request |
| `Server` | Gets the `HttpServerUtility` object that provides web-page processing methods |

#### 7.3 Page Property of the Page Object

- It provides property-like access to data shared between pages and layout pages.
- You can use (add) your own properties to the `Page` property: `Page.Title`, `Page.Version`, `Page.anythingyoulike`.
- The pages property is very helpful. For instance, it makes it possible to set the page title in content files and use it in the layout file:

```cshtml

@{
Layout="~/Shared/Layout.cshtml";
Page.Title="Home Page"
}

<h1>Welcome guys</h1>
<p>My dotnet journey</p>
```

```Layout.cshtml
<!DOCTYPE html>
<html>
<head>
<title>@Page.Title</title>
</head>
<body>
@RenderBody()
</body>
</html>
```
### 8. ASP.NET Web Pages Files
#### 8.1 Working with Text Files:
- Text file is also called flat files and stores data in formats such as .txt, .xml. and .csv.

#### 8.2 Display Data from a Text File
- Use 'File.ReadAllLines' to open file and read all lines into an array. 
- 'Server.MapPath' to read exact file path.
- Example:
```
@{
var dataFile = Server.MapPath("~/App_Date/File.txt"); 
Array userData = File.ReadAllLines(dataFile);
}

<!DOCTYPE html>
<html>
<body>

<h1>Reading Data from a File</h1>
@foreach (string dataLine in userData)
{
  foreach (string dataItem in dataLine.Split(','))
  {@dataItem <text>&nbsp;</text>}
  <br />
}
</body>
</html>
```


### 9. WP Databases
#### 9.1 Displaying Data from Database
- Connect to an existing database or create new from scratch. 
- Following is Products.cshtml file to read data from Product table from Shop123 database.
```
@{
var db = Database.Open("Shop123"); 
var selectQueryString = "SELECT * FROM Product ORDER BY Name"; 
}
<html> 
<body> 
<h1>Small Bakery Products</h1> 
<table> 
<tr>
<th>Id</th> 
<th>Product</th> 
<th>Description</th> 
<th>Price</th> 
</tr>
@foreach(var row in db.Query(selectQueryString))
{
<tr> 
<td>@row.Id</td> 
<td>@row.Name</td> 
<td>@row.Description</td> 
<td align="right">@row.Price</td> 
</tr> 
}
</table> 
</body> 
</html>
```

- Database.Open(name) connects to database by first searching application's App_Data folder database with matching name parameter without file name extension else if no file found it looks for connection string in web.config file.

#### 9.2 Database Object Reference
- Database.Executes(SqlStatement,[,paremteres]): Executes Sql statement such as INSERT, DELETE or UPDATE and returns a count of affected records. Parameters is optional.
- Database.GetLastInsertId(): Returns identity column from most recently inserted row.
- Database.Open(filename or connectStringName): Opens database file or database specified in connection string of web.config file.
- Database.OpenConnectionString(connectionString): Opens database using connection string.
- Database.Query(sqlstatement[,parametets]) : this is similar to Executes but returns results as a collection.
- Database.QueryValue(SQLstatement [,parameters]): executes sql statement and return a single value.

### 10. Asp.NET Helpers
- Components that can be accessed by single lines of Razor code.
- Either use built-in helpers or create own helpers using Razor syntax stored as .cshtml files.
- Some razor helpers are WebGrid, Chart, Web Image, WebMail.
- WebGrid helper simplifies way to display data by setting up an HTML table automatically, supports different formatting options, paging through data as first, next, previous, last and sorting data through column headings.
- Chart helper to display different charts with many formatting options and labels from arrays, database or from files.
- WebMail helper provides functions for sending email messages using SMTP.
- WebImage helper provides functionality to manage image in web page with features such as flip, rotate, resize, watermark.
- There are third party helpers too for email, database, multimedia, social network, navigation and web security such as Analytics(Google), Bing, Crypto, Facebook, FileUpload, GamerCard, Gravatar Object Reference, Json, LinkShare, ModelState, ObjectInfo, ReCAPTCHA, serverInfo, Twitter, Video, WebCache, WebImage.

#### 10.1 WebGrid Helper
- Example:
```
@{ 
var db = Database.Open("SmallBakery") ; 
var selectQueryString = "SELECT * FROM Product ORDER BY Name"; 
var data = db.Query(selectQueryString); 
var grid = new WebGrid(data); 
}
<html> 
<head> 
<title>Displaying Data Using the WebGrid Helper</title> 
</head> 
<body> 
<h1>Small Bakery Products</h1> 
<div id="grid"> 
@grid.GetHtml()
</div> 
</body> 
</html>
```

- WebGrid(data)	: Creates a new WebGrid object using data from a query.
- WebGrid.GetHtml(): Renders markup to display data in an HTML table.
- WebGrid.Pager(): Renders a pager for the WebGrid object.

#### 10.2. Chart Helper
- Create standard charts like area charts, bar charts, column charts, line charts, and pie charts, along with more specialized charts like stock charts
- Example to display a chart from an array of values:
```
@{ 
var myChart = new Chart(width: 600, height: 400) 
   .AddTitle("Employees") 
   .AddSeries(chartType: "column",
      xValue: new[] {  "Peter", "Andrew", "Julie", "Mary", "Dave" }, 
      yValues: new[] { "2", "6", "4", "5", "3" }) 
   .Write();
}
```
- new Chart creates a new chart object and sets its width and height
- the AddTitle method specifies the chart title
- the AddSeries method adds data to the chart
- the chartType parameter defines the type of chart
- the xValue parameter defines x-axis names
- the yValues parameter defines the y-axis values
- the Write() method displays the chart 

- Example to display chart from database data:
```
@{ 
var db = Database.Open("SmallBakery"); 
var dbdata = db.Query("SELECT Name, Price FROM Product"); 
var myChart = new Chart(width: 600, height: 400) 
   .AddTitle("Product Sales") 
   .DataBindTable(dataSource: dbdata, xField: "Name")
   .Write();
}
```

- the DataBindTable method binds the data source to the chart and easy to use.

- Example using AddSeries as alternative to DataBindtable as its more flexible to specify chart and data.
```
@{ 
var db = Database.Open("SmallBakery"); 
var dbdata = db.Query("SELECT Name, Price FROM Product"); 
var myChart = new Chart(width: 600, height: 400) 
   .AddTitle("Product Sales") 
   .AddSeries(chartType:"Pie",
      xValue: dbdata, xField: "Name",
      yValues: dbdata, yFields: "Price")
   .Write();
}
```

- Example to show chart from XML data:
```
@using System.Data;

@{
var dataSet = new DataSet();
dataSet.ReadXmlSchema(Server.MapPath("data.xsd"));
dataSet.ReadXml(Server.MapPath("data.xml"));
var dataView = new DataView(dataSet.Tables[0]);

var myChart = new Chart(width: 600, height: 400)
   .AddTitle("Sales Per Employee")
   .AddSeries("Default", chartType: "Pie",
      xValue: dataView, xField: "Name",
      yValues: dataView, yFields: "Sales")
   .Write();}
}
```

#### 10.2. WebMail Helper
- Easily send emails from web application using SMTP(Simple Mail Transfer Protocol).
- To send an email, first put following code to initiate Webmail helper and put its properties to AppStart page.
- _AppStart.cshtml:
```
@{
WebSecurity.InitializeDatabaseConnection("Users", "UserProfile", "UserId", "Email", true);  //this can be optional
WebMail.SmtpServer = "smtp.example.com";
WebMail.SmtpPort = 25;
WebMail.EnableSsl = false;
WebMail.UserName = "support@example.com";
WebMail.Password = "password-goes-here";
WebMail.From = "john@example.com";
}
```

- Explaining Properties:
- ______
- SmtpServer: The name the SMTP server that will be used to send the emails.
- SmtpPort: The port the server will use to send SMTP transactions (emails).
- EnableSsl: True, if the server should use SSL (Secure Socket Layer) encryption.
- UserName: The name of the SMTP email account used to send the email.
- Password: The password of the SMTP email account.
- From: The email to appear in the from address (often the same as UserName).
- _____
- Then secondly, create input page with name like Email_Form.cshml to collect info, and submit to new email send page.
-  Email_Form.cshml:
```
<!DOCTYPE html>
<html>
<body>
<h1>Request for Assistance</h1>

<form method="post" action="EmailSend.cshtml">
<label>Username:</label>
<input type="text" name="customerEmail" />
<label>Details about the problem:</label>
<textarea name="customerRequest" cols="45" rows="4"></textarea>
<p><input type="submit" value="Submit" /></p>
</form>

</body>
</html>
```
- Then finally, create email send page using Send method of webmail.
- SendEmailPage.cshtml:
```
@{ // Read input
var customerEmail = Request["customerEmail"];
var customerRequest = Request["customerRequest"];
try
{
// Send email
WebMail.Send(to:"someone@example.com", subject: "Help request from - " + customerEmail, body: customerRequest );
}
catch (Exception ex )
{
<text>@ex</text>
}
}
```

- Class of WebMail is System.Web.Helpers.WebMail.
- NameSpace of WebMail is System.Web.Helpers.
- Assembly is System.Web.Helpers.dll.


#### 10. 4. WebSecurity Object
- Provides security and authentication to crate user accounts, login, logout users, reset or changes passwords.
- ___________
- Properties:
- CurrentUserId: Gets the ID for the current user
- CurrentUserName: Gets the name of the current user
- HasUserId: Returns true if the current has a user ID
- IsAuthenticated: Returns true if the current user is logged in

- _______________
- Methods:
- ChangePassword(): Changes the password for a user
- ConfirmAccount(): Confirms an account using a confirmation token
- CreateAccount(): Creates a new user account
- CreateUserAndAccount(): Creates a new user account
- GeneratePasswordResetToken(): Generates a token that can be sent to as user by email
- GetCreateDate(): Gets the time the specified membership was created
- GetPasswordChangeDate(): Gets the date and time when password was changed
- GetUserId(): Gets a user ID from a user name
- InitializeDatabaseConnection(): Initializes the WebSecurity system (database)
- IsConfirmed(): Checks if a user is confirmed
- IsCurrentUser(): Checks if the current user matches a user name
- Login(): Logs the user in by setting a token in the cookie
- Logout(): Logs the user out by removing the token cookie
- RequireAuthenticatedUser(): Exits the page if the user is not an authenticated user
- RequireRoles(): Exits the page if the user is not a part of the specified roles
- RequireUser(): Exits the page if the user is not the specified user
- ResetPassword(): Changes a user's password using a token
- UserExists(): Checks if a given user exists
- _____
- To use websecurity object, need to create or initialize websecurity database.
- Put following code inside page named _AppStart.cshtml:
```
@{
WebSecurity.InitializeDatabaseConnection("UsersDB", "TBL_UserProfile", "UserId", "Email", true);
}
```

- "UsersDB" is the name of the WebSecurity database (Users.sdf).
- "TBL_UserProfile" is the name of the database table that contains the user profile information.
- "UserId" is the name of the column that contains the user IDs (primary key).
- "Email" is the name of the column that contains user names
- The last parameter true is a boolean value indicating that the user profile and membership tables should be created automatically if they don't exist, otherwise false.
- Although true indicates automatic creation of the database tables, the database itself will not be created automatically. It must exist.

- UserProfile table might have UserId, Email column.
- The Membership table will contain membership information about when the user was created and if (and when) the membership was confirmed.( Columns such as UserId, CreateDate, ConfirmationToken, IsConfirmed, LastPasswordFailure, Password, PasswordChange)

- You might get errors using the WebSecurity object, if your site is not configured to use the ASP.NET Web Pages membership system SimpleMembership.

- This can occur if a hosting provider's server is configured differently than your local server. To fix this, add the following element to the site's Web.config file:
```
<appSettings>
<add key="enableSimpleMembership" value="true" />
</appSettings>
```

 ### 11. ASP.NET Web Pages - Publishing the Website
- Use latest version of asp.net web page(here is 4.0 or 4.5).
- Copy web folders of website from developed computer to application folder of hosting server computer.
- then put same all dll files to bin folder on remote hosting computer.
```
Microsoft.Web.Infrastructure.dll
NuGet.Core.dll
System.Web.Helpers.dll
System.Web.Razor.dll
System.Web.WebPages.Administration.dll
System.Web.WebPages.Deployment.dll
System.Web.WebPages.dll
System.Web.WebPages.Razor.dll
WebMatrix.Data.dll
WebMatrix.WebData
```
- Do not copy data of test environment. just copy empty .sdf file from your development computer to server after deleing everything in database if have to copy an sql database to avoid overwrite production data.


