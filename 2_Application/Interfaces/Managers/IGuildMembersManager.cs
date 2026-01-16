using MlkAdmin._1_Domain.Entities;
using MlkAdmin.Shared.Dtos;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._2_Application.Interfaces.Managers;

public interface IGuildMembersManager
{
    Task<BaseResult> AuthorizeGuildMemberAsync(ulong guildMemberDiscordId, string guildMemberMention);
    Task<BaseResult> DeauthorizeGuildMemberAsync(ulong guildMemberDiscordId, string guildMemberGlobalName);
    Task<BaseResult> UpdateGuildMemberColorRoleAsync(ulong guildMemberDiscordId, string guildRoleKey);
    Task<BaseResult<GuildMemberAnalysisResultData>> AnalyzeGuildMemberAsync(ulong guildMemberDiscordId);
    Task<BaseResult> WelcomeNewMemberAsync(GuildMember guildMemberEntity);
}
