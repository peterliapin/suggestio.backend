using Suggestio.Api.Configuration;
using Suggestio.Api.Exceptions;
using Suggestio.Api.Interfaces;
using OpenAI;
using OpenAI.Chat;

namespace Suggestio.Api.Services;

public class GptService : IGptService
{

    private readonly OpenAIConfig openAIConfig = new();

    public GptService(IConfiguration configuration)
    {
        var settings = configuration.GetSection("OpenAI").Get<OpenAIConfig>();

        if (settings != null)
        {
            openAIConfig = settings;
        }
        else
        {
            throw new MissingConfigurationException($"The specified configuration section for the type {typeof(OpenAIConfig).FullName} could not be found in the settings file.");
        }
    }

    public async Task<string> GenerateResponse(string prompt, int maxTokens)
    {
        var client = new ChatClient(model: "gpt-4o", openAIConfig.ApiKey);

        var options = new ChatCompletionOptions()
        {
            MaxOutputTokenCount = maxTokens
        };

        var messages = new List<UserChatMessage>
        {
            new UserChatMessage(prompt)
        };

        var completion = await client.CompleteChatAsync(messages, options);

        return completion.Value.Content[0].Text;
    }
}
