namespace Command;
public class Light
{
    private readonly string _location;
    
    public Light(string location)
    {
        _location = location;
    }
    
    public void On()
    {
        Console.WriteLine($"Свет в {_location} включен");
    }
    
    public void Off()
    {
        Console.WriteLine($"Свет в {_location} выключен");
    }
}

public class Thermostat
{
    private int _temperature;
    
    public void SetTemperature(int temp)
    {
        _temperature = temp;
        Console.WriteLine($"Термостат установлен на {_temperature}°C");
    }
}

// Базовый интерфейс команды
public interface ICommand
{
    void Execute();
    void Undo();
}

// Конкретные команды
public class LightOnCommand : ICommand
{
    private readonly Light _light;
    
    public LightOnCommand(Light light)
    {
        _light = light;
    }
    
    public void Execute()
    {
        _light.On();
    }
    
    public void Undo()
    {
        _light.Off();
    }
}

public class LightOffCommand : ICommand
{
    private readonly Light _light;
    
    public LightOffCommand(Light light)
    {
        _light = light;
    }
    
    public void Execute()
    {
        _light.Off();
    }
    
    public void Undo()
    {
        _light.On();
    }
}

public class ThermostatCommand : ICommand
{
    private readonly Thermostat _thermostat;
    private readonly int _temperature;
    private int _previousTemp;
    
    public ThermostatCommand(Thermostat thermostat, int temperature)
    {
        _thermostat = thermostat;
        _temperature = temperature;
    }
    
    public void Execute()
    {
        _previousTemp = new Random().Next(15, 25); // имитация предыдущей температуры
        _thermostat.SetTemperature(_temperature);
    }
    
    public void Undo()
    {
        _thermostat.SetTemperature(_previousTemp);
    }
}

// Инициатор команд - пульт управления
public class RemoteControl
{
    private readonly Stack<ICommand> _commandHistory = new Stack<ICommand>();
    
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
    }
    
    public void UndoLastCommand()
    {
        if (_commandHistory.Count > 0)
        {
            var lastCommand = _commandHistory.Pop();
            lastCommand.Undo();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создаем устройства
        var livingRoomLight = new Light("Гостиная");
        var kitchenLight = new Light("Кухня");
        var thermostat = new Thermostat();

        // Создаем команды
        var livingRoomLightOn = new LightOnCommand(livingRoomLight);
        var livingRoomLightOff = new LightOffCommand(livingRoomLight);
        var kitchenLightOn = new LightOnCommand(kitchenLight);
        var setThermostat = new ThermostatCommand(thermostat, 22);

        // Пульт управления
        var remote = new RemoteControl();

        Console.WriteLine("Управление умным домом:");
        
        // Выполняем команды
        remote.ExecuteCommand(livingRoomLightOn);
        remote.ExecuteCommand(kitchenLightOn);
        remote.ExecuteCommand(setThermostat);
        
        Console.WriteLine("\nОтменяем последнюю команду:");
        remote.UndoLastCommand();
        
        Console.WriteLine("\nВыключаем свет в гостиной:");
        remote.ExecuteCommand(livingRoomLightOff);
        
        Console.WriteLine("\nИстория отмен:");
        remote.UndoLastCommand(); // Отмена выключения света
        remote.UndoLastCommand(); // Отмена установки термостата
    }
}