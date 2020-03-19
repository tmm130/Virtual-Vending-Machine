using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Classes;
using System.IO;

namespace Capstone.Classes
{
    public class Purchase
    {
        //Created Auditor object to make log file
        Auditor auditor = new Auditor();

        //Keeps account of how much money the user has in the machine
        public decimal TotalMoneyFed { get; private set; }

        int fedMoneyInt = 0;

        //Keeps track of what is available in the vending machine and stores each item with their location as the key
        public static Dictionary<string, VendingMachineItem> listOfItems = new Dictionary<string, VendingMachineItem>();

        //Method to run to stock the inventory on machine startup
        public void StockInventory()
        {
            string slotLocation;
            string name;
            decimal price;
            string type;
            string pathToInventoryFile = Path.Combine(Directory.GetCurrentDirectory(), "vendingmachine.csv");
            Queue<string> linesOfItems = new Queue<string>();

            //Pulls file containing what items to add to the vending machine
            try
            {
                using (StreamReader sr = new StreamReader(pathToInventoryFile))
                {
                    while (!sr.EndOfStream)
                    {
                        linesOfItems.Enqueue(sr.ReadLine());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry something went wrong while stocking items");
            }

            //Creates each item pulled from the file and creates them as the correct object depending on the type of the item
            while (linesOfItems.Count > 0)
            {
                string[] individualItemInfo = linesOfItems.Dequeue().Split('|');
                slotLocation = individualItemInfo[0];
                name = individualItemInfo[1];
                price = decimal.Parse(individualItemInfo[2]);
                type = individualItemInfo[3];

                if (type == "Chip")
                {
                    Chip chip = new Chip(name, price, 5);
                    listOfItems.Add(slotLocation, chip);
                }
                else if (type == "Candy")
                {
                    Candy candy = new Candy(name, price, 5);
                    listOfItems.Add(slotLocation, candy);
                }
                else if (type == "Drink")
                {
                    Drink drink = new Drink(name, price, 5);
                    listOfItems.Add(slotLocation, drink);
                }
                else if (type == "Gum")
                {
                    Gum gum = new Gum(name, price, 5);
                    listOfItems.Add(slotLocation, gum);
                }
            }
        }

        //Main menu method to allow customer to choose what they would like to do
        public bool MainMenu()
        {
            string mainMenuChoice = "";
            int mainChoice = 0;

            Console.WriteLine("> (1) Display Vending Machine Items > (2) Purchase > (3) Exit >");
            mainMenuChoice = Console.ReadLine();
            int.TryParse(mainMenuChoice, out mainChoice);

            if (mainChoice == 1)
            {
                DisplayVendingItems();
                MainMenu();
            }
            else if (mainChoice == 2)
            {
                PurchaseMenu();
            }
            else if (mainChoice == 3)
            {
                Console.Clear();
                auditor.WritingLog();
                return true;
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
                MainMenu();
            }

            return true;
        }

        //Purchase menu method to allow customer to choose an item to purchase, add money, or cash out
        public bool PurchaseMenu()
        {
            int purchMenuChoice = 0;
            string purchaseMenuChoice = "";

            Console.WriteLine($">(1) Feed Money >(2) Select Product >(3) Finish Transaction > > Current Money Provided: {TotalMoneyFed:C2}");
            purchaseMenuChoice = Console.ReadLine();
            int.TryParse(purchaseMenuChoice, out purchMenuChoice);

            if (purchMenuChoice == 1)
            {
                FeedMoney();
            }
            else if (purchMenuChoice == 2)
            {
                PickItem();
            }
            else if (purchMenuChoice == 3)
            {
                Console.WriteLine("Thank for your business. Have a great day!");
                FinishTransaction();
                return true;
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
                PurchaseMenu();
            }

            return true;
        }

        //This method displays the items currently in the vending machine, their price, how many are left, and the item's location
        public bool DisplayVendingItems()
        {
            Console.WriteLine();
            Console.WriteLine("Location".PadRight(9) + "Item Name".PadRight(20) + "Price".PadRight(8) + "Amount Left");
            Console.WriteLine("-".PadRight(49, '-'));

            foreach (KeyValuePair<string, VendingMachineItem> kvp in listOfItems)
            {
                string itemPrice = $"{kvp.Value.Price:C2}";
                string amountLeft = $"{kvp.Value.AmountOfItem}";
                Console.WriteLine($"{kvp.Key.PadLeft(4).PadRight(9)}{kvp.Value.Name.PadRight(20)}{itemPrice.PadRight(9)}{amountLeft.PadLeft(10)}");
            }
            Console.WriteLine();

            return true;
        }

        //This method prompts the customer to add money and keeps track of how much they have added
        public decimal FeedMoney()
        {
            Console.Write("How much money would you like to feed? (Accepts: $1, $2, $5, and $10 bills) ");
            string moneyFed = Console.ReadLine();
            bool wholeDollarAmount = int.TryParse(moneyFed, out fedMoneyInt);
            string moreMoney = "";

            //Makes customer choose a correct amount of money to add to the vending machine
            while (!wholeDollarAmount || (fedMoneyInt != -1 && fedMoneyInt != 1 && fedMoneyInt != 2 && fedMoneyInt != 5 && fedMoneyInt != 10))
            {
                fedMoneyInt = 0;
                Console.Write("You must enter a correct whole dollar amount: (Enter -1 if you no longer want to add money) ");
                moneyFed = Console.ReadLine();
                wholeDollarAmount = int.TryParse(moneyFed, out fedMoneyInt);
            }

            //If customer chooses -1 they will be taken back to the purchase menu, otherwise TotalMoneyFed will be updated correctly with how much customer added
            if (fedMoneyInt == -1)
            {
                PurchaseMenu();
                return TotalMoneyFed;
            }
            else
            {
                TotalMoneyFed += fedMoneyInt;
                auditor.WritingLog("FEED MONEY:", fedMoneyInt, TotalMoneyFed);
                Console.Write($"You have entered {TotalMoneyFed:C2}. Would you like to enter any more money? (Y/N) ");
                moreMoney = Console.ReadLine().ToLower();
            }

            //Makes customer choose either yes or no to whether they would like to add more money
            while (moreMoney != "n" && moreMoney != "no" && moreMoney != "yes" && moreMoney != "y")
            {
                Console.Write("You must enter either Yes or No (Y/N). Please try again: ");
                moreMoney = Console.ReadLine().ToLower();
            }

            //If customer says no they are taken back to the purchase menu, if they say yes the FeedMoney method starts over
            if (moreMoney == "n" || moreMoney == "no")
            {
                PurchaseMenu();
            }
            else
            {
                FeedMoney();
            }

            return TotalMoneyFed;
        }

        //This method prompts the customer to choose an item to purchase and updates the amount left of the item and debits the price from the customer's current balance
        public bool PickItem()
        {
            DisplayVendingItems();
            Console.Write("What would you like to purchase? ");
            string itemToPurchase = Console.ReadLine().ToUpper();
            int currentAmountLeft = 0;
            decimal priceToTakeForItem = 0M;

            //Checks if item customer chooses is available and if item exists it deletes 1 from the amount of the item left and debits the price of the item from the customer's balance
            foreach (KeyValuePair<string, VendingMachineItem> kvp in listOfItems)
            {
                if (kvp.Key == itemToPurchase && listOfItems[itemToPurchase].AmountOfItem > 0 && listOfItems[itemToPurchase].Price <= TotalMoneyFed)
                {
                    currentAmountLeft = kvp.Value.AmountOfItem;
                    listOfItems[itemToPurchase].AmountOfItem -= 1;
                    priceToTakeForItem = kvp.Value.Price;
                    TotalMoneyFed -= priceToTakeForItem;
                    auditor.WritingLog($"{kvp.Value.Name}", (kvp.Value.Price * -1), TotalMoneyFed);
                    Console.WriteLine($"You have purchased {kvp.Value.Name} for {kvp.Value.Price:C2}. You have a remaining balance of {TotalMoneyFed:C2}");

                    //Depending on the type of the item, this will return a statement referring to the item type
                    if (kvp.Value.Type == "gum")
                    {
                        Console.WriteLine("Chew Chew, Yum!");
                    }
                    else if (kvp.Value.Type == "drink")
                    {
                        Console.WriteLine("Glug Glug, Yum!");
                    }
                    else if (kvp.Value.Type == "candy")
                    {
                        Console.WriteLine("Munch Munch, Yum!");
                    }
                    else if (kvp.Value.Type == "chip")
                    {
                        Console.WriteLine("Crunch Crunch, Yum!");
                    }

                    PurchaseMenu();
                    return true;
                }
                else if (kvp.Value.AmountOfItem == 0)
                {
                    Console.WriteLine($"The item({kvp.Value.Name}) you selected is sold out. Please try again.");
                    PurchaseMenu();
                    return true;
                }
                else if (!listOfItems.ContainsKey(itemToPurchase))
                {
                    Console.WriteLine("This product code does not exist. Please try again.");
                    PurchaseMenu();
                    return true;
                }
                else if (listOfItems[itemToPurchase].Price > TotalMoneyFed)
                {
                    Console.WriteLine($"You do not have enough money to buy {kvp.Value.Name}. It costs {kvp.Value.Price:C2} and you have {TotalMoneyFed:C2}");
                    PurchaseMenu();
                    return true;
                }
            }
            return true;
        }

        //This method ends the transaction and gives the correct change back to the customer and returns them to the main menu
        public bool FinishTransaction()
        {
            auditor.WritingLog("GIVE CHANGE:", (TotalMoneyFed * -1), 0);
            int customerMoneyLeftAfterTransaction = Convert.ToInt32(TotalMoneyFed * 100);
            int quartersReturned = 0;
            int dimesReturned = 0;
            int nickelsReturned = 0;

            //Calculates the correct amount of change to give customer in quarters, dimes, and nickels in the smallest amount of coins possible
            while (customerMoneyLeftAfterTransaction - 25 >= 0)
            {
                quartersReturned++;
                customerMoneyLeftAfterTransaction -= 25;
            }
            while (customerMoneyLeftAfterTransaction - 10 >= 0)
            {
                dimesReturned++;
                customerMoneyLeftAfterTransaction -= 10;
            }
            while (customerMoneyLeftAfterTransaction - 5 >= 0)
            {
                nickelsReturned++;
                customerMoneyLeftAfterTransaction -= 5;
            }

            Console.WriteLine($"Your change is {TotalMoneyFed:C2}. You received {quartersReturned} quarters, {dimesReturned} dimes, and {nickelsReturned} nickels");
            TotalMoneyFed = 0;

            //Returns customer to main menu
            MainMenu();
            return true;

        }
    }
}
