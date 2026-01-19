namespace Amnyam.Shared.Dtos;

public class SelectionMenuConfigDto
{
    public string Placeholder { get; set; } = string.Empty;
    public string CustomId { get; set; } = string.Empty;
    public List<SelectOptionConfigDto> Options { get; set; } = [];
}

public class SelectOptionConfigDto
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
