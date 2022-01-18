using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SodaMachine sodaMachine = new SodaMachine();
            sodaMachine.Start();
        }
    }

    public class SodaMachine
    {
        private static int money;

        /// <summary>
        /// This is the starter method for the machine
        /// </summary>
        public void Start()
        {
            var inventory = new[] { new Soda { Name = "coke", Amount = 5, Price = 20 }, new Soda { Name = "sprite", Amount = 3, Price = 15 }, new Soda { Name = "fanta", Amount = 3, Price = 15 } };

            while (true)
            {
                PrintMenu();
                var input = Console.ReadLine();

                if (input.StartsWith("insert"))
                    AddToCredit(input);

                if (input.Equals("recall"))
                {
                    Console.WriteLine("Returning " + money + " to customer");
                    money = 0;
                }

                var sms = false;
                if (input.StartsWith("sms"))
                    sms = true;

                if (input.Contains("order"))
                {
                    var sodaFromInput = input.Split(' ')[sms ? 2 : 1];

                    // replaced switch block so inventory can be extended with more soda's
                    var foundSoda = inventory.FirstOrDefault(sodaInInventory => sodaFromInput == sodaInInventory.Name);
                    if (foundSoda == null)
                        Console.WriteLine("No such soda");
                    else
                        TryToServe(foundSoda, sms);
                }
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine("\n\nAvailable commands:");
            Console.WriteLine("insert (money) - Money put into money slot");
            Console.WriteLine("order (coke, sprite, fanta) - Order from machines buttons");
            Console.WriteLine("sms order (coke, sprite, fanta) - Order sent by sms");
            Console.WriteLine("recall - gives money back");
            Console.WriteLine("-------");
            Console.WriteLine("Inserted money: " + money);
            Console.WriteLine("-------\n\n");
        }

        private void AddToCredit(string input)
        {
            var amount = int.Parse(input.Split(' ')[1]);
            money += amount;
            Console.WriteLine("Adding " + amount + " to credit");
        }

        private void TryToServe(Soda soda, bool sms = false)
        {
            if (soda.Amount == 0)
            {
                Console.WriteLine($"No {soda.Name} left");
            } 
            else if (!sms && money < soda.Price)
            {
                Console.WriteLine("Need " + (soda.Price - money) + " more money");
            } 
            else
            {
                soda.Amount--;
                Console.WriteLine($"Giving {soda.Name} out");
                if (sms)
                    return;

                Console.WriteLine("Giving " + (money - soda.Price) + " out in change");
                money = 0;
            }
        }
    }
    public class Soda
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
    }
}
