# Команды
## Общие положения
- **Источник**: `Discord API (HTTP REST)`.
- **Обработка**: через `Discord.NET`.
- **Логика обработки**: Создание slash-команд происходит в [`DiscordSlashCommandsService`](../4_Presentation/Implementations/DiscordSlashCommandsService.cs). Обработчики находятся в слое **Application** в папке `2_Application/Commands`.

## Список команд
### `/set-voiceroom`
- **Описание**: Настраивает параметры создаваемой личной голосовой комнаты.
- **Права**: Все участники.
- **Параметры**:
    - `voice_name`- имя создаваемой комнаты. 
        > __type__: string<br>
        > __max_lenght__: 20
    - `members_limit` - лимит пользователей в канале.
        > __type__: int<br>
        > __min_members__: 1<br>
        > __max_members__: 99
    - `region` - регион подключения.
        > __type__: string<br>
    - `is_private` - канал с возрастными ограничениями. 
        > __type__: bool<br>
