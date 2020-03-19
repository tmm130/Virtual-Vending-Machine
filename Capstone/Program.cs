using Capstone.Classes;
using System;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            Purchase menuSetUp = new Purchase();
            menuSetUp.StockInventory();
            menuSetUp.MainMenu();
        }
    }
}
