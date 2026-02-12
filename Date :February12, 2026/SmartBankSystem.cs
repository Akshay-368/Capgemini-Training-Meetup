namespace SmartBankSystem;
using System;
using System.Collections.Generic;
public interface ICreditCardEvaluate
{
    bool ValidateCustomerDetails ( int age , string employmentType , double monthlyIncome , double dues , int creditscore , int defaults ) ;
    double CalculateCreditLimit(double monthlyIncome, double dues, int creditScore, int defaults);
}

// Creating the exception class
public class InvalidCreditCardDetailsException : Exception
{
    public InvalidCreditCardDetailsException(string message) : base(message) { }
}

public abstract class CreditRiskProcessorBase : ICreditCardEvaluate
{
    public virtual bool ValidateCustomerDetails(int age, string employmentType, double monthlyIncome, double dues, int creditScore, int defaults)
    {
        if ( age < 21 || age > 65 ) throw new InvalidCreditCardDetailsException("Invalid age");

        if (employmentType != "Salaried" && employmentType != "Self-Employed") throw new InvalidCreditCardDetailsException("Invalid employment type");

            if (monthlyIncome < 20000) throw new InvalidCreditCardDetailsException("Invalid monthly income");

            if (dues < 0) throw new InvalidCreditCardDetailsException("Invalid credit dues");

            if (creditScore < 300 || creditScore > 900) throw new InvalidCreditCardDetailsException("Invalid credit score");

            if (defaults < 0) throw new InvalidCreditCardDetailsException("Invalid default count");
        return true;
    }

    // Abstract method to be implemented by child class
    public abstract double CalculateCreditLimit(double monthlyIncome, double dues, int creditScore, int defaults);
}

public class SmartBankProcessor : CreditRiskProcessorBase
    {

        public override double CalculateCreditLimit ( double monthlyIncome , double dues , int creditScore, int defaults )
        {
            double debtRatio = dues / (monthlyIncome * 12);

            // for Low Risk
            if (creditScore >= 750 && defaults == 0 && debtRatio < 0.25)
            {
                return 300000;
            }

            // for High Risk
            if (creditScore < 600 || defaults >= 3 || debtRatio > 0.4)
            {
                return 50000;
            }

            // Default Medium Risk
            return 150000;
        }
    }