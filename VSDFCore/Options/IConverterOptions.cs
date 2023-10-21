namespace VSDFCore.Options;

public interface IConverterOptions
{
    public int BufferSize { get; set; }
    public int MaxPartCount { get; set; }
    public int MaxConcurrentFiles { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public byte RAlpha { get; set; }
    public byte RgAlpha { get; set; }
    public byte RgbAlpha { get; set; }
    public bool DebugMode { get; set; }
}