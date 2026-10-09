### 1. Intro
- C# or C-sharp is programming language developed by Microsoft that runs on .NET framework.
- Used to develop web apps, desktop apps, mobile apps, games and more.
- Example:
```
using System;

namespace Something
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hellow World");
		}
	}
}
```

- OOP, has roots from C family, first version released in 2002 and latest version C#14 in Nov2025.
- Namespace is container for classes and other namespaces and used to organize code. Eg. System
- Classes is container for data and methods. Eg. Console is class of System namespace.
- C# is case-sensitive, so MyClass and myclass are different.
- In C#, file name do not need to match class name like Java.
- To pring values, use WriteLine() method. 
- WriteLine() adds a new line for each one while Write() does not.
- Single line comments start with two forward slashes(//) and multiple line comment starts with /* and ends with */.

### 2. Variables
- Containers for storing data values.
- int, double, char, string, bool, etc. are types for variable.
- Syntax: 	``` datatype variableName = value;```

- Constants is unchangeable and read-only
- Eg. const int pie = 1.234555;

- Identigies are unique names as all variable must be identified with unique names.
- Eg. int daysInWeek = 7 is unique and good but int d =7 is also ok but not understandable

- Rules for naming variables are:
- -Names can contain letters, digits and the underscore character (_)
- -Names must begin with a letter or underscore
- -Names should start with a lowercase letter, and cannot contain whitespace
- -Names are case-sensitive ("myVar" and "myvar" are different variables)
- -Reserved words (like C# keywords, such as int or double) cannot be used as names

### 3. Data types
- It defines size and type of variables.
- Integer type store whole numbers both positive or negative. Int and long.
- int store whole numbers from -2147483648 to 2147483647.
- long can store whole numbers from -9223372036854775808 to 9223372036854775807. Eg. long myNum = 1234567891011L; //put L at the end
- Floating point types are fractional numbers with decimals. float and double.
- float to store small fractional numbers ending with F letter. Eg. float num = 99.9F. Float can have six or seven decimal digits
- double for large fractional numbers ending with D letter. Eg. double num = 123233.11D. double can have upto 15 digits precision.
- Booleans are true or false.
- Charactes for storing single character. Eg. char grad = 'A';
- String to store sequence of characters or text. Eg. string myName = "Ameer";

#### 3.1. C# Type Casting
- Its to assign a value of one data type to another type.
- Two types: Implicit Casting(automatically) and Explicit Casting(manually)
- Implicit Casting converts smaller type to larger type size.  char > int > long > float > double
```
int intType =10;
double doubleType = intType; //Automaticalyy casts int to double
```

- Explicit casting convert larger type to smaller size type. double > float > long > int > char
```
double doubleData = 99.9;
int intData = (int) doubleData; //manual casting double to int
```
- there are explicit built-in type conversion methods such as Convert.ToBoolean, Convert.ToDouble, Convert.ToString, Convert.ToInt32 (int) and Convert.ToInt64 (long):

- Eg: Take user input
```
console.WriteLine("Enter name:");
string name = Console.ReadLine(); //user input from keyboard of only string data type, not integer
Console.WriteLine("Name is:" + name);

//example of type conversion here
Console.WriteLine("Enter age");
int age = Convert.ToInt32(Console.ReadLine()); //as readline returns string, age of int gives error so conversion needed
```
