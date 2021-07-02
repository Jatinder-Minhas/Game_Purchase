using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Jatinder_Minhas
{
    class DatabaseAccess
    {

        public bool isCustomerExist(string name)
        {
            bool isExist = false;

            using (var context = new GameShoppingDBEntities())
            {
                var customer = context.Customers
                          .Where(s => s.Name.ToUpper().Equals(name.ToUpper()))
                          .FirstOrDefault();

                if (customer == null)
                    isExist = false;
                else
                    isExist = true;
            }
            return isExist;
        }

        public bool isCustomerExist(int customerId)
        {
            bool isExist = false;

            using (var context = new GameShoppingDBEntities())
            {
                var customer = context.Customers
                          .Where(c => c.CustomerId == customerId)
                          .FirstOrDefault();

                if (customer == null)
                    isExist = false;
                else
                    isExist = true;
            }
            return isExist;
        }

        public Customer getCustomerByName(string name)
        {
            using (var context = new GameShoppingDBEntities())
            {
                var customer = context.Customers
                          .Where(s => s.Name.ToUpper().Equals(name.ToUpper()))
                          .FirstOrDefault();

                return customer;
            }
        }

        public Customer getCustomerById(int customerId)
        {
            using (var context = new GameShoppingDBEntities())
            {
                var customer = context.Customers
                          .Where(c => c.CustomerId == customerId)
                          .FirstOrDefault();

                return customer;
            }
        }

        public List<Customer> getAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (var context = new GameShoppingDBEntities())
            {
                customers = context.Customers
                               .ToList();
            }
            return customers;
        }

        public void displayLatestOrder()
        {
            using (var context = new GameShoppingDBEntities())
            {
                Order order = context.Orders
                              .OrderByDescending(o => o.OrderId)
                              .FirstOrDefault();

                if(order != null)
                {
                    double tax = ((order.Game.Price * order.Quantity - order.Discount) * 13) / 100;
                    double total = order.Game.Price * order.Quantity - order.Discount + tax;

                    Console.WriteLine($"{order.OrderId,15} | {order.Date.ToString("dddd, dd MMMM yyyy"),-25} | {order.Customer.Name,-25} | {order.Game.Name,-25} | {"$" + order.Game.Price,15} | {order.Quantity,15} | {"$" + order.Discount,15} | {"$" + tax,15} | {"$" + total,15}");
                }
                else
                {
                    Console.Write("\tNo Records Found\n");
                }
            }
        }

        public void historyByCustomerId(int customerId)
        {

            using (var context = new GameShoppingDBEntities())
            {
               var orders = context.Orders
                          .Where(o => o.CustomerId == customerId)
                          .ToList();

                if (orders.Any())
                {
                    foreach (Order o in orders)
                    {

                        double tax = ((o.Game.Price * o.Quantity - o.Discount) * 13) / 100;
                        double total = o.Game.Price * o.Quantity - o.Discount + tax;

                        Console.WriteLine($"{o.OrderId,15} | {o.Date.ToString("dddd, dd MMMM yyyy"),-25} | {o.Customer.Name,-25} | {o.Game.Name,-25} | {"$" + o.Game.Price,15} | {o.Quantity,15} | {"$" + o.Discount,15} | {"$" + tax,15} | {"$" + total,15}");
                    }
                }
                else
                {
                    Console.Write("\tNo Records Found\n");
                }
            }
        }

        public void historyAll()
        {
            using (var context = new GameShoppingDBEntities())
            {
                var orders = context.Orders
                           .ToList();

                if(orders.Any())
                {
                    foreach (Order o in orders)
                    {
                        double tax = ((o.Game.Price * o.Quantity - o.Discount) * 13) / 100;
                        double total = o.Game.Price * o.Quantity - o.Discount + tax;

                        Console.WriteLine($"{o.OrderId,15} | {o.Date.ToString("dddd, dd MMMM yyyy"),-25} | {o.Customer.Name,-25} | {o.Game.Name,-25} | {"$" + o.Game.Price,15} | {o.Quantity,15} | {"$" + o.Discount,15} | {"$" + tax,15} | {"$" + total,15}");
                    }
                }
                else
                {
                    Console.Write("\tNo Records Found\n");
                }
            }
        }

        public bool insertCustomer(string name)
        {
            bool isInserted = false;
            using (var context = new GameShoppingDBEntities())
            {
                var customer = new Customer();
                customer.Name = name;
                context.Customers.Add(customer);
                context.SaveChanges();

                isInserted = isCustomerExist(name);
            }

            return isInserted;
        }

        public List<Game> getGames()
        {
            List<Game> games = new List<Game>();

            using (var context = new GameShoppingDBEntities())
            {
                games = context.Games
                        .ToList();
            }
            return games;
        }

        public void placeOrder(int customerId, int gameId, int quantity)
        {
            using (var context = new GameShoppingDBEntities())
            {
                var order = new Order
                {
                    Date = DateTime.Now,
                    Quantity = quantity,
                    CustomerId = customerId,
                    GameId = gameId
                };

                var game = new Game();
                game = context.Games
                       .Where(g => g.GameId == order.GameId)
                       .FirstOrDefault();

                order.Discount = (game.Price * quantity * ((quantity >= 5) ? 10 : 0)/100);

                game.Stock -= quantity;

                context.Orders.Add(order);

                context.SaveChanges();
            }
        }
    }
}
