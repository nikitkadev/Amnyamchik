using Discord.WebSocket;
using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._2_Application.Interfaces.Managers;

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