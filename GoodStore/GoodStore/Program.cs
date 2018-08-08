using System;
using System.Data.SqlClient;
using System.IO;

namespace GoodStore
{
    class Program
    {
        static void Main(string[] args)
        {
            StartWork();
        }

        static void StartWork()
        {
            while (true)
            {
                try
                {
                    ShowMainMenu();
                    Menu menuChoise = (Menu)int.Parse(Console.ReadLine());

                    using (var repo = new Repository())
                    {
                        switch (menuChoise)
                        {
                            case Menu.AddGoods:
                                AddGoods(repo);
                                break;
                            case Menu.AddBatch:
                                AddBatch(repo);
                                break;
                            case Menu.ShowGoods:
                                ShowGoods(repo);
                                break;
                            case Menu.Exit:
                                break;
                            default:
                                Console.WriteLine("Wrong menu point. Try again");
                                break;
                        }
                    }
                    if (menuChoise == Menu.Exit) break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("What are you want to do?\n Choose:");
            Console.WriteLine("1. Add goods");
            Console.WriteLine("2. Add batch");
            Console.WriteLine("3. Show goods");
            Console.WriteLine("4. Exit");
        }

        static void AddGoods(Repository repo)
        {
            Goods goods = new Goods();

            Console.WriteLine("Enter all values:");
            Console.WriteLine("Name:");
            goods.Name = Console.ReadLine();

            Console.WriteLine("Unit:");
            goods.Unit = Console.ReadLine();

            Console.WriteLine("UnitPrice:");
            goods.UnitPrice = double.Parse(Console.ReadLine());

            Console.WriteLine("Quantity:");
            goods.Quantity = int.Parse(Console.ReadLine());

            repo.AddGoods(goods);
        }

        static void AddBatch(Repository repo)
        {
            var batch = new Batch();

            Console.WriteLine("Enter all values:");
            Console.WriteLine("Operation type:");
            batch.OperationType = Console.ReadLine();

            Console.WriteLine("Date:");
            batch.Date = DateTime.Parse(Console.ReadLine());

            var batchContent = new BatchContent();
            Console.WriteLine("GoodsId:");
            batchContent.GoodsId = int.Parse(Console.ReadLine());

            Console.WriteLine("BatchId:");
            batchContent.BatchId = int.Parse(Console.ReadLine());

            Console.WriteLine("Quanity:");
            batchContent.Quantity = int.Parse(Console.ReadLine());

            repo.AddBatch(batch, batchContent);
        }

        static void ShowGoods(Repository repo)
        {
            var goods = repo.GetGoods();

            if (goods.Count < 1)
            {
                Console.WriteLine("There are no goods");
                return;
            }

            Console.WriteLine($"GoodsId\tName\tUnit\tUnitPrice\tQuantity");
            Console.WriteLine(new string('-', 50));

            foreach (var g in goods)
                Console.WriteLine($"{g.GoodsId} \t {g.Name} \t {g.Unit} \t {g.UnitPrice} \t {g.Quantity}");

            Console.WriteLine(new string('-', 50));
        }

        enum Menu
        {
            AddGoods = 1, AddBatch, ShowGoods, Exit
        }
    }
}
