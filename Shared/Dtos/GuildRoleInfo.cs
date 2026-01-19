using Amnyam._1_Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Amnyam.Shared.Dtos;

public class GuildRoleInfo
{
    public ulong Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Description { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RoleType Type { get; set; }
}
