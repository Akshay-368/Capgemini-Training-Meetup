// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
// Build using dotnet new console -n Playground
// Run using dotnet run
using System;
using SmartBankSystem;

/*

SmartBank – Customer Credit Risk Evaluation System
SmartBank is a banking platform that evaluates customer financial 
stability and determines the maximum credit limit that can be offered.

The system validates customer financial data and applies risk-based calculations.

Robust custom exception handling is used to handle invalid inputs.
Problem Scenario
The UserInterface class accepts the following customer details:

Customer Name
Age
Employment Type
Monthly Income
Existing Credit Card Dues
Credit Score
Number of Loan Defaults
These details are passed to a utility class named CreditRiskProcessor.
Component Specification
1. CreditRiskProcessor (Utility Class)
TypeMethodParametersResponsibilitiesClassvalidateCustomerDetailsint age, 
String employmentType, double monthlyIncome, double dues, int creditScore,
 int defaultsValidates customer inputs. Returns true if valid; 
 otherwise throws InvalidCreditDataException.ClasscalculateCreditLimitdouble 
 monthlyIncome, double dues, int creditScore, int defaultsCalculates and
  returns the customer credit limit.Validation Rules
The validateCustomerDetails() method must validate input according to 
the following rules:

Age
Must be between 21 and 65 (inclusive)
If invalid → throw exception with message:
"Invalid age"
Employment Type
Must be "Salaried" or "Self-Employed" (case-sensitive)
If invalid → throw exception with message:
"Invalid employment type"
Monthly Income
Must be greater than or equal to ₹20,000
If invalid → throw exception with message:
"Invalid monthly income"
Existing Credit Card Dues
Must be greater than or equal to 0
If invalid → throw exception with message:
"Invalid credit dues"
Credit Score
Must be between 300 and 900
If invalid → throw exception with message:
"Invalid credit score"
Number of Loan Defaults
Must be greater than or equal to 0
If invalid → throw exception with message:
"Invalid default count"
Credit Limit Calculation Rules
The calculateCreditLimit() method calculates the credit limit 
based on risk category.

Risk Assessment Criteria
Debt Ratio
 
Plain Text
Debt Ratio = Existing Dues / (Monthly Income × 12)
 
Risk Categories
High Risk
Credit Score < 600
OR Defaults ≥ 3
OR Debt Ratio > 0.4
Credit Limit = ₹50,000
Medium Risk
Credit Score between 600 and 749
OR Defaults = 1 or 2
Credit Limit = ₹1,50,000
Low Risk
Credit Score ≥ 750
AND Defaults = 0
AND Debt Ratio < 0.25
Credit Limit = ₹3,00,000
2. InvalidCreditDataException
TypeResponsibilitiesClassCustom checked exception extending Exception
Constructor
 
Plain Text
public InvalidCreditDataException(String message) 
 
This exception is thrown whenever any validation rule fails.
3. UserInterface Class
Contains the main() method
Accepts all customer details using Scanner
Calls validateCustomerDetails()
If validation succeeds:Calls calculateCreditLimit()
Displays the credit limit
If validation fails:Catches InvalidCreditDataException
Displays only the exception message
Sample Input & Output – Test Cases
Test Case 1 – Valid (Low Risk)
Input
 
Plain Text
Enter customer name: Arjun Enter age: 35 
Enter employment type: Salaried 
Enter monthly income: 80000 
Enter existing credit dues: 100000 
Enter credit score: 820 
Enter number of loan defaults: 0 
 
Output
 
Plain Text
Customer Name: Arjun Approved Credit Limit: ₹300000 
 
Test Case 2 – Valid (Medium Risk)
Input
 
Plain Text
Enter customer name: Neha Enter age: 42 
Enter employment type: Self-Employed 
Enter monthly income: 60000 
Enter existing credit dues: 200000 
Enter credit score: 690 
Enter number of loan defaults: 1 
 
Output
 
Plain Text
Customer Name: Neha Approved Credit Limit: ₹150000 
 
Test Case 3 – Valid (High Risk)
Input
 
Plain Text
Enter customer name: Rakesh Enter age: 50 
Enter employment type: Salaried 
Enter monthly income: 45000 
Enter existing credit dues: 300000 
Enter credit score: 560 
Enter number of loan defaults: 3 
 
Output
 
Plain Text
Customer Name: Rakesh Approved Credit Limit: ₹50000 
 
Test Case 4 – Invalid Age
Input
 
Plain Text
Enter customer name: Pooja 
Enter age: 19 
Enter employment type: Salaried 
Enter monthly income: 50000 
Enter existing credit dues: 0 
Enter credit score: 720 
Enter number of loan defaults: 0 
 
Output
 
Plain Text
Invalid age
 
Test Case 5 – Invalid Employment Type
Input
 
Plain Text
Enter customer name: Sandeep Enter age: 40 
Enter employment type: Contract 
Enter monthly income: 70000 
Enter existing credit dues: 100000 
Enter credit score: 780 
Enter number of loan defaults: 0 
 
Output
 
Plain Text
Invalid employment type 
 
Test Case 6 – Invalid Monthly Income
Input
 
Plain Text
Enter customer name: Anjali Enter age: 30 
Enter employment type: Salaried 
Enter monthly income: 15000 
Enter existing credit dues: 50000 
Enter credit score: 750 Enter number of loan defaults: 0 
 
Output
 
Plain Text
Invalid monthly income
 
Test Case 7 – Invalid Credit Score
Input
 
Plain Text
Enter customer name: Mohit Enter age: 45 
Enter employment type: Self-Employed 
Enter monthly income: 90000 
Enter existing credit dues: 200000 
Enter credit score: 950 Enter number of loan defaults: 0 
 
Output
 
Plain Text
Invalid credit score
 
Test Case 8 – Invalid Default Count
Input
 
Plain Text
Enter customer name: Rina Enter age: 37 
Enter employment type: 
Salaried Enter monthly income: 65000 
Enter existing credit dues: 120000 
Enter credit score: 740 Enter number of loan defaults: -1 
 
Output
 
Plain Text
Invalid default count
 


*/


public class Program
{
    public static void Main(string[] args)
    {
        try
            {
                // Input
                Console.Write("Enter customer name: ");
                string name = Console.ReadLine()!;

                Console.Write("Enter age: ");
                int age = int.Parse(Console.ReadLine())!;

                Console.Write("Enter employment type: ");
                string empType = Console.ReadLine();

                Console.Write("Enter monthly income: ");
                double income = double.Parse(Console.ReadLine());

                Console.Write("Enter existing credit dues: ");
                double dues = double.Parse(Console.ReadLine());

                Console.Write("Enter credit score: ");
                int score = int.Parse(Console.ReadLine());

                Console.Write("Enter number of loan defaults: ");
                int defaults = int.Parse(Console.ReadLine());

                // Create instance using the Interface (Polymorphism)
                ICreditCardEvaluate processor = new SmartBankProcessor();

                // Process and Display
                if (processor.ValidateCustomerDetails(age, empType, income, dues, score, defaults))
                {
                    double limit = processor.CalculateCreditLimit(income, dues, score, defaults);
                    

                    Console.WriteLine($"Customer Name: {name}");
                    Console.WriteLine($"Approved Credit Limit: {limit}");

                }
            }
            catch (InvalidCreditCardDetailsException ex)
            {
                // Only for  the specific messg as per question
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                // unexpected format errors like letters in age
                Console.WriteLine("Input error. Please enter numbers correctly.");
            }
        }
}
