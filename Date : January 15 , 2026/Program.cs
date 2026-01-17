// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
using System;
using Task;
// Created using dotnet new console -n Playground
class Program
{
    static void Main()
    {
        Task.Task.FunctionAdd();
        Task.Task.EvenOrOdd() ;
        Task.Task.NegativeOrPositive() ;

        // Demo of Compile Time Polymorphism with Method OverLoading
        Task.Task.LoadingPolymorphism obj = new Task.Task.LoadingPolymorphism() ;
        obj.Example ( 5 ) ;
        obj.Example ( " Hello " ) ;
        obj.Example ( 10 , " World " ) ;

        // Demo of Abstract Class and Method Overriding which is Run Time Polymorphism
        Task.Task.ExampleAbstract example = new Task.Task.ChildClass() ;
        example.AbstractMethod() ; 

        // Now running the Litware Employee Salary Calculation given in the session
        LitwareLib.Employee emp = new LitwareLib.Employee() ;
        Console.WriteLine ( " Employee Salary Details " ) ;
        emp.AcceptEmployeeDetails ( 101 , " Akshay " , 12000 ) ;
        emp.CalculateSalary() ;
        emp.DisplayDetails() ;
    }
}
