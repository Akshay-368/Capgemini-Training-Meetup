using System;

namespace LitwareLib
{
    public class Employee
    {
        // Data Members
        private int EmpNo;
        private string EmpName;
        private double Salary;
        private double HRA;
        private double TA;
        private double DA;
        private double PF;
        private double TDS;
        private double NetSalary;
        private double GrossSalary;

        // Method to accept input
        public void AcceptEmployeeDetails(int no, string name, double salary)
        {
            this.EmpNo = no;
            this.EmpName = name;
            this.Salary = salary;

            // Automatically trigger calculations
            CalculateSalaries();
        }

        public void CalculateSalary()
        {
            CalculateSalaries();
        }

        private void CalculateSalaries()
        {
            // Logic based on tabke

            if (Salary < 5000)
            {
                HRA = Salary * 0.10;
                TA = Salary * 0.05;
                DA = Salary * 0.15;
            }
            else if (Salary < 10000)
            {
                HRA = Salary * 0.15;
                TA = Salary * 0.10;
                DA = Salary * 0.20;
            }
            else if (Salary < 15000)
            {
                HRA = Salary * 0.20;
                TA = Salary * 0.15;
                DA = Salary * 0.25;
            }
            else if (Salary < 20000)
            {
                HRA = Salary * 0.25;
                TA = Salary * 0.20;
                DA = Salary * 0.30;
            }
            else // Salary >= 20000
            {
                HRA = Salary * 0.30;
                TA = Salary * 0.25;
                DA = Salary * 0.35;
            }

            // Gross Salary
            GrossSalary = Salary + HRA + TA + DA;

            //now let's assume standard deductions for PF and TDS which i am taking randomly
            PF = GrossSalary * 0.15; // 15 percent of Gross Salary
            TDS = GrossSalary * 0.25; // 25 percent of Gross Salary
            
            // Net Salary Calculation
            NetSalary = GrossSalary - (PF + TDS);
        }

        public void DisplayDetails()
        {
            Console.WriteLine($" Employee Statement: {EmpName} ");
            Console.WriteLine($"Employee No : {EmpNo}" );
            Console.WriteLine($"Basic Salary: {Salary}" );
            Console.WriteLine($"HRA : {HRA }");
            Console.WriteLine($"TA : {TA}");
            Console.WriteLine($"DA : {DA}");
            Console.WriteLine($"Gross Salary: {GrossSalary}" );
            Console.WriteLine($"Net Salary : {NetSalary:C}"); // it is for currency format
        }
    }
}