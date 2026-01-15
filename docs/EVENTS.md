# События 

## Общие положения
- **Источник**: `Discord Gateway API (WebSocket)`.
- **Обработка**: через `Discord.NET`.
- **Формат данных**: `JSON (Gateway)` → `SocketEventArgs (Discord.NET)`.
- **Логика обработки**: подписки на события происходят в [`DiscordEventsService`](../4_Presentation/Implementations/DiscordEventsService.cs). Обработчики находятся в слое **Application** в папке `2_Application/Events`.

## Список событий

## `ButtonExecuted`
- **Описание**: Участник нажал кнопку.
- **Условия**: -
- **Параметры**:
    - `SocketMessageComponent` - компонент с детальной информацией о нажатой кнопке.
- **Пример обработчика**:
```
public async Task Handle(ButtonExecuted notification, CancellationToken cancellationToken)
{
    try
    {
        await notification.SocketMessageComponent.DeferAsync(ephemeral: true);
        
        var socketGuildUser = notification.SocketMessageComponent.User as SocketGuildUser 
            ?? throw new InvalidOperationException("Пользователь не является участником сервера");

        switch (notification.SocketMessageComponent.Data.CustomId)
        {
            case MlkAdminConstants.BUTTONS_AUTHORIZATION_CUSTOM_ID:

                var authResult = await membersManager.AuthorizeGuildMemberAsync(
                    socketGuildUser.Id, 
                    socketGuildUser.Mention);

                await messagesManager.SendDefaultResponseAsync(
                    notification.SocketMessageComponent, 
                    authResult.ClientMessage);

                break;

    ...
```

- **Где используется**: 
    - При приветствие нового пользователя для перехода в Hub-канал. 
    - В Hub-сообщение для **Авторизации**, получение списка **правил** и для отправки формы получения **цвета для сервер-имени**. 

## `GuildAvailable`
- **Описание**: Сервер становится доступным для приложения после периода недоступности (при перезагрузке).
- **Условия**: -
- **Параметры**:
    - `SocketGuild` - объект с детальной информацией о доступном сервере.
- **Пример обработчика**:
```
public async Task Handle(GuildAvailable notification, CancellationToken token)
{
    await initializationService.InitializeAsync(notification.SocketGuild.Id, token);

    logger.LogInformation(
        "Успешная инициализация сервера");
}
```
- **Где используется**: 
    - Инициализация сущностей сервера (каналы/роли).
    - Выполнение некоторой бизнес-логики при перезагрузке приложения.

## `MessageReceived`
- **Описание**: Участник отправил сообщение.
- **Условия**: Участник не является ботом.
- **Параметры**:
    - `SocketMessage` - объект с детальной информацией об отправленном сообщение.
- **Пример обработчика**:
```
public async Task Handle(MessageReceived notification, CancellationToken token)
{
    try
    {
        if (notification.SocketMessage.Author.IsBot)
            return;


        await messageRepository.AddMessageAsync(
            new GuildMessage()
            {
                SenderDiscordId = notification.SocketMessage.Author.Id,
                MessageDiscordId = notification.SocketMessage.Id,
                SentAt = DateTime.UtcNow,
                TChannelId = notification.SocketMessage.Channel.Id,
                Content = notification.SocketMessage.Content ?? null
            }, 
            token
        );

        await metricRepository.UpdateLastMessageDateAsync(notification.SocketMessage.Author.Id);
        await metricRepository.IncrementMessageSentCountAsync(notification.SocketMessage.Author.Id);

        var attachments = notification.SocketMessage.Attachments;

        if (notification.SocketMessage.Stickers.Count > 0)
            await metricRepository.IncrementStickerCountAsync(notification.SocketMessage.Author.Id);

        if (attachments.Any(message => message.Filename.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)))
            await metricRepository.IncrementGifSentCountAsync(notification.SocketMessage.Author.Id);

        if (attachments.Any(message => message.Filename.EndsWith(".png", StringComparison.OrdinalIgnoreCase)))
            await metricRepository.IncrementPngPictureSentCountAsync(notification.SocketMessage.Author.Id);
    ...
```
- **Где используется**: 
    - Статистика по отправленным сообщениям и вложениям.

## `ReactionAdded`
- **Описание**: Участник поставил реакцию.
- **Условия**: Участник не является ботом.
- **Параметры**:
    - `Cacheable<IUserMessage, ulong> Message` - сообщение, на которое была добавлена реакция.
    - `Cacheable<IMessageChannel, ulong> Channel` - канал, в котором находилось сообщение.
    - `SocketReaction` - объект с детально информацией о реакции.
- **Пример обработчика**:
```
public async Task Handle(ReactionAdded notification, CancellationToken cancellationToken)
{
	try
	{
		if (notification.Reaction.User.Value.IsBot)
			return;

		await metricRepository.IncrementReactionAddedCountAsync(notification.Reaction.UserId);
	}
	catch (DbUpdateException ex)
	{
		logger.LogError(
			ex,
			"Ошибка при попытке взаимодействия с базой данных");

		return;
	}
}
```
- **Где используется**: 
    - Статистика по добавленным реакциям на сообщения.
    - Ранее события использовалось для авторизации участников.

## `Ready`
- **Описание**: Приложение полностью подключилось к `Discord Gateway` и готово к работе.
- **Условия**: -
- **Параметры**: -
- **Пример обработчика**:
```
public async Task Handle(Ready notification, CancellationToken cancellationToken)
{
    try
    {
        await commandsService.RegistrateCommandsAsync();
    }
    catch (Exception exception)
    {
        logger.LogError(
            exception, 
            "Error: {Message} StackTrace: {StackTrace}", 
            exception.Message, 
            exception.StackTrace);
    }
}
```
- **Где используется**: 
    - Добавление команд на сервер.

## `SelectMenuExecuted`
- **Описание**: Участник взаимодействует с выпадающим меню.
- **Условия**: -
- **Параметры**: 
    - `SocketMessageComponent` - объект с детальной информацией о выпадающем меню.
- **Пример обработчика**:

```
public async Task Handle(SelectMenuExecuted notification, CancellationToken cancellationToken)
{
    try
    {
        await notification.SocketMessageComponent.DeferAsync();

        var values = notification.SocketMessageComponent.Data.Values;

        switch (notification.SocketMessageComponent.Data.CustomId)
        {
            case MlkAdminConstants.SELECTION_MENU_COLORS_CUSTOM_ID:

                var changeResult = await membersManager.UpdateGuildMemberColorRoleAsync(
                    notification.SocketMessageComponent.User.Id,
                    values.First());

                await messagesManager.SendDefaultResponseAsync(
                    notification.SocketMessageComponent, 
                    changeResult.ClientMessage);

                break;
    ...
```
- **Где используется**: 
    - Автополучение роли для смены цвета имени на сервере.

## `SlashCommandExecuted`
- **Описание**: Участник отправляет slash-команду.
- **Условия**: Зависит от команды.
- **Параметры**: 
    - `SocketSlashCommand` - объект с детальной информацией о slash-команде.
- **Пример обработчика**:
```
public async Task Handle(SlashCommandExecuted notification, CancellationToken token)
{
    await notification.SocketSlashCommand.DeferAsync(ephemeral: true);

    var command = notification.SocketSlashCommand;

    if (command.Channel.Id != providersHub.GuildConfigProvidersHub.Channels.TextChannels.BotCommandsText.DiscordId)
    {
        await messagesManager.SendDefaultResponseAsync(
            notification.SocketSlashCommand, 
            $"Команды бота можно вызывать только в канале {providersHub.GuildConfigProvidersHub.Channels.TextChannels.BotCommandsText.Https}");

        return;
    }

    switch (command.CommandName)
    {
        case MlkAdminConstants.SET_VOICEROOM_COMMAND_NAME:

            try
            {
                var voiceRoomName = command.Data.Options
                    .FirstOrDefault(
                        x => x.Name.Equals("name", StringComparison.Ordinal)).Value.ToString() ?? string.Empty;

                var updateVoiceRoomNameResult = await mediator.Send(
                    new SetupGuildVoiceRoomCommand()
                    {
                        GuildMemberDiscordId = command.User.Id,
                        GuildVoiceRoomName = voiceRoomName,
                    },
                    token
                );

                await messagesManager.SendDefaultResponseAsync(notification.SocketSlashCommand, updateVoiceRoomNameResult.ClientMessage);

                break;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Ошибка при попытке обработать команду {CommandName} настройки личной голосовой комнаты",
                    MlkAdminConstants.SET_VOICEROOM_COMMAND_NAME);

                break;
            }
    ...
```
- **Где используется**: 
    - Задать настройки личной комнаты.
    - Получить детальный анализ по действиям на сервере.

## `UserJoined`
- **Описание**: Пользователь присоединяется к серверу.
- **Условия**: Пользователь не бот.
- **Параметры**: 
    - `SocketGuildUser` - объект с детальной информацией о пользователе.
- **Пример обработчика**:
```
public async Task Handle(UserJoined notification, CancellationToken cancellationToken)
{
    try
    {
        if (notification.SocketGuildUser.IsBot)
        {
            logger.LogInformation(
                "Участник {MemberName} является ботом",
                notification.SocketGuildUser.GlobalName);

            return;
        }

        await membersManager.WelcomeNewMemberAsync(
            new GuildMember()
            {
                DiscordId = notification.SocketGuildUser.Id,
                DisplayName = notification.SocketGuildUser.DisplayName,
                JoinedAt = notification.SocketGuildUser.JoinedAt ?? DateTimeOffset.Now,
                IsAuthorized = false
            });
    }
    catch (Exception ex)
    {
        logger.LogError(
            ex, 
            "Ошибка при работе события UserJoinedHandler");
    }
}
```
- **Где используется**: 
    - Приветствие нового участника.

## `UserLeft`
- **Описание**: Участник покидает сервер.
- **Условия**: -
- **Параметры**: 
    - `SocketGuild` - объект с детальной информацией о покинутой гильдии.
    - `SocketUser` - объект с детальной информацией о бывшем участнике.
- **Пример обработчика**:
```
public async Task Handle(UserLeft notification, CancellationToken cancellationToken)
{
    try
    {
        await membersManager.DeauthorizeGuildMemberAsync(notification.SocketUser.Id, notification.SocketUser.GlobalName);
    }
    catch (Exception ex)
    {
        logger.LogError(
            ex,
            "Ошибка про попытке обработать уход участника {GuildMemberDiscordName}:{GuildMemberDiscordId} с сервера",
            notification.SocketUser.GlobalName, 
            notification.SocketUser.Id);

        return;
    }
}
```
- **Где используется**: 
    - Деавторизация пользователя путем удаление информации о нем из базы данных.

## `UserVoiceStateUpdated`
- **Описание**: Пользователь меняет свое состояние относительно голосового канала (заходи/выходит/меняет канал).
- **Условия**: Пользователь не бот.
- **Параметры**: 
    - `SocketGuild` - объект с детальной информацией о покинутой гильдии.
    - `SocketUser` - объект с детальной информацией о бывшем участнике.
- **Пример обработчика**:
```
public async Task Handle(UserVoiceStateUpdated notification, CancellationToken token)
{
    try
    {
        if (notification.SocketUser is not SocketGuildUser guildUser)
            return;


        if (notification.OldState.VoiceChannel != null)
        {
            if(await channelsRepository.IsTemporaryVoiceChannel(notification.OldState.VoiceChannel.Id, token) && notification.OldState.VoiceChannel.ConnectedUsers.Count == 0)
            {
                await channelsRepository.RemoveDbVoiceChannelAsync(notification.OldState.VoiceChannel.Id);
                await notification.OldState.VoiceChannel.DeleteAsync();
            }
        }

        if (notification.NewState.VoiceChannel != null)
        {
            if (await channelsRepository.IsGeneratingVoiceChannel(notification.NewState.VoiceChannel.Id, token))
            {
                var brandNewRestChannel = await channelsService.CreateVoiceChannelAsync(notification.SocketUser.Id);

                var dbVChannel = new GuildVoiceChannel()
                {
                    Category = notification.NewState.VoiceChannel.Category.ToString(),
                    DiscordId = brandNewRestChannel.Id,
                    Name = brandNewRestChannel.Name,
                    IsGen = false,
                    IsTemp = true
                };

                await channelsRepository.UpsertDbVoiceChannelAsync(dbVChannel);
                await guildUser.ModifyAsync(properties => properties.ChannelId = brandNewRestChannel.Id);
            }
        }
    }
    ...
```
- **Где используется**: 
    - Создание авто-лобби.
    - Подсчет времени, проведенных в голосовых каналах.

[Прыгнуть](#события)