namespace MlkAdmin._4_Presentation.Interfaces;

public interface IChatGPTService
{
    Task<string> ResponseAsync(string memberPrompt);
}
