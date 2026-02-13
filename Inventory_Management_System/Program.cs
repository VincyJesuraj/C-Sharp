using System;

class Program
{
    static void Main()
    {
        Inventory inventory = new Inventory();

        while (true)
        {
            Console.WriteLine("\n--- Inventory Management System ---");
            Console.WriteLine("1. Add Supplier");
            Console.WriteLine("2. View Suppliers");
            Console.WriteLine("3. Add Product");
            Console.WriteLine("4. View Products");
           
            Console.WriteLine("5. Sell Product");
            Console.WriteLine("6. Remove Product");
            Console.WriteLine("7. Exit");

            Console.Write("Choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1: inventory.AddSupplier(); break;
                case 2: inventory.ViewSuppliers(); break;
                case 3: inventory.AddProduct(); break;
                case 4: inventory.ViewProducts(); break;
            
                case 5: inventory.SellProduct(); break;
                case 6: inventory.RemoveProduct(); break;
                case 7: return;
                default: Console.WriteLine("Invalid choice!"); break;
            }
        }
    }
}
