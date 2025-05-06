using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Kernel = Microsoft.SemanticKernel.Kernel;

public class Program(string[] args)
{
    public static async Task Main(string[] args)
    {
        // Inject your logger 
        // see Microsoft.Extensions.Logging.ILogger @ https://learn.microsoft.com/dotnet/core/extensions/logging
        ILoggerFactory myLoggerFactory = NullLoggerFactory.Instance;

        // Load configuration from secrets (for your Azure OpenAI keys)
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>()
            .Build();

        string? modelId = configuration["SemanticKernel:ModelId"];
        string? endpoint = configuration["SemanticKernel:Endpoint"];
        string? apiKey = configuration["SemanticKernel:ApiKey"];

        var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey)
            .Build();

        var funPluginDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Plugins", "FunPlugin");

        CreateFileBasedPluginTemplate(funPluginDirectoryPath);

        var funPlugin = kernel.ImportPluginFromPromptDirectory(funPluginDirectoryPath, "FunPlugin");

                // Invoke the plugin with a prompt
        var result = await kernel.InvokeAsync(funPlugin["Joke"], new()
        {
            ["input"] = "Why did the chicken cross the road?",
            ["style"] = "dad joke"
        });

        Console.WriteLine(result.GetValue<string>());
    }

    private static void CreateFileBasedPluginTemplate(string pluginRootDirectory)
    {
        // Create the sub-directory for the plugin function "Joke"
        var pluginRelativeDirectory = Path.Combine(pluginRootDirectory, "Joke");

        const string ConfigJsonFileContent =
            """
            {
              "schema": 1,
              "description": "Generate a funny joke",
              "execution_settings": {
                "default": {
                  "max_tokens": 1000,
                  "temperature": 0.9,
                  "top_p": 0.0,
                  "presence_penalty": 0.0,
                  "frequency_penalty": 0.0
                }
              },
              "input_variables": [
                {
                  "name": "input",
                  "description": "Joke subject",
                  "default": ""
                },
                {
                  "name": "style",
                  "description": "Give a hint about the desired joke style",
                  "default": ""
                }
              ]
            }
            """;

        const string SkPromptFileContent =
            """
            WRITE EXACTLY ONE JOKE or HUMOROUS STORY ABOUT THE TOPIC BELOW
            JOKE MUST BE:
            - G RATED
            - WORKPLACE/FAMILY SAFE
            NO SEXISM, RACISM OR OTHER BIAS/BIGOTRY
            BE CREATIVE AND FUNNY. I WANT TO LAUGH.
            Incorporate the style suggestion, if provided: {{$style}}
            +++++
            {{$input}}
            +++++
            """;

        // Create the directory structure
        if (!Directory.Exists(pluginRelativeDirectory))
        {
            Directory.CreateDirectory(pluginRelativeDirectory);
        }

        // Create the config.json file if not exists
        var configJsonFilePath = Path.Combine(pluginRelativeDirectory, "config.json");
        if (!File.Exists(configJsonFilePath))
        {
            File.WriteAllText(configJsonFilePath, ConfigJsonFileContent);
        }

        // Create the skprompt.txt file if not exists
        var skPromptFilePath = Path.Combine(pluginRelativeDirectory, "skprompt.txt");
        if (!File.Exists(skPromptFilePath))
        {
            // Create the skprompt file with the content
            File.WriteAllText(skPromptFilePath, SkPromptFileContent);
        }
    }
}
