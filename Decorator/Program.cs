namespace TestTask;

internal abstract class Laptop
{
    public string Name { get; protected set; }
    public abstract int GetPrice();
    public abstract string GetDescription();
}

class BasicLaptop : Laptop
{
    public BasicLaptop()
    {
        Name = "Базовый ноутбук";
    }

    public override int GetPrice()
    {
        return 30000;
    }

    public override string GetDescription()
    {
        return "8GB RAM, 256GB HDD, Intel UHD Graphics";
    }
}

abstract class LaptopUpgrade : Laptop
{
    protected readonly Laptop Laptop;
    public LaptopUpgrade(Laptop laptop)
    {
        Laptop = laptop;
    }
}

class RamUpgrade : LaptopUpgrade
{
    private readonly int _additionalRam;
    
    public RamUpgrade(Laptop laptop, int additionalRam) : base(laptop)
    {
        _additionalRam = additionalRam;
        Name = $"{Laptop.Name} + {additionalRam}GB RAM";
    }

    public override int GetPrice()
    {
        return Laptop.GetPrice() + (_additionalRam * 500);
    }

    public override string GetDescription()
    {
        return $"{Laptop.GetDescription()}, +{_additionalRam}GB RAM";
    }
}

class SsdUpgrade : LaptopUpgrade
{
    private readonly int _ssdSize;
    
    public SsdUpgrade(Laptop laptop, int ssdSize) : base(laptop)
    {
        _ssdSize = ssdSize;
        Name = $"{Laptop.Name} + {_ssdSize}GB SSD";
    }

    public override int GetPrice()
    {
        return Laptop.GetPrice() + (_ssdSize * 50);
    }

    public override string GetDescription()
    {
        return $"{Laptop.GetDescription()}, +{_ssdSize}GB SSD";
    }
}

class GraphicsCardUpgrade : LaptopUpgrade
{
    public GraphicsCardUpgrade(Laptop laptop) : base(laptop)
    {
        Name = $"{Laptop.Name} + NVIDIA RTX 3060";
    }

    public override int GetPrice()
    {
        return Laptop.GetPrice() + 30_000;
    }

    public override string GetDescription()
    {
        return $"{Laptop.GetDescription()}, NVIDIA RTX 3060";
    }
}

internal abstract class Program
{
    static void Main()
    {
        DisplayBaseLaptop();
        DisplayLaptopWithRamAndSsd();
        DisplayMaxConfigurationLaptop();
    }

    private static void DisplayBaseLaptop()
    {
        Laptop laptop1 = new BasicLaptop();
        Display(laptop1);
    }
    
    private static void DisplayLaptopWithRamAndSsd()
    {
        Laptop laptop2 = new BasicLaptop();
        laptop2 = new RamUpgrade(laptop2, 16); // +16GB RAM
        laptop2 = new SsdUpgrade(laptop2, 512); // +512GB SSD
        Display(laptop2);
        
    }
    
    private static void DisplayMaxConfigurationLaptop()
    {
        Laptop laptop3 = new BasicLaptop();
        laptop3 = new RamUpgrade(laptop3, 32); // +32GB RAM
        laptop3 = new SsdUpgrade(laptop3, 1024); // +1TB SSD
        laptop3 = new GraphicsCardUpgrade(laptop3); // +Доп. видеокарта
        Display(laptop3);
        
    }

    private static void Display(Laptop laptop)
    {
        Console.WriteLine("Название: {0}", laptop.Name);
        Console.WriteLine("Цена: {0} руб", laptop.GetPrice());
        Console.WriteLine("Характеристики: {0}", laptop.GetDescription());
        Console.WriteLine();
    }
}