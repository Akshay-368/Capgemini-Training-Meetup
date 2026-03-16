using System;
using System.Collections.Generic;

namespace BikeRentalApplication ;

    //Interface
    // so that I

    public interface IBikeOperationContract
    {
        void AddBikeDetails(string modelInput, string brandInput, int pricePerDayInput);
        SortedDictionary< string, List<Bike>> GroupBikesByBrand();
    }


    // THE MODEL (with Encapsulation)

    public class Bike
    {
        // Encapsulated properties with private setters to ensure data integrity

        public string Model { get; private set; }
        public string Brand { get; private set; }
        public int PricePerDay { get; private set; }

        // Constructor for initialization
        public Bike(string modelName, string brandName, int price)
        {
            this.Model = modelName;
            this.Brand = brandName;
            this.PricePerDay = price;
        }
    }


    // THE BASE CLASS ( with Abstraction & Central Storage)

    //Abstract base class holding the central storage and common logic.

    public abstract class BaseBikeManager : IBikeOperationContract
    {
        // Protected Access Modifier:
        // It keeps the data private from the outside world (Encapsulation),
        // but allows the Child class ( BikeUtility ) to access it.
        // This avoids static memory waste .
        protected SortedDictionary<int, Bike> bikeDetailsRepository ;

        public BaseBikeManager()
        {
            // Initialize storage
            bikeDetailsRepository = new SortedDictionary<int, Bike>();
        }

        // Concrete implementation of Adding.

        public void AddBikeDetails(string modelInput, string brandInput, int pricePerDayInput)
        {
            // Logic - Key is Count + 1
            int newIdentificationKey = bikeDetailsRepository.Count + 1;

            // Creating the object
            Bike newBikeObject = new Bike(modelInput, brandInput, pricePerDayInput);

            // Adding to central storage
            bikeDetailsRepository.Add(newIdentificationKey, newBikeObject);
            
            Console.WriteLine(" Bike details added successfully " );
        }

        // Abstract Method :
        // Let's leave this Logic for the child class.
        // This forces the child to define how the grouping happens.

        public abstract SortedDictionary<string, List<Bike>> GroupBikesByBrand();
    }


    //  THE UTILITY CHILD CLASS (Inheritance)

    public class BikeUtility : BaseBikeManager
    {
        // Implementation of the abstract method.

        public override SortedDictionary<string, List<Bike>> GroupBikesByBrand()
        {
            // Initialize the result dictionary
            SortedDictionary<string, List<Bike>> groupedResultDictionary = new SortedDictionary < string, List<Bike>>();

            // Iterate through the PROTECTED repo inherited from BaseBikeManager
            foreach (KeyValuePair<int, Bike> entry in bikeDetailsRepository)
            {
                Bike currentBike = entry.Value;
                string currentBrand = currentBike.Brand;


                if (!groupedResultDictionary.ContainsKey(currentBrand))
                {
                    groupedResultDictionary.Add(currentBrand, new List<Bike>());
                }

                groupedResultDictionary[currentBrand].Add(currentBike);
            }

            return groupedResultDictionary;
        }
    }





