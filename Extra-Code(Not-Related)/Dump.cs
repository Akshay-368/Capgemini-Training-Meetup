namespace Testing; // Note: actual namespace depends on the project name
using System ;
using System.Collections.Generic;
using System.Text.Json;
// inspired by LINQPad signature method. - Dump()
public static class DumpExtensions
{
    public static void Dump<T>(this T obj, string label = null)
    {
        if (!string.IsNullOrEmpty(label))
            Console.WriteLine($"{label}:");

        // Pretty-print JSON representation
        var options = new JsonSerializerOptions { WriteIndented = true , IncludeFields=true };
        string json = JsonSerializer.Serialize(obj, options);
        Console.WriteLine(json);
    }

    public static T Dump1<T> ( this T obj , string label = null )
    {
        if ( !string.IsNullOrEmpty(label)) Console.WriteLine($"{label}:");

        var options = new JsonSerializerOptions {WriteIndented = true} ;
        string json = JsonSerializer.Serialize(obj , options ) ;
        Console.WriteLine(json) ;
        
        return obj ;
    }
    // By making it return the obj , i can now do the chaining of the methods.
    // Like this : obj.Dump1("Value of a : ").Dump1("Value of b : ")
    // which would not be possible with the previous method which returns void
}

public class TestClass
{
    public int A { get; set; }
    public int B { get; set; }
    public int C = 10;
}
public class Program
{


    public static void Main()
    {
        "Hello World!".Dump();
        int a = 10 ;
        a.Dump();
        int b = 20 ;
        b.Dump();
        (a + b).Dump();
        List<int> list = new List<int>();
        list.Add(11);
        list.Add(12);
        list.Dump("Value of the list : ");
        Test(a,b).Dump("Value of the sum : ");

        TestClass t = new TestClass();
        t.A = 10 ;
        t.B = 20 ;
        t.Dump("Value of the TestClass : ") ;


        // int a = 10 ;
        // a.Dump1("Value of a : ");
        // int b = 20 ;
        // b.Dump1( " Value of B : ");
        // (a + b).Dump1();
        // List<int> list1 = new List<int>();
        // list1.Add(a);
        // list1.Add(b);
        // list1.Dump1();
        // Test(a,b).Dump1();
    }

    public static int Test(int a , int b )
    {
        return a + b ;
    }

}
