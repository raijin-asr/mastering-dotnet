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


### 4 C# Operators
- Arithmetic Operators perform mathematical operations such as Addition, subtraction, multiplication, division, modulus, Increment, decrement.
- Assignment operators(=) to assign values to variables such as =, +=, -=, *=, /=, &=, !=, , >>=, <<=.
- Comparison operators used to compare two values or variables such as ==, !=, >, <, >=, <=.
- Logical Operators to test for True or False values such as &&, ||, !.

### 5. Math
- C# Math has many methods to perform mathematical tasks on numbers.
- Math.Max(x,y) to find highest values of x and y.
- Math.Min(x,y) to find lowest value of x and y.
- Math.Sqrt(x) returns square root of x.
- Math.Abs(x) returns positive value of x.
- Math.Round() rounds number to nearest whole number.

### 6. C# String
- Stores text or collection of characters surrounded by double quotes.
- String length is property of string to find its length. ToUpper() and ToLower() are other methods.
- String Concatenation combine two strings using + symbol or string.Concat() method.
- String Interpolation is another string concatenation option which substitute values of variables into placeholders in a string using $ sign..
- Eg. string name = $"My full name is: {firstName} {lastName}";
- Access strings using square bracket [] referrring to its index. Eg. string[0]. Or find specific index postion of character using IndexOf().
- Another method to access extracts characters from a stirng is using Substring(). Eg. name.Substring(name.IndexOf('e'));
- C# special characters are written using backslash(\) escape character that turns special character into string. Eg. "I am \"special\" character".
- Other useful escape characters are \n(New Line), \t(tab), \b(backspace).

### 7. If..Else Statement
- If, else, else if are conditonal statements.
- a. If(condition){} else {},and b. If(condition1){}else if (condition2){} else {}.
- Short hand of If else is ternary operator. Eg. variable = (condition) ? expressiontrue: expresionFalse;

### 8. Switch Statement
- To select one of many code blocks to be execute.
```
switch(expression) 
{
  case x:
    // code block
    break;
  case y:
    // code block
    break;
  default:
    // code block
    break;
}
```
### 9. While Loop & Do while loop
- While loop Execute loop as long as specified condition is met.
```
int i = 0;
while (i < 5) 
{
  Console.WriteLine(i);
  i++;
}
```

- Do while loop execute code block once and then repeat loop till condition is true.
```
int i = 0;
do 
{
  Console.WriteLine(i);
  i++;
}
while (i < 5);
```

### 10. For loop
- Use For loop when you know times to loop the code block.
```
for (statement 1; statement 2; statement 3) 
{
  // code block to be executed
}
```

- Use foreach loop for array.
```
//Syntax
foreach (type variableName in arrayName) 
{
  // code block to be executed
}
```

```
//Example
string[] cars = {"Volvo", "BMW", "Ford", "Mazda"};
foreach (string i in cars) 
{
  Console.WriteLine(i);
}
```

### 11. Array
- Stores multiple values in a single variable instead of declaring separate variables for each value. Eg. string[] names;
```
//Access elements of Array
string[] cars = {"Volvo", "BMW", "Ford", "Mazda"};
Console.WriteLine(cars[0]);
```
- Using 'new' keyword is another way to create array.
```
// Create an array of four elements, and add values later
string[] cars = new string[4];

// Create an array of four elements and add values right away 
string[] cars = new string[4] {"Volvo", "BMW", "Ford", "Mazda"};

// Create an array of four elements without specifying the size 
string[] cars = new string[] {"Volvo", "BMW", "Ford", "Mazda"};

// Create an array of four elements, omitting the new keyword, and without specifying the size
string[] cars = {"Volvo", "BMW", "Ford", "Mazda"};
```

- However, you should note that if you declare an array and initialize it later, you have to use the new keyword
```
// Declare an array
string[] cars;

// Add values, using new
cars = new string[] {"Volvo", "BMW", "Ford"};

// Add values without using new (this will cause an error)
cars = {"Volvo", "BMW", "Ford"};
```

- Loop through array using for loop and length property.
```
string[] cars = {"Volvo", "BMW", "Ford", "Mazda"};
for (int i = 0; i < cars.Length; i++) 
{
  Console.WriteLine(cars[i]);
}
```

- Another way is to use foreach loop.
```
string[] cars = {"Volvo", "BMW", "Ford", "Mazda"};
foreach (string i in cars) 
{
  Console.WriteLine(i);
}
```

- To sort arrays alphabetically in ascending order, use Sort() method. Eg. Array.Sort(cars);
- Other useful array methods, such as Min, Max, and Sum, can be found in the System.Linq namespace
```
using System;
using System.Linq;

namespace MyApplication
{
  class Program
  {
    static void Main(string[] args)
    {
      int[] myNumbers = {5, 1, 8, 9};
      Console.WriteLine(myNumbers.Max());  // returns the largest value
      Console.WriteLine(myNumbers.Min());  // returns the smallest value
      Console.WriteLine(myNumbers.Sum());  // returns the sum of elements
    }
  }
}
```
- C# Multidimensional Arrays stores data as a tabular form, like a table with rows and columns.
- To create 2D array, add each array within its own set of curly braces, and insert a comma (,) inside the square brackets:
- Eg. int[,] numbers = { {1, 4, 2}, {3, 6, 8} };
- The single comma [,] specifies that the array is two-dimensional. A three-dimensional array would have two commas: int[,,].
- To access an element of a two-dimensional array, you must specify two indexes: one for the array, and one for the element inside that array.
- Eg. Console.WriteLine(numbers[0, 2]);
```
//Change Elements of a 2D Array

int[,] numbers = { {1, 4, 2}, {3, 6, 8} };
numbers[0, 0] = 5;  // Change value to 5
Console.WriteLine(numbers[0, 0]); // Outputs 5 instead of 1
```

```
//Loop through 2D array
int[,] numbers = { {1, 4, 2}, {3, 6, 8} };

foreach (int i in numbers)
{
  Console.WriteLine(i);
} 
```

- For looping through multidimensional arrays, one loop for each of array dimensions is needed and GetLength() is used instead of Length.
```
int[,] numbers = { {1, 4, 2}, {3, 6, 8} };

for (int i = 0; i < numbers.GetLength(0); i++) 
{ 
  for (int j = 0; j < numbers.GetLength(1); j++) 
  { 
    Console.WriteLine(numbers[i, j]); 
  } 
} 
```


