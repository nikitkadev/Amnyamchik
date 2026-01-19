using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Amnyam.Shared.JsonProviders;

public abstract class JsonProviderBase<T>(string path, ILogger logger) : IJsonProvider<T> where T : class
{
    protected T? _config = null;

    private void Load()
    {
		try
		{
			_config = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

			logger.LogInformation(
				"Конфигурация загружена из {path}", 
				path);
		}
		catch (Exception exception)
		{
			logger.LogError(
				exception,
				"Ошибка при загрузки конфигурации из {Path}",
                path);

		}
    }

	public T GetConfig()
	{
		if (_config is null) 
			Load();

		return _config ?? throw new InvalidDataException("Не удалось загрузить конфинг");
	}
}
