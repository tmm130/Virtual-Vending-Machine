using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Candy : VendingMachineItem
    {
        public Candy(string name, decimal price, int amountOfItem)
        {
            Name = name;
            Price = price;
            AmountOfItem = amountOfItem;
            Type = "candy";
        }
    }
}
