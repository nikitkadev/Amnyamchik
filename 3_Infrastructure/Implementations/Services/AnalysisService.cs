using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Interfaces;
using Amnyam._2_Application.Interfaces.Services;
using Amnyam._4_Presentation.Interfaces;
using Amnyam.Shared.Dtos;

namespace Amnyam._3_Infrastructure.Implementations.Services;

public class AnalysisService(ILogger<AnalysisService> logger,
    IGuildMessagesRepository messagesRepository,
    IChatGPTService chatService) : IAnalysisService
{
    public async Task<GuildMemberAIAnalysisResultDto?> GetGuildMemberAIAnalysisAsync(ulong guildMemberDiscordId)
    {
        try
        {
            var messages = await messagesRepository.GetMessagesColectionByMemberAsync(guildMemberDiscordId);

            if (messages is null || messages.Count == 0)
            {
                logger.LogWarning(
                    "Сообщений от участника с DiscordId {GuildMemberDiscordId} не найдено",
                    guildMemberDiscordId);

                return null;
            }

            var analysisAIResult = await chatService.AnalyzeWithAIGuildMemberAsync(messages);

            return analysisAIResult;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке провести AI-анализ для участника с DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            return null;
        }
    }
}
