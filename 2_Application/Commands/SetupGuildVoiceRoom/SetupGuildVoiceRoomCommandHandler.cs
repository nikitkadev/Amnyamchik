using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Amnyam._1_Domain.Exceptions;
using Amnyam._1_Domain.Interfaces;
using Amnyam._1_Domain.Enums;
using Amnyam.Shared.Constants;
using Amnyam.Shared.Results;

namespace Amnyam._2_Application.Commands.SetupGuildVoiceRoom;

public class SetupGuildVoiceRoomCommandHandler(
    ILogger<SetupGuildVoiceRoomCommandHandler> logger,
    IRoomSettingsRepository settingsRepository) : IRequestHandler<SetupGuildVoiceRoomCommand, BaseResult>
{
    public async Task<BaseResult> Handle(SetupGuildVoiceRoomCommand request, CancellationToken token)
    {
        try
        {
            await settingsRepository.UpsertRoomSettingsAsync(
                new _1_Domain.Entities.RoomSettings()
                {
                    GuildMemberDiscordId = request.GuildMemberDiscordId,
                    VoiceRoomName = request.RoomName,
                    MembersLimit = request.MembersLimit,
                    Region = request.Region,
                    IsNSFW = request.IsNSFW,
                    SlowModeLimit = request.SlowModeLimit
                }, token);

            logger.LogInformation(
                "Настройки успешно заданы для участника с DiscordId {GuildMemberDiscordId}",
                request.GuildMemberDiscordId);

            return BaseResult.Success(
                $"Успешная настройка для личной комнаты!");
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке обновить имя голосовой комнаты для пользователя {GuildMemberDiscordId}",
                request.GuildMemberDiscordId);

            return BaseResult.Fail(MlkAdminConstants.ERROR_CLIENT_MESSAGE,
                new Error(
                    ErrorCodes.INTERNAL_ERROR, 
                    ex.Message));
        }
        catch(GuildMemberNotFoundException ex)
        {
            logger.LogError(ex, 
                "Попытка изменить имя комнаты для несуществующего участника {GuildMemberDiscordId}",
                ex.GuildMemberDiscordId);

            return BaseResult.Fail(
                "Участник сервера не найден",
                new Error(
                    ErrorCodes.NOT_FOUND,
                    ex.Message));
        }
    }
}