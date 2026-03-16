using System;
using System.Runtime.CompilerServices;
namespace Task
{
    public class Task
    {
        public static void FunctionAdd()
        {
            Console.WriteLine("Function A is called. Now please enter two numbers to add:");
            int a = int.Parse(Console.ReadLine());
            int b = Convert.ToInt32(Console.ReadLine());
            int sum = a + b;
            Console.WriteLine("Sum: " + sum);
        }

        public static void EvenOrOdd()
        {
            Console.WriteLine ( " Please enter a number to check even or odd " ) ;
            int input = Convert.ToInt32 ( Console.ReadLine() ) ;
            if ( ( input % 2 ) == 0 ) {
                Console.WriteLine( "Even " ) ;
            }else
            {
                Console.WriteLine ( " Odd");
            }
        }

        public static void NegativeOrPositive()
        {
            Console.WriteLine(" Please enter a number to check negative or positive ");
            int number = Convert.ToInt32(Console.ReadLine());
            if (number < 0)
            {
                Console.WriteLine("Negative");
            }
            else
            {
                Console.WriteLine("Positive");
            }
        }

        public class LoadingPolymorphism
        {
            // Making the class public so that we can acccess it from outside of this Task.cs file in the Program.cs file which is teh entry point of the application
            public void Example(int a)
            {
                Console.WriteLine("Integer: " + a);
            }

            public void Example(string b)
            {
                Console.WriteLine("String: " + b);
            }

            public void Example ( int a , string b )
            {
                Console.WriteLine ( " Integer: " + a + " String: " + b ) ;
            }
        }

        public abstract class ExampleAbstract
        {
            
            public abstract void AbstractMethod();

            // let's now think about a real life example of abstract class and method overriding
            // Let's use an example of a hospital management system
            // We can use an abstract class called "Employee" which will have an abstract method called "CalculateSalary"
            // So considering this ExampleAbstract class as Employee class

            protected abstract void CalclateSalary(); // making it as private so that it can only be accessed within the class
        }

        public class ChildClass : ExampleAbstract
        {
            public override void AbstractMethod()
            {
                Console.WriteLine ( " Implementation of Abstract Method in Child Class ") ;
            }

            protected override void CalclateSalary()
            {
                Console.WriteLine(" Calculating Salary in Child Class ");
            }
        }

        public class AnotherChildClass : ExampleAbstract
        {
            public override void AbstractMethod()
            {
                Console.WriteLine(" Another Implementation of Abstract Method in Another Child Class ");
            }

            protected override void CalclateSalary()
            {
                Console.WriteLine(" Calculating Salary in Another Child Class which  is only concerned with one particular type of employees ");
                Console.WriteLine(" For Example: Interns who are paid hourly basis ");
                
            }
        }
    }
}