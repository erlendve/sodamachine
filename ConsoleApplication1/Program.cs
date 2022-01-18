using System;
using System.Linq;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main()
        {
            new SodaMachine().Start();
        }
    }

    public class SodaMachine
    {
        private static int money;

        private static void PrintMenu()
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
        private static void AddToCredit(string input)
        {
            var amount = int.Parse(input.Split(' ')[1]);
            money += amount;
            Console.WriteLine("Adding " + amount + " to credit");
        }

        private static void Recall()
        {
            Console.WriteLine("Returning " + money + " to customer");
            money = 0;
        }
        private static void Serve(Soda soda)
        {
            soda.Amount--;
            Console.WriteLine($"Giving {soda.Name} out");
        }

        private static void GiveChange(Soda soda, bool overtime)
        {
            if (overtime)
            {
                Console.WriteLine("Keep the change you filthy animal");
                return;
            }

            var change = money - soda.Price;
            if (change != 0)
            {
                Console.WriteLine("Giving " + (money - soda.Price) + " out in change");
                money = 0;
            }
        }

        private static void TryToServe(Soda soda, string input)
        {
            var prepaid = input.StartsWith("sms");
            var overtime = input.Contains("overtime");

            if (soda.Amount == 0)
            {
                Console.WriteLine($"No {soda.Name} left");
            }
            else if (!prepaid && money < soda.Price)
            {
                Console.WriteLine("Need " + (soda.Price - money) + " more money");
            }
            else
            {
                Serve(soda);
                if (!prepaid)
                    GiveChange(soda, overtime);
            }
        }

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
                    Recall();

                if (input.Contains("order"))
                {
                    var sodaFromInput = input.Split(' ').Last();

                    // replaced switch block so inventory can be extended with more soda's
                    var foundSoda = inventory.FirstOrDefault(sodaInInventory => sodaFromInput == sodaInInventory.Name);
                    if (foundSoda == null)
                        Console.WriteLine("No such soda");
                    else
                        TryToServe(foundSoda, input);
                }
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
