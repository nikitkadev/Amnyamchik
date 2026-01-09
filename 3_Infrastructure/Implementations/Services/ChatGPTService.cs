using Microsoft.Extensions.Logging;
using MlkAdmin._4_Presentation.Interfaces;
using OpenAI.Chat;

namespace MlkAdmin._3_Infrastructure.Implementations.Services;

public class ChatGPTService(ILogger<ChatGPTService> logger,
    ChatClient chatClient) : IChatGPTService
{
    public async Task<string> ResponseAsync(string memberPrompt)
    {
		try
		{
            return (await chatClient.CompleteChatAsync(memberPrompt)).Value.Content[0].Text;
        }
		catch (Exception exception)
		{
            logger.LogError(
                exception,
                "");

            return exception.Message;
        }
    }
}
