using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Interfaces;
using Amnyam._2_Application.Interfaces.Managers;
using Amnyam._2_Application.Interfaces.Services;

namespace Amnyam._2_Application.Implementations.Managers;


public class GuildVoiceSessionsManager(
    ILogger<GuildVoiceSessionsManager> logger,
    IGuildVoiceSessionCacheService voiceSessionCache,
    IGuildVoiceSessionRepository sessionRepository) : IGuildVoiceSessionsManager
{
    private async Task SaveVoiceSessionAsync(ulong guildMemberDiscordId, ulong sessionChannelDiscordId, string sessionChannelName, DateTimeOffset? startDate)
    {
        if (startDate is null)
            return;

        var endingAt = DateTimeOffset.UtcNow;

        var sessionEntity = new GuildVoiceSession()
        {
            GuildMemberDiscordId = guildMemberDiscordId,
            StartingAt = startDate,
            EndingAt = endingAt,
            VChannelDiscordId = sessionChannelDiscordId,
            VChannelName = sessionChannelName,
            TotalSeconds = (long)(endingAt - startDate).Value.TotalSeconds
        };

        await sessionRepository.AddGuildVoiceSessionAsync(sessionEntity);

    }

    public async Task HandleVoiceStateUpdateAsync(SocketVoiceChannel? newVoiceChannel, SocketVoiceChannel? oldVoiceChannel, ulong guildMemberDiscordId)
    {
        try
        {
            if (newVoiceChannel is not null)
            {
                if(voiceSessionCache.AlreadySet(guildMemberDiscordId) && oldVoiceChannel is not null)
                {
                    await SaveVoiceSessionAsync(
                        guildMemberDiscordId, 
                        oldVoiceChannel.Id, 
                        oldVoiceChannel.Name, 
                        voiceSessionCache.GetVoiceSessionStartByMemberDiscordId(guildMemberDiscordId));

                    voiceSessionCache.RemoveVoiceSessionFromDictionary(guildMemberDiscordId);
                }

                voiceSessionCache.SetVoiceSessionStart(guildMemberDiscordId);
            }

            if (newVoiceChannel is null)
            {
                if(oldVoiceChannel is not null)
                {
                    await SaveVoiceSessionAsync(
                        guildMemberDiscordId,
                        oldVoiceChannel.Id,
                        oldVoiceChannel.Name,
                        voiceSessionCache.GetVoiceSessionStartByMemberDiscordId(guildMemberDiscordId));

                    voiceSessionCache.RemoveVoiceSessionFromDictionary(guildMemberDiscordId);
                }
            }
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке обрабатать голосовую сессию для участника с DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            return;
        }
    }
}
