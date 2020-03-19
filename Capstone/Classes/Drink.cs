using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Drink : VendingMachineItem
    {
        public Drink(string name, decimal price, int amountOfItem)
        {
            Name = name;
            Price = price;
            AmountOfItem = amountOfItem;
            Type = "drink";
        }
    }
}
