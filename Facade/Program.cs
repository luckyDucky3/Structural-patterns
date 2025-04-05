namespace Facade;

class VideoFile
{
    public VideoFile(string filename) 
    {
        Console.WriteLine($"VideoFile: Загружен файл {filename}");
    }
}

class OggCompressionCodec
{
    public OggCompressionCodec() 
    {
        Console.WriteLine("OggCompressionCodec: Создан кодек OGG");
    }
}

class Mpeg4CompressionCodec
{
    public Mpeg4CompressionCodec() 
    {
        Console.WriteLine("MPEG4CompressionCodec: Создан кодек MP4");
    }
}

internal class CodecFactory
{
    public object Extract(VideoFile file) 
    {
        Console.WriteLine("CodecFactory: Извлечение кодека из видеофайла");
        return new object(); // Возвращаем абстрактный кодек
    }
}

internal static class BitrateReader
{
    public static byte[] Read(string filename, object codec) 
    {
        Console.WriteLine($"BitrateReader: Чтение файла {filename} с кодеком {codec.GetType().Name}");
        return new byte[1024]; // Возвращаем буфер с данными
    }

    public static byte[] Convert(byte[] buffer, object codec) 
    {
        Console.WriteLine($"BitrateReader: Конвертация в {codec.GetType().Name}");
        return buffer; // Возвращаем конвертированные данные
    }
}

internal class AudioMixer
{
    public byte[] Fix(byte[] result) 
    {
        Console.WriteLine("AudioMixer: Фиксация аудиодорожки");
        return result;
    }
}
class VideoConverter
{
    public byte[] Convert(string filename, string format) 
    {
        Console.WriteLine($"VideoConverter: Конвертация {filename} в {format}");

        var file = new VideoFile(filename);
        var sourceCodec = new CodecFactory().Extract(file);

        object destinationCodec;
        if (format == "mp4")
            destinationCodec = new Mpeg4CompressionCodec();
        else
            destinationCodec = new OggCompressionCodec();

        var buffer = BitrateReader.Read(filename, sourceCodec);
        var result = BitrateReader.Convert(buffer, destinationCodec);
        result = new AudioMixer().Fix(result);

        Console.WriteLine("VideoConverter: Конвертация завершена");
        return result;
    }
}

internal static class Program
{
    static void Main() 
    {
        var converter = new VideoConverter();
        var mp4 = converter.Convert("funny-cats-video.ogg", "mp4");
    }
}