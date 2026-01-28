using Amnyam._1_Domain.Interfaces;
using Amnyam.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Amnyam._2_Application.Commands.RemoveVoiceRoomSettingsCommand;

public class RemoveVoiceRoomSettingsCommandHandler(
    ILogger<RemoveVoiceRoomSettingsCommandHandler> logger,
    IRoomSettingsRepository roomVoiceSettingsRepository) : IRequestHandler<RemoveVoiceRoomSettingsCommand, BaseResult>
{
    public async Task<BaseResult> Handle(RemoveVoiceRoomSettingsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await roomVoiceSettingsRepository.RemoveRoomSettingsByGuildMemberDiscordIdAsync(request.GuildMemberDiscordId, cancellationToken);

            return BaseResult.Success(
                "Настройки личной комнаты были успешно удалены");
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке удалить настройки личной комнаты для участника с DiscordId {GuildMemberDiscordId}",
                request.GuildMemberDiscordId);

            return BaseResult.Fail(
                "Не удалось удалить настройки личной комнаты из-за ошибки базы данных. Пожалуйста, попробуйте еще раз позже.", 
                new Error(_1_Domain.Enums.ErrorCodes.DB_ERROR, 
                ex.Message));
        }
        catch(ArgumentNullException ex)
        {
            logger.LogError(
                ex,
                "Настройки для участника с DiscordId {GuildMemberDiscordId} не были найдены",
                request.GuildMemberDiscordId);

            return BaseResult.Fail(
                "Не удалось удалить настройки личной комнаты, так как они не были заданы", 
                new Error(_1_Domain.Enums.ErrorCodes.NOT_FOUND, 
                ex.Message));
        }
        
    }
}
