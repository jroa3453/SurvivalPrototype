
Store myStore = new Store("Joshs MiniMart");
myStore.PrintStoreInfo();

    
Product keyboard = new Product("Mechanical Keyboard", 99.99m, 2, 0.20m);
Product mouse    = new Product("Wireless Mouse", 49.99m, 1, 0.10m);

    

     keyboard.PrintLabel();
    Console.WriteLine($"Subtotal: ${keyboard.CalculateSubTotal():F2}");

    mouse.PrintLabel();    
    Console.WriteLine($"Subtotal: ${mouse.CalculateSubTotal():F2}");

    Console.WriteLine($"Keyboard expensive? {keyboard.IsExpensive()}");
    Console.WriteLine($"Mouse expensive? {mouse.IsExpensive()}");

    Console.WriteLine($"Discounted Price: ${keyboard.CalculateDiscountedPrice():F2}");
    Console.WriteLine($"Discounted Price: ${mouse.CalculateDiscountedPrice():F2}");

    
class Store
{
    public string Name { get; set;}

    public Store(string name)
    {
        Name = name;
    }

    public void PrintStoreInfo()
    {
        Console.WriteLine("--------------------------");
        Console.WriteLine($"Store Name: {Name}");
        Console.WriteLine("--------------------------");
    }
}
class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }


    public  Product(string name, decimal price, int quantity, decimal discount)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        Discount = discount;
    }

    public bool IsExpensive()
    {
        return Price >50m;
        
    }
    
    public void PrintLabel()
    {
        Console.WriteLine($"Product {Name}");
        Console.WriteLine($"Price: ${Price:F2}");
        Console.WriteLine($"Quantity: {Quantity}");
    }

    public decimal CalculateSubTotal()
    {
        
        return Price * Quantity;
    }  

    public decimal CalculateDiscountedPrice()
    {
        return CalculateSubTotal() * (1 - Discount);
    }
}

    
