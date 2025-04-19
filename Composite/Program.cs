namespace Composite;
public abstract class FileSystemComponent
{
    public string Name { get; }
    public abstract long GetSize();
    public abstract void Display(string indent = "");

    protected FileSystemComponent(string name)
    {
        Name = name;
    }
}

// Листовой компонент - файл
public class File : FileSystemComponent
{
    private readonly long _size;

    public File(string name, long size) : base(name)
    {
        _size = size;
    }

    public override long GetSize()
    {
        return _size;
    }

    public override void Display(string indent = "")
    {
        Console.WriteLine($"{indent}📄 {Name} ({_size} bytes)");
    }
}

// Композитный компонент - папка
public class Directory : FileSystemComponent
{
    private readonly List<FileSystemComponent> _children = new List<FileSystemComponent>();

    public Directory(string name) : base(name) { }

    public void Add(FileSystemComponent component)
    {
        _children.Add(component);
    }

    public void Remove(FileSystemComponent component)
    {
        _children.Remove(component);
    }

    public override long GetSize()
    {
        long totalSize = 0;
        foreach (var component in _children)
        {
            totalSize += component.GetSize();
        }
        return totalSize;
    }

    public override void Display(string indent = "")
    {
        Console.WriteLine($"{indent}📁 {Name} (total: {GetSize()} bytes)");
        
        foreach (var component in _children)
        {
            component.Display(indent + "  ");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создаем файлы
        var file1 = new File("document.txt", 1500);
        var file2 = new File("image.jpg", 2500);
        var file3 = new File("notes.txt", 500);
        var file4 = new File("report.pdf", 4200);

        // Создаем папки
        var documents = new Directory("Документы");
        var images = new Directory("Изображения");
        var work = new Directory("Работа");
        var root = new Directory("Корневая папка");

        // Строим структуру
        documents.Add(file1);
        documents.Add(file3);
        
        images.Add(file2);
        
        work.Add(file4);
        work.Add(documents);
        
        root.Add(work);
        root.Add(images);

        // Выводим структуру
        Console.WriteLine("Структура файловой системы:");
        root.Display();

        // Выводим размеры
        Console.WriteLine("\nРазмеры:");
        Console.WriteLine($"Размер папки 'Документы': {documents.GetSize()} bytes");
        Console.WriteLine($"Размер папки 'Работа': {work.GetSize()} bytes");
        Console.WriteLine($"Общий размер: {root.GetSize()} bytes");
    }
}