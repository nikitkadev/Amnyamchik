using Newtonsoft.Json;
using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._3_Infrastructure.Providers.Models.Guild;

public class RolesListModel
{
    [JsonProperty(nameof(GuildRoles))]
    public List<GuildRoleInfo> GuildRoles { get; set; } = [];
}



