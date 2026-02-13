using System;
using System.Collections.Generic;

class Inventory
{
    List<Product> products = new List<Product>();
    List<Supplier> suppliers = new List<Supplier>();

    public void AddSupplier()
    {
        Console.Write("Supplier ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Supplier Name: ");
        string name = Console.ReadLine();

        Console.Write("Contact Info: ");
        string contact = Console.ReadLine();

        suppliers.Add(new Supplier
        {
            SupplierId = id,
            Name = name,
            Contact = contact
        });

        Console.WriteLine("Supplier added successfully!");
    }

    public void ViewSuppliers()
    {
        Console.WriteLine("\n--- Suppliers ---");
        foreach (Supplier s in suppliers)
        {
            Console.WriteLine($"ID: {s.SupplierId}, Name: {s.Name}, Contact: {s.Contact}");
        }
    }

    public void AddProduct()
    {
        Console.Write("Product ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Product Name: ");
        string name = Console.ReadLine();

        Console.Write("Quantity: ");
        int qty = int.Parse(Console.ReadLine());

        Console.Write("Price: ");
        double price = double.Parse(Console.ReadLine());

        Console.Write("Supplier ID: ");
        int supplierId = int.Parse(Console.ReadLine());

        Supplier supplier = suppliers.Find(s => s.SupplierId == supplierId);

        if (supplier == null)
        {
            Console.WriteLine("Supplier not found! Add supplier first.");
            return;
        }

        products.Add(new Product
        {
            ProductId = id,
            Name = name,
            Quantity = qty,
            Price = price,
            Supplier = supplier
        });

        Console.WriteLine("Product added successfully!");
    }

    public void ViewProducts()
    {
        Console.WriteLine("\n--- Products ---");
        foreach (Product p in products)
        {
            Console.WriteLine(
                $"ID: {p.ProductId}, Name: {p.Name}, Qty: {p.Quantity}, Price: {p.Price}, Supplier: {p.Supplier.Name}"
            );
        }
    }

    

    public void SellProduct()
    {
        Console.Write("Product ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Quantity to sell: ");
        int sellQty = int.Parse(Console.ReadLine());

        foreach (Product p in products)
        {
            if (p.ProductId == id)
            {
                if (p.Quantity >= sellQty)
                {
                    p.Quantity -= sellQty;
                    Console.WriteLine("Product sold successfully!");
                }
                else
                {
                    Console.WriteLine("Insufficient stock!");
                }
                return;
            }
        }
        Console.WriteLine("Product not found.");
    }

    public void RemoveProduct()
    {
        Console.Write("Product ID to remove: ");
        int id = int.Parse(Console.ReadLine());

        for (int i = 0; i < products.Count; i++)
        {
            if (products[i].ProductId == id)
            {
                products.RemoveAt(i);
                Console.WriteLine("Product removed successfully!");
                return;
            }
        }
        Console.WriteLine("Product not found.");
    }
}
