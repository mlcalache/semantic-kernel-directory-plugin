# 🏠 Real Estate Chat Search — Semantic Kernel + Azure OpenAI

This project is my **first experiment** using [Microsoft Semantic Kernel](https://github.com/microsoft/semantic-kernel) with [Azure OpenAI Service](https://learn.microsoft.com/en-us/azure/ai-services/openai/) in a practical scenario.

The use case simulates a **real estate listings application**, where users can **interact with an AI agent** to search and filter property listings in the Netherlands using natural language.

---

## ✨ Project Goals

- Learn the basics of integrating **Semantic Kernel** with **Azure OpenAI**.
- Explore **AI-powered chat interfaces** for filtering structured data.
- Build a small demo using **mocked real estate listings**.
- Apply best practices around configuration and secrets using `.NET user-secrets`.

---

## 📦 Tech Stack

- [.NET 8 Console Application](https://learn.microsoft.com/en-us/dotnet/core/)
- [Semantic Kernel](https://github.com/microsoft/semantic-kernel) (C# SDK)
- [Azure OpenAI via Azure AI Studio](https://learn.microsoft.com/en-us/azure/ai-services/openai/overview)
- [Azure AI Foundry](https://aka.ms/azureaifoundry)
- `System.Text.Json` for JSON deserialization
- `Microsoft.Extensions.Configuration` for managing secrets and settings

---

## 🧠 AI Capabilities

The project uses **Semantic Kernel Plugins** to enable chat-based querying of property listings.

We can ask things like:

> Find houses for sale in Amsterdam with at least 3 bedrooms and energy label A.
> List apartments available for rent under €1500 in Utrecht.

The plugin parses the intent and filters data accordingly from a local mock database.

---

## 🏗️ Project Structure

```
/data/
└── real_estate_data.json           # Mocked property data in Dutch cities

/Plugins/
└── ListingPlugin.cs                # Semantic Kernel plugin for property queries

/Repositories/
└── RealEstateRepository.cs         # Repository for managing listings (CRUD)

/Models/
└── RealEstateListing.cs            # Core entity representing a real estate object

Program.cs                          # Initializes config, SK, and runs the demo
```

---

## 🔐 Configuration & Secrets

All sensitive data like Azure endpoints and keys are stored using **.NET user secrets**:

```bash
dotnet user-secrets set "SemanticKernel:Endpoint" "https://your-endpoint"
dotnet user-secrets set "SemanticKernel:ApiKey" "your-api-key"
dotnet user-secrets set "SemanticKernel:ModelId" "your-model-id"
```

These values are loaded into the app using IConfiguration.

## 🗃️ Real Estate Data

All listing data is mocked.

Data includes properties like city, province, zip code, energy label, bedrooms, pricing, and more.

Properties are localized to the Netherlands.

## 📹 Demo


- [Demo available on YouTube](https://youtu.be/tupiUTkqohs?si=1P4YtlSqBNgY2_R6)


## 📚 References

- Microsoft Semantic Kernel

- Azure OpenAI Service

- Azure AI Foundry

- Getting started with Semantic Kernel in C#

## 🚀 Next Steps

Add natural language processing for multilingual input

Integrate with real data APIs or CRM systems

Deploy as a web API or chat UI interface

Support richer queries with embeddings or vector search

### Tech details

#### Temperature

Temperature is a parameter that controls the randomness of the model's output.

It affects the probability distribution over the possible next tokens (words or characters) that the model can generate.

Range: The temperature can be set from 0 to 1 or higher (though typically values between 0 and 1 are used).

Temperature = 0: This makes the model output deterministic responses. The model will choose the most probable next token at each step, leading to more predictable and less creative outputs. This is useful when we want clear, concise, and reliable answers.

Temperature = 1: This allows for more randomness and creative responses. The model will explore a wider range of possible next tokens, resulting in more diverse outputs.

Temperature > 1: This will increase the randomness further, though it can lead to less coherent or sensible responses.

In this case, setting Temperature = 0 means we are asking for responses that are as predictable and structured as possible.

Summary: Temperature = 0: Predictable, deterministic output with little randomness.

#### TopP (nucleus sampling)

Top-p, also known as nucleus sampling, is a method for sampling from the model's output distribution.

Instead of considering all possible tokens (words) when generating the next token, top-p sampling restricts the set of possible tokens to a subset where the cumulative probability is less than or equal to p.

Top-P = 1: This means that the model considers the entire probability distribution of possible tokens. Effectively, this is similar to the default behavior where the model has access to all potential words (no truncation), meaning it can generate more diverse and potentially creative responses.

Top-P < 1: This limits the model to a subset of tokens whose cumulative probability is less than or equal to the value of p. For example, if we set top-p to 0.9, the model would only consider the smallest set of tokens whose probabilities sum to 90%, effectively cutting out less likely options. This often results in more coherent responses but can make them less creative.

So, when we set TopP = 1, we're allowing the model to consider the full distribution of words (meaning it can generate a wide range of possible outputs).

Summary: Top-P = 1 means a full probability distribution of tokens, allowing for a wider variety of potential outputs, though the responses are still determined by the top choices the model makes.

## 👋 Author

Created by Matheus de Lara Calache as an educational project and hands-on experiment with AI + .NET.

Feel free to fork, star, or contribute!
