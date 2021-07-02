using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Jatinder_Minhas
{
    class Controller
    {
        private DatabaseAccess databaseAccess;

        public Controller()
        {
            databaseAccess = new DatabaseAccess();
        }

        private void displayMenu()
        {
            Console.WriteLine("Main Menu\n");
            Console.WriteLine("1. Purchase Game");
            Console.WriteLine("2. View Customer Transaction History");
            Console.WriteLine("3. View all Transactions");
            Console.WriteLine("4. Exit");
        }

        public void start()
        {
            bool loopCheck = true;

            while (loopCheck)
            {
                Console.Clear();
                displayMenu();
                Console.WriteLine();

                Console.Write("Enter your choice: ");
                int input = Convert.ToInt32(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Purchase Game\n");

                        Console.Write("The Customer Name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine();

                        if(!databaseAccess.isCustomerExist(name))
                        {
                            databaseAccess.insertCustomer(name);
                        }

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"{"Game ID",15} | {"Game Name",-35} | {"Price",12} | {"In Stock",15}");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;

                        List<Game> games = databaseAccess.getGames();
                        foreach (Game g in games)
                        {
                            Console.WriteLine($"{g.GameId,15} | {g.Name,-35} | {g.Price,12} | {g.Stock,15}");
                        }

                        bool isGameExist = false;

                        while (!isGameExist)
                        {
                            Console.Write("\nEnter Game Id: ");
                            int gameId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine();

                            if (games.Exists(g => g.GameId == gameId))
                            {
                                isGameExist = true;
                                Game game = games.Find(g => g.GameId == gameId);

                                int quantity = 0;

                                if (game.Stock != 0)
                                {
                                    do
                                    {
                                        Console.Write("Enter Quantity(More than 5 = 10% discount): ");
                                        quantity = Convert.ToInt32(Console.ReadLine());

                                        if (game.Stock >= quantity)
                                        {
                                            databaseAccess.placeOrder(databaseAccess.getCustomerByName(name).CustomerId, gameId, quantity);

                                            Console.WriteLine("\nYour order is placed successfully!\n");

                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine($"{"Order ID",15} | {"Date",-25} | {"Customer Name",-25} | {"Game Name",-25} | {"Price",15} | {"Quantity",15} | {"Discount",15} | {"Tax",15} | {"Net Total",15}");
                                            Console.WriteLine();
                                            Console.ForegroundColor = ConsoleColor.White;

                                            databaseAccess.displayLatestOrder();
                                            
                                        }
                                        else
                                        {
                                            Console.WriteLine("Enter a quantity less than stock!\n");
                                        }
                                    }
                                    while (game.Stock < quantity);
                                }
                                else
                                {
                                     Console.WriteLine("Sorry, this game is out of stock");
                                     isGameExist = false;
                                }
                                    
                            }
                            else
                            {
                                Console.WriteLine("Game ID is invalid!");
                            }
                        }

                        Console.WriteLine("\nPress Enter to Continue...");
                        Console.ReadLine();
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("View Customer Transactions\n");

                        Console.WriteLine("The Customer Names --->\n");

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"{"Game ID",15} | {"Game Name",-35}");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;

                        List<Customer> customers = databaseAccess.getAllCustomers();
                        foreach (Customer c in customers)
                        {
                            Console.WriteLine($"{c.CustomerId,15} | {c.Name,-35}");
                        }

                        Console.Write("\nEnter the CustomerId: ");
                        int customerId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine();

                        if(databaseAccess.isCustomerExist(customerId))
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"{"Order ID",15} | {"Date",-25} | {"Customer Name",-25} | {"Game Name",-25} | {"Price",15} | {"Quantity",15} | {"Discount",15} | {"Tax",15} | {"Net Total",15}");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.White;

                            databaseAccess.historyByCustomerId(customerId);
                        }
                        else
                        {
                            Console.Write("\tCustomer Not Exist. Invalid Customer Id\n");
                        }

                        Console.WriteLine("\nPress Enter to Continue...");
                        Console.ReadLine();

                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("View All Customers Transactions\n");

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"{"Order ID",15} | {"Date",-25} | {"Customer Name",-25} | {"Game Name",-25} | {"Price",15} | {"Quantity",15} | {"Discount",15} | {"Tax",15} | {"Net Total",15}");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;

                        databaseAccess.historyAll();

                        Console.WriteLine("\nPress Enter to Continue...");
                        Console.ReadLine();

                        break;

                    case 4:
                        loopCheck = false;
                        break;
                }
            }
        }
    }
}
