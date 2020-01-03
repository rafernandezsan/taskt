using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    class CustomCode
    {
        static void Main(string[] args)
        {


            System.Console.WriteLine("Number of command line parameters = {0}", args.Length);

            foreach (string s in args)
            {
                System.Console.WriteLine("Found Argument: " + s);
            }


            Console.WriteLine("Hi! This code was compiled on the fly from taskt!");

            //Keep the console window open, remove this if you do not want the exe to block
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

    }
}
