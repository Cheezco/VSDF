using CommandLine;
using CommandLine.Text;
using Spectre.Console;
using VSDFConverter;
using VSDFConverter.Options;

AnsiConsole.Write(
    new FigletText("VSDF Converter")
        .Centered()
        .Color(Color.Red));

var parser = new Parser(with =>
{
    with.CaseInsensitiveEnumValues = true;
    with.IgnoreUnknownArguments = true;
});

var parserResult = parser.ParseArguments<FileConverterOptions>(args);
parserResult
    .WithParsed(FileConverter.Run)
    .WithNotParsed(_ =>
    {
        var helpText = HelpText.AutoBuild(parserResult, h =>
        {
            h.AdditionalNewLineAfterOption = false;
            h.Heading = "VSDF Converter";
            return HelpText.DefaultParsingErrorsHandler(parserResult, h);
        }, e => e);

        Console.WriteLine(helpText);
    });