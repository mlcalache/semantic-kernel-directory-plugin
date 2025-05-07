using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Kernel = Microsoft.SemanticKernel.Kernel;

public static class KernelHostBuilder
{
    public static Kernel BuildKernel()
    {
        // Inject your logger 
        // see Microsoft.Extensions.Logging.ILogger @ https://learn.microsoft.com/dotnet/core/extensions/logging
        // ILoggerFactory myLoggerFactory = NullLoggerFactory.Instance;
        
        var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddUserSecrets<Program>()
                    .Build();

        // Load configuration from secrets (for your Azure OpenAI keys)
        string? modelId = configuration["SemanticKernel:ModelId"];
        string? endpoint = configuration["SemanticKernel:Endpoint"];
        string? apiKey = configuration["SemanticKernel:ApiKey"];

        var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey)
            .Build();
        return kernel;
    }
}