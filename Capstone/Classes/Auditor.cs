using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{
    public class Auditor
    {
        public bool WritingLog(string whatHappened, decimal beginningAmount, decimal totalAmount)
        
        {
            string directory = Directory.GetCurrentDirectory();
            string fileName = "Log.txt";
            string fullPath = Path.Combine(directory, fileName);

            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    sw.WriteLine($"{DateTime.Now} {whatHappened}: {beginningAmount:C2} {totalAmount:C2}");
                }
            }catch(Exception e)
            {
                Console.WriteLine("Bananas");
            }
            return true;
        }

        public bool WritingLog()
        {
            string directory = Directory.GetCurrentDirectory();
            string fileName = "Log.txt";
            string fullPath = Path.Combine(directory, fileName);

            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    sw.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Bananas");
            }
            return true;
        }
    }
}
