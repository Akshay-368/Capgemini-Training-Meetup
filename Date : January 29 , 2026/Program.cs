// See https://aka.ms/new-console-template for more information

using System;

using BikeRentalApplication;


// created using dotnet new console -n Playground
/*

    📘 Scenario
    A bike rental shop wants a simple console-based application to manage bike details such as model, brand, and price per day, 
    and also to group bikes based on their brand. This application will help the shop organize its inventory efficiently and easily 
    view bikes from the same brand.

    🛠️ Functionalities
     
    In class Program
     
     
    Plain Text
    public static SortedDictionary<int, Bike> bikeDetails
     
     
    This sorted dictionary is already provided. It stores bike details with a unique integer key.

    In class Bike, implement the below properties
     
     
    Data TypeProperty NamestringModelstringBrandintPricePerDay
     

    In class BikeUtility, implement the below methods
    Method 1
     
     
    Plain Text
    public void AddBikeDetails(string model, string brand, int pricePerDay)
     
     

    Adds the bike details (model, brand, price per day) to the bikeDetails dictionary.
    The key of the dictionary should be one more than the current number of items.
    Initially, the dictionary contains 0 items.
    Method 2
     
     
    Plain Text
    public SortedDictionary<string, List<Bike>> GroupBikesByBrand()
     
     

    Groups bikes based on their brand.
    Each brand should map to a list of bikes belonging to it.
    The grouped result should be returned as a SortedDictionary.
     

    In Program class – Main Method
    Get the required values from the user.
    Call the appropriate methods.
    Display the output exactly as shown in the Sample Input/Output.

    Sample Input/Output

    1. Add Bike Details

    2. Group Bikes By Brand

    3. Exit
     
    Enter your choice

    1

    Enter the model

    CBR 250R

    Enter the brand

    Honda

    Enter the price per day

    1200

    Bike details added successfully
     
    1. Add Bike Details

    2. Group Bikes By Brand

    3. Exit
     
    Enter your choice

    1

    Enter the model

    Ninja 300

    Enter the brand

    Kawasaki

    Enter the price per day

    1500

    Bike details added successfully
     
    1. Add Bike Details

    2. Group Bikes By Brand

    3. Exit
     
    Enter your choice

    2

    Honda

    CBR 250R
     
    Kawasaki

    Ninja 300
     
    1. Add Bike Details

    2. Group Bikes By Brand

    3. Exit
     
    Enter your choice

    3
     
 
*/
class Program
{
    public static void Main(string[] args)
        {
            // Instantiating the Child class

            BikeUtility bikeManagerInstance = new BikeUtility();

            bool isApplicationRunning = true;

            while (isApplicationRunning)
            {
                Console.WriteLine(" 1. Add Bike Details");
                Console.WriteLine("2. Group Bikes By Brand");
                Console.WriteLine("3. Exit");
                Console.WriteLine(" Enter your choice");

                //  input
                string userChoiceInput = Console.ReadLine();
                int parsedChoice ;
                

                if(!int.TryParse(userChoiceInput, out parsedChoice))
                {
                    parsedChoice = 0; // Invalid default
                }

                switch (parsedChoice)
                {
                    case 1:
                        // Input 
                        Console.WriteLine("Enter the model");
                        string enteredModel = Console.ReadLine();

                        Console.WriteLine("Enter the brand");
                        string enteredBrand = Console.ReadLine();

                        Console.WriteLine("Enter the price per day");
                        string enteredPriceRaw = Console.ReadLine();
                        
                        // Default value logic for emergencies.
                        int finalPrice;
                        if (!int.TryParse(enteredPriceRaw, out finalPrice))
                        {
                            Console.WriteLine("Invalid price entered. Defaulting to 1000.");
                            finalPrice = 1000;
                        }

                        // Call the method inherited from Base Class
                        bikeManagerInstance.AddBikeDetails(enteredModel, enteredBrand, finalPrice);
                        break;

                    case 2:
                        // GROUPING SECTION
                        // Here i will Call the method implemented in Child Class
                        SortedDictionary<string, List<Bike>> groupedBikes = bikeManagerInstance.GroupBikesByBrand();

                        // Display
                        foreach (KeyValuePair<string, List<Bike>> brandGroup in groupedBikes)
                        {
                            Console.WriteLine( brandGroup.Key); // Print Brand Name
                            
                            foreach (Bike bikeItem in brandGroup.Value)
                            {
                                Console.WriteLine(bikeItem.Model); // Print Bike Model
                            }
                        }
                        break;

                    case 3:
                        isApplicationRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }
}

