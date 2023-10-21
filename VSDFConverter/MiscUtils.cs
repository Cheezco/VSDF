using Serilog;
using Serilog.Core;
using Serilog.Sinks.Spectre;

namespace VSDFConverter;

public static class MiscUtils
{
    public static Logger CreateLogger()
    {
        return new LoggerConfiguration()
            .WriteTo.Spectre()
            .CreateLogger();
    }
}