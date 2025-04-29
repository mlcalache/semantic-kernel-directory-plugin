// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using Microsoft.SemanticKernel;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Logging.Abstractions;
// using Kernel = Microsoft.SemanticKernel.Kernel;

// // Inject your logger 
// // see Microsoft.Extensions.Logging.ILogger @ https://learn.microsoft.com/dotnet/core/extensions/logging
// ILoggerFactory myLoggerFactory = NullLoggerFactory.Instance;

// var builder = Kernel.CreateBuilder();
// builder.Services.AddSingleton(myLoggerFactory);

// var kernel = builder.Build();

// // Load configuration from secrets (for your Azure OpenAI keys)
// var configuration = new ConfigurationBuilder()
//     .SetBasePath(Directory.GetCurrentDirectory())
//     .AddUserSecrets<Program>()
//     .Build();

// string? modelId = configuration["SemanticKernel:ModelId"];
// string? endpoint = configuration["SemanticKernel:Endpoint"];
// string? apiKey = configuration["SemanticKernel:ApiKey"];

// var kernelBuilder = Kernel.CreateBuilder();

// kernelBuilder.AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);

// // FunPlugin directory path
// var funPluginDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
// // var funPluginDirectoryPath = Path.Combine("Plugins", "FunnyJoke");

// // Load the FunPlugin from the Plugins Directory
// var funPluginFunctions = kernel.ImportPluginFromPromptDirectory(funPluginDirectoryPath);

// // Construct arguments
// var arguments = new KernelArguments() { ["input"] = "time travel to dinosaur age" };

// // Run the Function called Joke
// var result = await kernel.InvokeAsync(funPluginFunctions["FunnyJoke"], arguments);

// // Return the result to the Notebook
// Console.WriteLine(result);




using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Kernel = Microsoft.SemanticKernel.Kernel;
using System;
using System.IO;
using Microsoft.SemanticKernel.TextGeneration;

// Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .Build();

string? modelId = configuration["SemanticKernel:ModelId"];
string? endpoint = configuration["SemanticKernel:Endpoint"];
string? apiKey = configuration["SemanticKernel:ApiKey"];

if (string.IsNullOrWhiteSpace(modelId) || string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("❌ Missing required Azure OpenAI configuration.");
    return;
}

// Logger (optional, for logging)
ILoggerFactory loggerFactory = NullLoggerFactory.Instance;

// Kernel builder setup (single builder!)
var builder = Kernel.CreateBuilder();

// Setup the Azure OpenAI text generation service
builder.AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey); // Ensure this line is added

// Setup DI for kernel and logger
builder.Services.AddSingleton<ILoggerFactory>(loggerFactory);

// Build kernel
var kernel = builder.Build();

// Plugin path
var pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
var plugin = kernel.ImportPluginFromPromptDirectory(pluginPath);

// Confirm plugin functions
Console.WriteLine("Loaded plugin functions:");
foreach (var f in plugin)
{
    Console.WriteLine($"- {f.Name}");
}

// Check if the function exists
if (!plugin.TryGetFunction("FunnyJoke", out var jokeFunction))
{
    Console.WriteLine("❌ Could not find function 'FunnyJoke' in plugin.");
    return;
}

// Construct arguments for the function
var arguments = new KernelArguments { ["input"] = "time travel to dinosaur age" };

// Execute the function
try
{
    var result = await jokeFunction.InvokeAsync(kernel, arguments);
    Console.WriteLine("✅ Result:");
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error invoking function: {ex.Message}");
}