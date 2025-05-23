using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

public class Program(string[] args)
{
    public static async Task Main(string[] args)
    {
        Kernel kernel = KernelHostBuilder.BuildKernel();

        var funPluginDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Plugins", "FunPlugin");

        CreateFileBasedPluginTemplate(funPluginDirectoryPath);

        var funPlugin = kernel.ImportPluginFromPromptDirectory(funPluginDirectoryPath, "FunPlugin");

        // Start the chat loop
        string? userInput;
        do
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("User > ");
            userInput = Console.ReadLine();

            // Invoke the plugin with a prompt
            var result = await kernel.InvokeAsync(funPlugin["Joke"], new()
            {
                ["input"] = userInput,
                ["style"] = "dad joke"
            });

            Console.WriteLine(result.GetValue<string>());

        } while (true);
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
