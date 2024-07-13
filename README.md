# Setup
ANTLR is used to generate the lexer and parser for Luie based on its [grammar](./LUIECompiler/Luie.g4). It can be downloaded from the [official website](https://www.antlr.org/download.html). The website also contains information on how to install ANTLR and generate files for different languages.

The compiler is written in C# and uses .NET 8. For building the compiler, the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is required.

## Generate files
The following command can be used to generate the lexer and parser files for Luie:
```bash
antlr4 -Dlanguage=CSharp -listener Luie.g4
```

Alternatively, you can use the Visual Studio Code extension "ANTLR4 grammar syntax support" and set the settings in the `settings.json` file to generate the files automatically. An example of the settings is shown below:
```json
    "antlr4.generation": {
        "mode": "external",
        "language": "CSharp",
        "listeners": true,
        "visitors": false
    },
```