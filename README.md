# Semantic Kernel Plugin Runner (.NET 8 + Azure OpenAI)

This project demonstrates how to use [Microsoft Semantic Kernel 1.47.0](https://github.com/microsoft/semantic-kernel) in a .NET console application to load and invoke a plugin defined using `config.json` and `skprompt.txt` from a local directory. It integrates with **Azure OpenAI** for text generation.

## Features

- Loads a plugin from a directory using the `ImportPluginFromPromptDirectory` method.
- Executes a prompt-based function (defined by `skprompt.txt`) using Azure OpenAI.
- Injects necessary services for Semantic Kernel operation.
- Reads OpenAI credentials from user secrets.

---

## Project Structure

```
Semantic-Kernel-Directory-Plugin/
│ 
├── Plugins/
│   └── FunnyJoke/
│       ├── config.json # Function definition
│       └── skprompt.txt # Prompt template
├── Program.cs # Main application logic
├── README.md
└── .csproj
```

---

## Plugin Example (FunnyJoke)

**config.json**

```json
{
  "schema_version": "1.0",
  "name": "FunnyJoke",
  "description": "Creates a funny joke.",
  "type": "completion",
  "input_parameters": [
    {
      "name": "input",
      "description": "Topic for the joke.",
      "default": ""
    }
  ],
  "execution_settings": {
    "azure_openai": {
      "temperature": 0.2,
      "max_tokens": 256
    }
  }
}
```

**skprompt.txt**

```json
Tell me a joke about {{$input}}.
```

## 🛠️ Prerequisites

.NET 8 SDK

An Azure OpenAI account with a deployed model (e.g., gpt-35-turbo)

User secrets configured with:

```json
{
  "SemanticKernel": {
    "ModelId": "your-model-id",
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "your-api-key"
  }
}
```

To set these secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "SemanticKernel:ModelId" "your-model-id"
dotnet user-secrets set "SemanticKernel:Endpoint" "https://your-endpoint/"
dotnet user-secrets set "SemanticKernel:ApiKey" "your-api-key"

```

## Running the App

1. Restore packages:

```bash
dotnet restore
```

2. Build the project:

```bash
dotnet build
```

3. Run it:

```bash
dotnet run
```

If successful, you'll see the result from the FunnyJoke plugin printed to the console.

## Dependencies

Microsoft.SemanticKernel v1.47.0

Microsoft.SemanticKernel.Connectors.OpenAI

Microsoft.Extensions.Configuration

Microsoft.Extensions.Logging.Abstractions

Install with:

```bash
dotnet add package Microsoft.SemanticKernel --version 1.47.0
dotnet add package Microsoft.SemanticKernel.Connectors.OpenAI
```

## Learn More

- [Semantic Kernel Docs](https://learn.microsoft.com/semantic-kernel)
- [Prompt Engineering](https://learn.microsoft.com/semantic-kernel/prompts/prompt-template-format)
- [Azure OpenAI Service](https://learn.microsoft.com/azure/cognitive-services/openai/)
