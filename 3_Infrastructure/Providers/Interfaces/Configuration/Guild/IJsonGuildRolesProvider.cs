using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Guild;

public interface IJsonGuildRolesProvider
{
    List<GuildRoleInfo> GuildRoles { get; }
    List<ulong> GetColorRolesIds();
    GuildRoleInfo GetGuildRoleByKey(string roleKey);
}
