# VSDF Converter

VSDF Converter is a tool that allows you to compress and decompress files using the VSDF format. The VSDF format is designed to efficiently store and retrieve files while minimizing storage space.

## Getting Started

To use the VSDF Converter, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to restore dependencies and compile the project.

Or you can download prebuild binaries from releases

## Usage

The VSDF Converter provides a command-line interface for performing file conversion operations. Here are the available options:

VSDFConverter [inputFile] [outputDir] [options]

- `inputFile`: The path to the input file.
- `outputDir`: The directory where the converted files will be saved.

### Options

- `--bufferSize`: The buffer size used for file operations. Default is 2048.
- `--maxPartCount`: The maximum number of parts to split the file into during compression. Default is 200.
- `--maxConcurrentFiles`: The maximum number of files to process concurrently. Default is 3.
- `--width`: The width of the image. All parts will have the same width. Default is 3000.
- `--height`: The height of the image. All parts will have the same height. Default is 3500.
- `--rAlpha`: The alpha value when only the R component contains information. Default is 245.
- `--rgAlpha`: The alpha value when both the R and G components contain information. Default is 246.
- `--rgbAlpha`: The alpha value when all components contain information. Default is 247.
- `--debug`: Enable debug mode. Default is false.

### Examples

To compress a file using the VSDF format:

`VSDFConverter --op ToVsdf input.txt output/`

To decompress a VSDF file:

`VSDFConverter --op FromVsdf input.vsdf output/`

## Format

VSDF file is just renamed zip file that contains lossless webp images with information and metadata.json.

### Images

Each image pixel can contain 3 bytes of information. Useful bytes are specified by pixel alpha value.

### Metadata

Metadata file contains image order, their file names, creation date, original extension and original file name

## Contributing

Contributions to the VSDF Converter are welcome! If you find a bug or have a feature request, please open an issue on the GitHub repository.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
