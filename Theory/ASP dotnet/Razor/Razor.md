### 1. Razor 
- It is not Programming language but a server side markup language.
- Its markup syntax to embed server based code such as vb and c# into web pages.
- Based on ASP.Net designed for creating web application.

#### 1.1. Syntax:
- Has similar syntax like PHP and Classic ASP.
- It supports both C# and VB.
```
<ul>
@for (int i = 0; i < 10; i++) {
<li>@i</li>
}
</ul>
```

#### 1.2 Syntax for C#:
-  Razor code blocks are enclosed in @{ ... }
- Inline expressions (variables and functions) start with @
- Code statements end with semicolon
- Variables are declared with the var keyword
- Strings are enclosed with quotation marks
- C# code is case sensitive
- C# files have the extension .cshtml

```
<!-- Single statement block -->
@{ var myMessage = "Hello World"; }

<!-- Inline expression or variable -->
<p>The value of myMessage is: @myMessage</p>

<!-- Multi-statement block -->
@{
var greeting = "Welcome to our site!";
var weekDay = DateTime.Now.DayOfWeek;
var greetingMessage = greeting + " Here in Huston it is: " + weekDay;
}
<p>The greeting is: @greetingMessage</p>
```

#### 1.3 Syntax for VB
- Razor code blocks are enclosed in @Code ... End Code
- Inline expressions (variables and functions) start with @
- Variables are declared with the Dim keyword
- Strings are enclosed with quotation marks
- VB code is not case sensitive
- VB files have the extension .vbhtml

```
<!-- Single statement block  --> 
@Code dim myMessage = "Hello World" End Code
 
<!-- Inline expression or variable --> 
<p>The value of myMessage is: @myMessage</p> 
 
<!-- Multi-statement block --> 
@Code
dim greeting = "Welcome to our site!" 
dim weekDay = DateTime.Now.DayOfWeek 
dim greetingMessage = greeting & " Here in Huston it is: " & weekDay
End Code

<p>The greeting is: @greetingMessage</p>
```

#### 1.4 Reading User Input
- Input is read by the Request[] function, and posting (input) is tested by the IsPost condition:


```
@{
var totalMessage = "";
if(IsPost)
    {
    var num1 = Request["text1"];
    var num2 = Request["text2"];
    var total = num1.AsInt() + num2.AsInt();
    totalMessage = "Total = " + total;
    }
}
<html>
<body style="background-color: beige; font-family: Verdana, Arial;">
<form action="" method="post">
<p><label for="text1">First Number:</label><br>
<input type="text" name="text1" /></p>
<p><label for="text2">Second Number:</label><br>
<input type="text" name="text2" /></p>
<p><input type="submit" value=" Add " /></p>
</form>
<p>@totalMessage</p>
</body>
</html>
```

#### 1.5. Razor Helpers
- Helpers are components accessed by single lines of Razor code.
- Some helpers are Web Grid, Web Graphics, Google Analytics, Facebook Integration, Twitter Integration, Email and Validation.


### 2.  Variables
- Used to store data.
```
// Using the var keyword:
var greeting = "Welcome to W3Schools";
var counter = 103;
var today = DateTime.Today;

// Using data types:
string greeting = "Welcome to W3Schools";
int counter = 103;
DateTime today = DateTime.Today;
```

### 2.1 Converting Data Types
- AsInt() / IsInt(): Converts a string to an integer.	 Eg: if (myString.IsInt()) / {myInt=myString.AsInt();}
- AsFloat() / IsFloat(): Converts a string to a floating-point number. Eg: if (myString.IsFloat()) / {myFloat=myString.AsFloat();}
- AsDecimal() / IsDecimal(): Converts a string to a decimal number. Eg: if (myString.IsDecimal()) {myDec=myString.AsDecimal();}
- AsDateTime() / IsDateTime() : Converts a string to an ASP.NET DateTime type. Eg: myString="10/10/2012"; myDate=myString.AsDateTime();
- AsBool() / IsBool(): Converts a string to a Boolean. Eg: myString="True"; myBool=myString.AsBool();
- ToString(): Converts any data type to a string. Eg: myInt=1234; myString=myInt.ToString();

### 3. C# Loops and Arrays

- For Loops
```
<html>
<body>
@for(var i = 10; i < 21; i++)
    {<p>Line @i</p>}
</body>
</html>
```

- For Each Loops used with array.
```
<html>
<body>
<ul>
@foreach (var x in Request.ServerVariables)
    {<li>@x</li>}
</ul>
</body>
</html>
```

- While Loops
```
<html>
<body>
@{
var i = 0;
while (i < 5)
    {
    i += 1;
    <p>Line @i</p>
    }
}
</body>
</html>
```

- An array is useful when you want to store similar variables but don't want to create a separate variable for each of them.
```
@{
string[] members = {"Jani", "Hege", "Kai", "Jim"};
int i = Array.IndexOf(members, "Kai")+1;
int len = members.Length;
string x = members[2-1];
}
<html>
<body>
<h3>Members</h3>
@foreach (var person in members)
{
<p>@person</p>
}
<p>The number of names in Members are @len</p>
<p>The person at position 2 is @x</p>
<p>Kai is now in position @i</p>
</body>
</html>
```

### 4. C# Conditions
- Execute code based on conditions.
- If, If Else, Else If
```
@{var price=20;}
<html>
<body>
@if (price>30)
  {
  <p>The price is too high.</p>
  }
else
  {
  <p>The price is OK.</p>
  }
</body>
</html>
```
- Switch:
```
@{
var weekday=DateTime.Now.DayOfWeek;
var day=weekday.ToString();
var message="";
}
<html>
<body>
@switch(day)
{
case "Monday":
    message="This is the first weekday.";
    break;
case "Thursday":
    message="Only one day before weekend.";
    break;
case "Friday":
    message="Tomorrow is weekend!";
    break;
default:
    message="Today is " + day;
    break;
}
<p>@message</p>
</body>
</html>
```



