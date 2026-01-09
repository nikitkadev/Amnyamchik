using MlkAdmin._1_Domain.Entities;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._2_Application.Interfaces.Managers;

public interface IGuildMembersManager
{
    Task<BaseResult> AuthorizeGuildMemberAsync(ulong memberId, string guildMemberMention);
    Task<BaseResult> DeauthorizeGuildMemberAsync(ulong memberDiscordId, string globalName);
    Task<BaseResult> UpdateGuildMemberColorRoleAsync(ulong guildMemberId, string guildRoleKey);
    Task<BaseResult> WelcomeNewMemberAsync(GuildMember guildMemberId);
}
