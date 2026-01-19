using Newtonsoft.Json;
using Amnyam.Shared.Dtos;

namespace Amnyam._3_Infrastructure.Providers.Models.Guild;

public class RolesListModel
{
    [JsonProperty(nameof(GuildRoles))]
    public List<GuildRoleInfo> GuildRoles { get; set; } = [];
}



