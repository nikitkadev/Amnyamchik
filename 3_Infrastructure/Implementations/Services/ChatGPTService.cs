using Microsoft.Extensions.Logging;
using Amnyam._4_Presentation.Interfaces;
using Amnyam.Shared.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenAI.Chat;
using System.Globalization;

namespace Amnyam._3_Infrastructure.Implementations.Services;

public class ChatGPTService(
    ILogger<ChatGPTService> logger,
    ChatClient chatClient) : IChatGPTService
{
    public async Task<GuildMemberAIAnalysisResultDto> AnalyzeWithAIGuildMemberAsync(IReadOnlyCollection<string?> guildMemberMessages)
    {
        try
        {
            var chatToSendMessages = new List<ChatMessage>()
            {
                ChatMessage.CreateSystemMessage(
                (
                    "Ты — AI‑анализатор социального взаимодействия на сервере Malenkie. " +
                    "Проанализируй сообщения участника и верни JSON с полями: \n" +
                    "- AvgToxicity: уровень токсичности (float, 0.0–1.0).\n" +
                    "- MostToxicMessage: самое токсичное сообщение (string, '-' если нет токсичности).\n" +
                    "- SpeechStyle: стиль речи одним словом (string).\n" +
                    "- Tonality: тональность одним словом (string).\n" +
                    "- AvgCharsInMessage: среднее количество символов в слове (float).\n\n" +
                    "Требования:\n" +
                    "- Отвечай ТОЛЬКО JSON, без пояснений.\n" +
                    "- Все поля обязательны.\n" +
                    "- Если данных недостаточно, ставь null или '-'.\n" +
                    "Ниже список переданных сообщений от пользователя: "
                )),
                ChatMessage.CreateUserMessage(
                    string.Join("\n", guildMemberMessages.Select((m, i) => $"[{i + 1}] {m}")))
            };

            var completion = (await chatClient.CompleteChatAsync(
                chatToSendMessages,
                new ChatCompletionOptions()
                {
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()

                }));

            var aiAnalysisResult = JsonConvert.DeserializeObject<GuildMemberAIAnalysisResultDto>(
                completion.Value.Content[0].Text, 
                new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        IgnoreIsSpecifiedMembers = true,
                        IgnoreSerializableAttribute = true
                    },
                    Culture = CultureInfo.InvariantCulture,
                    NullValueHandling = NullValueHandling.Include,
                    Formatting = Formatting.None
                });

            if (aiAnalysisResult is null)
            {
                logger.LogWarning(
                    "Десериализация JSON завершилась с null. Исходный JSON: {Json}",
                    completion);

                throw new JsonException("Десериализация вернула null");
            }

            return aiAnalysisResult;
        }
        catch (JsonException ex)
        {
            logger.LogError(
                "Ошибка десериализации JSON: {Error}",
                ex.Message);

            throw; 
        }
        catch (Exception ex)
        {
            logger.LogError(
                "Ошибка при обработке JSON: {Error}",
                ex.Message);

            throw;
        }
    }
}
