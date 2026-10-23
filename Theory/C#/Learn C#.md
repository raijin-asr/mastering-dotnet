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

### 12. Methods
- block of code which only runs when called, also known as functions and made to reuse code.
```
class Program {
static void MyMethod() 
{
  Console.WriteLine("I just got executed!");
}

static void Main(string[] args)
{
  MyMethod();
}
}
```
- static means that the method belongs to the Program class and not an object of the Program class.
- Parameters act as variables inside method and arguments are passed to methods.
```
static void MyMethod(string fname, int age) 
{
  Console.WriteLine(fname + " is " + age);
}

static void Main(string[] args)
{
  MyMethod("RAM", 5);

}
```
- fname and age are parameters and "RAM", 5 are arguments.
- default parameter value can be passed using equal sign and also known as optional parameter.
- void keyword before method Name means no value returned and return keyword means returns some data type value.
- C# Named Arguments make possible to send arguments with key:value syntax and order of arguments does not matter.
```
static void MyMethod(string child1, string child2, string child3) 
{
  Console.WriteLine("The youngest child is: " + child3);
}

static void Main(string[] args)
{
  MyMethod(child3: "John", child1: "Liam", child2: "Liam");
}

// The youngest child is: John
```

- Method overloading means multiple method can have same name with different parameters.
```
static int PlusMethod(int x, int y)
{
  return x + y;
}

static double PlusMethod(double x, double y)
{
  return x + y;
}

static void Main(string[] args)
{
  int myNum1 = PlusMethod(8, 5);
  double myNum2 = PlusMethod(4.3, 6.26);
  Console.WriteLine("Int: " + myNum1);
  Console.WriteLine("Double: " + myNum2);
}
```

### 13. OOP
- OOP is about creating objects having both data and methods and made for building scalable, reusable, and maintainable DRY code.
- Class is template and object is its instance.
```
class Car 
{
  string color = "red";  //field or attribute

  static void Main(string[] args)
  {
    Car myObj = new Car();
    Console.WriteLine(myObj.color);
  }
}
```

- Class members are fields and method inside class.

#### 13.1. Constructor
- Special method used to initialize objects and its called when object created and used to set initial values for fields.
```
lass Car
{
  public string model;
  public string color;
  public int year;

  // Create a class constructor with multiple parameters
  public Car(string modelName, string modelColor, int modelYear)
  {
    model = modelName;
    color = modelColor;
    year = modelYear;
  }
 static void Main(string[] args)
  {
    Car Ford = new Car("Mustang", "Red", 1969);  
    Console.WriteLine(Ford.color + " " + Ford.year + " " + Ford.model);
  }
}
```
- constructor name must match the class name, and it cannot have a return type (like void or int).
- All classes have constructors by default: if you do not create a class constructor yourself, C# creates one for you.
- Constructors save time because without it you initialize 3 fields 3 times.

#### 13.2. Access Modifiers
- They are to achieve Encapsulation, so that sensitive data is hidden from users.
- Public, Private, protected and internal are access modifiers.
- public: The code is accessible for all classes
- private: The code is only accessible within the same class
- protected: The code is accessible within the same class, or in a class that is inherited from that class.
- internal: The code is only accessible within its own assembly, but not from another assembly.

#### 13.3 Properties(Get and Set)
- Sometimes we may need to access private variables fields, so it can be done with properties with get and set methods.
- Property is like combination of variable and methods to access and update value of private field.
```
class Person
{
  private string name; // field
  public string Name   // property
  {
    get { return name; }
    set { name = value; }
  }
}

class Program  //here this class is accessing private name field of class person using properties
{
  static void Main(string[] args)
  {
    Person myObj = new Person();
    myObj.Name = "Ameer";
    Console.WriteLine(myObj.Name);
  }
}
```


- Shorthand or automatic properties is "public string Name {get; set;}" and you do not need to define field.
- Encapsulation helps make code flexible and not messed up and it helps field to be made read-only using only get method and write only using only set method.


#### 13.4 Inheritance
- Inherit fields and methods by derived class of base parent class using colon(:) symbol.
- If you don't want other classes to inherit from a class, use the sealed keyword.
```
class Vehicle  // base class (parent) 
{
  public string brand = "Ford";  // Vehicle field
  public void honk()             // Vehicle method 
  {                    
    Console.WriteLine("Tuut, tuut!");
  }
}

class Car : Vehicle  // derived class (child)
{
  public string modelName = "Mustang";  // Car field
}
class Program
{
  static void Main(string[] args)
  {
    // Create a myCar object
    Car myCar = new Car();

    // Call the honk() method (From the Vehicle class) on the myCar object
    myCar.honk();

    // Display the value of the brand field (from the Vehicle class) and the value of the modelName from the Car class
    Console.WriteLine(myCar.brand + " " + myCar.modelName);
  }
}
```

#### 13.5 Polymorphism
- Polymorphism means "many forms" and uses inherited class methods to perform different task.
- In this example, same output from Base class is shown three times even its derived class has their own implementation because the base class method overrides the derived class method, when they share the same name.
```
 class Animal  // Base class (parent) 
{
  public void animalSound() 
  {
    Console.WriteLine("The animal makes a sound");
  }
}

class Pig : Animal  // Derived class (child) 
{
  public void animalSound() 
  {
    Console.WriteLine("The pig says: wee wee");
  }
}

class Dog : Animal  // Derived class (child) 
{
  public void animalSound() 
  {
    Console.WriteLine("The dog says: bow wow");
  }
}

class Program 
{
  static void Main(string[] args) 
  {
    Animal myAnimal = new Animal();  // Create a Animal object
    Animal myPig = new Pig();  // Create a Pig object
    Animal myDog = new Dog();  // Create a Dog object

    myAnimal.animalSound();
    myPig.animalSound();
    myDog.animalSound();
  }
}

//Output (which is not right, so use polymorphism to get correct output)
//The animal makes a sound
//The animal makes a sound
//The animal makes a sound
```
- C# provides an option to override the base class method, by adding the virtual keyword to the method inside the base class, and by using the override keyword for each derived class methods
```
class Animal  // Base class (parent) 
{
  public virtual void animalSound() //here base class method has virtual keyword
  {
    Console.WriteLine("The animal makes a sound");
  }
}

class Pig : Animal  // Derived class (child) 
{
  public override void animalSound()   //here child class has override keyword to override base class method
  {
    Console.WriteLine("The pig says: wee wee");
  }
}

class Dog : Animal  // Derived class (child) 
{
  public override void animalSound() 
  {
    Console.WriteLine("The dog says: bow wow");
  }
}

class Program 
{
  static void Main(string[] args) 
  {
    Animal myAnimal = new Animal();  // Create a Animal object
    Animal myPig = new Pig();  // Create a Pig object
    Animal myDog = new Dog();  // Create a Dog object

    myAnimal.animalSound();
    myPig.animalSound();
    myDog.animalSound();
  }
}

//Output:
//The animal makes a sound
//The pig says: wee wee
//The dog says: bow wow
```

### 13.6 Abstraction
- Process of hiding certain details and showing only essential info to user.
- It can be achieved with either abstract classes or interfaces.
- Abstract class is a restricted class that cannot be used to create objects (to access it, it must be inherited from another class).
- An abstract class can have both abstract and regular methods
```
abstract class Animal 
{
  public abstract void animalSound();   //abstract method
  public void sleep() 				//normal method
  {
    Console.WriteLine("meoww");
  }
}

Animal myObj = new Animal(); // Will generate an error
```
- Abstract method can only be used in an abstract class, and it does not have a body. The body is provided by the derived class (inherited from).

```
// Abstract class
abstract class Animal
{
  // Abstract method (does not have a body)
  public abstract void animalSound();
  // Regular method
  public void sleep()
  {
    Console.WriteLine("zzz");
  }
}

// Derived class (inherit from Animal)
class Pig : Animal
{
  public override void animalSound()
  {
    // The body of animalSound() is provided here
    Console.WriteLine("The pig says: wee wee");
  }
}

class Program
{
  static void Main(string[] args)
  {
    Pig myPig = new Pig(); // Create a Pig object
    myPig.animalSound();  // Call the abstract method
    myPig.sleep();  // Call the regular method
  }
}
```


### 13.7 Interface
- another way to achieve abstraction.
- is completely abstract class, only contain abstract methods and properties with empty bodies.
-  Bit does not contain fields, and by default members are abstract and public.
- It must be inherited by another class to implement just like inheritance using (:) symbol and no need to use override keyword.
```
// Interface
interface IAnimal 
{
  void animalSound(); // interface method (does not have a body)
}

// Pig "implements" the IAnimal interface
class Pig : IAnimal 
{
  public void animalSound() 
  {
    // The body of animalSound() is provided here
    Console.WriteLine("The pig says: wee wee");
  }
}

class Program 
{
  static void Main(string[] args) 
  {
    Pig myPig = new Pig();  // Create a Pig object
    myPig.animalSound();
  }
}
```

- C# does not support multiple inheritance but can be achieved with interfaces because class can implement multiple interfaces.
```
{
  void myMethod(); // interface method
}

interface ISecondInterface 
{
  void myOtherMethod(); // interface method
}

// Implement multiple interfaces
class DemoClass : IFirstInterface, ISecondInterface 
{
  public void myMethod() 
  {
    Console.WriteLine("Some text..");
  }
  public void myOtherMethod() 
  {
    Console.WriteLine("Some other text...");
  }
}

class Program 
{
  static void Main(string[] args)
  {
    DemoClass myObj = new DemoClass();
    myObj.myMethod();
    myObj.myOtherMethod();
  }
}
```

### 13.8 Enums
- Enum is special class that represents a group of constant, read-only unchangeable variables.
- Enum is short for "enumerations", which means "specifically listed".
- Use enums when you have values that you know aren't going to change, like month days, days, colors, deck of cards, etc
```
class Program
{
  enum Level
  {
    Low,
    Medium,
    High
  }
  static void Main(string[] args)
  {
    Level myVar = Level.Medium;
    Console.WriteLine(myVar);
  }
}
```

### 13.9 Files
- File class from the System.IO namespace, allows us to work with files
___
- Methods:
- AppendText(): Appends text at the end of an existing file
- Copy(): Copies a file
- Create(): Creates or overwrites a file
- Delete(): Deletes a file
- Exists(): Tests whether the file exists
- ReadAllText(): Reads the contents of a file
- Replace(): Replaces the contents of a file with the contents of another file
- WriteAllText(): Creates a new file and writes the contents to it. If the file already exists, it will be overwritten.
___
- Eg. Write to file and read it
```
using System.IO;  // include the System.IO namespace

string writeText = "Hello World!";  // Create a text string
File.WriteAllText("filename.txt", writeText);  // Create a file and write the content of writeText to it

string readText = File.ReadAllText("filename.txt");  // Read the contents of the file
Console.WriteLine(readText);  // Output the content
```

### 13.10 Exceptions and TryCatch
- Exception is thrown when error happens.
- Try Catch used to handle it where try statement has block of code to be test for errors and catch execute it catch logic when error occurs.
```
try
{
  int[] myNumbers = {1, 2, 3};
  Console.WriteLine(myNumbers[10]);
}
catch (Exception e)
{
  Console.WriteLine(e.Message);
}

//Output:
//Index was outside the bounds of the array.

//BUT without try catch, it shows
//System.IndexOutOfRangeException: 'Index was outside the bounds of the array.'
```

- Finally statement lets to execute code after try catch regardless of result.
```
try
{
  int[] myNumbers = {1, 2, 3};
  Console.WriteLine(myNumbers[10]);
}
catch (Exception e)
{
  Console.WriteLine("Something went wrong.");
}
finally
{
  Console.WriteLine("The 'try catch' is finished.");
}

//Output:
//Something went wrong.
//The 'try catch' is finished.
```

- Throw keyword allows to create a custom error.
- Its used together with an exception class like ArithmeticException, FileNotFoundException, IndexOutOfRangeException, TimeOutException.
```
static void checkAge(int age)
{
  if (age < 18)
  {
    throw new ArithmeticException("Access denied - You must be at least 18 years old.");
  }
  else
  {
    Console.WriteLine("Access granted - You are old enough!");
  }
}

static void Main(string[] args)
{
  checkAge(15);
}

//Output:
//System.ArithmeticException: 'Access denied - You must be at least 18 years old.'
```




