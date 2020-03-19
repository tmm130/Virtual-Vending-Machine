using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    //Creates variables for a vending machine item
    public class VendingMachineItem
    {
        public string Name { get; protected set; }
        public decimal Price { get; protected set; }
        public string SlotLocation { get; protected set; }
        public int AmountOfItem { get; set; }
        public string Type { get; protected set; }
    }
}
