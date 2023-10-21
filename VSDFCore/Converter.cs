using Serilog;
using Serilog.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using VSDFCore.Options;

namespace VSDFCore;

public class Converter
{
    private readonly IConverterOptions _options;
    private readonly Logger? _logger;

    public Converter(IConverterOptions? options = null, Logger? logger = null)
    {
        _options = options ?? new DefaultFileConverterOptions();
        _logger = logger;
    }

    public void ToVsdf(string filePath, string outputDir)
    {
        if (!File.Exists(filePath))
        {
            _logger?.Fatal("Specified file doesn't exist");
            return;
        }

        if (!Directory.Exists(outputDir))
        {
            _logger?.Fatal("Specified output directory doesn't exist");
            return;
        }

        var buffer = new byte[_options.BufferSize];
        var partCount = 0;
        var files = new List<VSDFFile>();
        var tasks = new List<Task>();

        using var stream = File.OpenRead(filePath);

        var tempDir = $"{Path.GetDirectoryName(outputDir)}\\{Guid.NewGuid()}";
        Directory.CreateDirectory(tempDir);

        _logger?.Information("Converting file");

        while (_options.MaxPartCount > partCount)
        {
            if (!CreateImage(stream, buffer, tempDir, tasks, files, ref partCount)) break;
        }

        Task.WaitAll(tasks.ToArray());

        if (_options.MaxPartCount <= partCount)
        {
            _logger?.Fatal("Failed to convert. MaxPartCount reached");
            Directory.Delete(tempDir, true);
            return;
        }

        _logger?.Information("Finished converting");
        _logger?.Information("Writing metadata");

        var metadata = new Metadata(files, Path.GetExtension(filePath), Path.GetFileNameWithoutExtension(filePath));
        metadata.Save(tempDir);

        _logger?.Information("Finished writing metadata");
        _logger?.Information("Packing files");

        if (!PackingUtils.PackFile(tempDir, outputDir, metadata.OriginalFileName))
        {
            _logger?.Fatal("Failed to pack files");
            Directory.Delete(tempDir);
            return;
        }

        _logger?.Information("Finished packing files");
    }

    public void FromVsdf(string filePath, string outputDir)
    {
        _logger?.Information("Unpacking files");

        if (!PackingUtils.UnpackFile(filePath, outputDir, out var tempDir))
        {
            _logger?.Fatal("Failed to unpack files. Check whether specified paths are correct");
            return;
        }

        _logger?.Information("Loading metadata");
        var metadata = Metadata.Load(tempDir);

        if (metadata is null)
        {
            _logger?.Fatal("Failed to load metadata");
            return;
        }

        _logger?.Information("Converting from vsdf");

        using var outputStream =
            File.OpenWrite($"{outputDir}\\{metadata.OriginalFileName}{metadata.OriginalExtension}");

        foreach (var file in metadata.Files.OrderBy(x => x.Order))
        {
            WriteToFileFromImage($"{tempDir}\\{file.Name}", outputStream);
        }

        outputStream.Close();
        Directory.Delete(tempDir, true);

        _logger?.Information("Finished converting");
    }

    private void WriteToFileFromImage(string path, Stream outputStream)
    {
        using var image = Image.Load(path).CloneAs<Rgba32>();

        for (var y = 0; y < image.Height; y++)
        {
            for (var x = 0; x < image.Width; x++)
            {
                var pixel = image[x, y];

                if (pixel.A < _options.RAlpha) continue;

                outputStream.WriteByte(pixel.R);

                if (pixel.A >= _options.RgAlpha)
                {
                    outputStream.WriteByte(pixel.G);
                }

                if (pixel.A >= _options.RgbAlpha)
                {
                    outputStream.WriteByte(pixel.B);
                }
            }
        }
    }

    private bool CreateImage(Stream inputStream, byte[] buffer, string tempDir, ICollection<Task> tasks,
        ICollection<VSDFFile> files, ref int partCount)
    {
        var bytesRead = 0;
        var image = new Image<Rgba32>(_options.Width, _options.Height);

        var colorIndex = 0;
        var color = new Rgba32();

        var posX = 0;
        var posY = 0;

        while (posY < image.Height - 1 && (bytesRead = inputStream.Read(buffer)) > 0)
        {
            for (var i = 0; i < bytesRead; i++)
            {
                switch (colorIndex)
                {
                    case 0:
                        color.R = buffer[i];
                        break;
                    case 1:
                        color.G = buffer[i];
                        break;
                    case 2:
                        color.B = buffer[i];
                        break;
                    default:
                        throw new ArgumentException();
                }

                color.A = colorIndex switch
                {
                    0 => _options.RAlpha,
                    1 => _options.RgAlpha,
                    2 => _options.RgbAlpha,
                    _ => throw new ArgumentException()
                };

                colorIndex++;
                image[posX, posY] = color;

                if (colorIndex >= 3)
                {
                    color = new Rgba32();
                    colorIndex = 0;
                    posX++;
                }

                if (posX < image.Width) continue;

                posX = 0;
                posY += 1;
            }
        }

        var fileName = $"{Guid.NewGuid()}.webp";
        var task = Task.Run(() =>
        {
            image.Mutate(x => x.EntropyCrop());
            image.SaveAsWebp($"{tempDir}\\{fileName}",
                new WebpEncoder { FileFormat = WebpFileFormatType.Lossless, Quality = 100 });
            image.Dispose();
        });
        tasks.Add(task);

        files.Add(new VSDFFile(partCount, fileName));
        partCount++;

        if (tasks.Count < _options.MaxConcurrentFiles) return bytesRead > 0;

        Task.WaitAll(tasks.ToArray());
        tasks.Clear();

        return bytesRead > 0;
    }
}