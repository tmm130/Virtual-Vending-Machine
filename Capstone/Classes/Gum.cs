using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Gum : VendingMachineItem
    {
        public Gum(string name, decimal price, int amountOfItem)
        {
            Name = name;
            Price = price;
            AmountOfItem = amountOfItem;
            Type = "gum";
        }
    }
}
