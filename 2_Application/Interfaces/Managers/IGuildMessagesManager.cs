using Discord.WebSocket;
using Amnyam.Shared.Dtos;

namespace Amnyam._2_Application.Interfaces.Managers;

public interface IGuildMessagesManager
{
    Task RefreshDynamicMessagesAsync();
    Task SendDefaultResponseAsync(SocketMessageComponent messageAction, string clientMessage);
    Task SendDefaultResponseAsync(SocketSlashCommand messageAction, string clientMessage);
    Task SendRulesMessageResponseAsync(SocketMessageComponent messageAction);
    Task SendWelcomeMessageAsync(ulong newMemberDiscordId);
    Task SendColorsMenuResponseAsync(SocketMessageComponent messageAction);
    Task SendLogMessageAsync(LogMessageDto logMessageDto);
    Task SendAnalyzeResultMessageAsync(SocketSlashCommand messageAction, GuildMemberAnalysisResultData data);
}