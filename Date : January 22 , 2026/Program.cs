// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System;
using Task ;

public static class Program
{
    public static bool IsPalindrome(this string str)
            {
                int left = 0;
                int right = str.Length - 1;
                while (left < right)
                {
                    if (str[left] != str[right])
                        return false;
                    left++;
                    right--;
                }
                return true;
            }
        }

    public static class StringExtensions
        {
            
    public static void Main ( string[] args )
    {
        DemoStringAndStringBuilder demo = new DemoStringAndStringBuilder();
        demo.Run();

        // Now some viva questions and asnwers :
        // What are extension methods in C#?
        // Extension methods in C# are static methods that allow you to add new methods to existing types without modifying the original type or creating a new derived type. They are defined in static classes and use the "this" keyword in the first parameter to specify the type they extend.
        // Example of extension method:
        
        // The difference between String and StringBuilder is that String is immutable, meaning once created, it cannot be changed.
        // Boxing vs Unboxing in C#:
        // Boxing is the process of converting a value type (like int, char, etc.) to a reference type (object). This involves wrapping the value type in an object so it can be treated as an object.
        // Unboxing is the reverse process, where the object is converted back to a value type. This requires an explicit cast.
        // Method Overloading in C#:
        // Method overloading is a feature in C# that allows multiple methods to have the same
        // name but differ in the type or number of parameters. The correct method is chosen at compile time based on the arguments passed.
        // Method Overriding in C#:
        // Method overriding is a feature that allows a derived class to provide a specific implementation of a method that is already defined in its base class. The method in the derived class must have the same name, return type, and parameters as the method in the base class
        // What is interface and why we use it ?
        // Differenvce between var and dynamic in C#:
        //  The difference between var and dynamic in C# is that var is statically typed, meaning the type is determined at compile time and cannot change, while dynamic is dynamically typed, meaning the type is determined at runtime and can change.
        // Example of var and dynamic:
        var staticVar = 10; // statically typed as int
        dynamic dynamicVar = 10; // dynamically typed
        Console.WriteLine("Static Var Type: " + staticVar.GetType());
        Console.WriteLine("Dynamic Var Type: " + dynamicVar.GetType());
        dynamicVar = "Now I'm a string"; // changing type at runtime
        Console.WriteLine("Dynamic Var New Type: " + dynamicVar.GetType());
        // when to use var and when to use dynamic ?
        // Use var when the type is known at compile time and you want type safety. Use dynamic when you need flexibility and the type may change at runtime, but be cautious as it can lead to runtime errors.
        // General structure of a C# program
    }
}
