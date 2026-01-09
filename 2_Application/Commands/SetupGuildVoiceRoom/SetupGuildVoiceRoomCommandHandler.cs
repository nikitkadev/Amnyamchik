using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MlkAdmin._1_Domain.Exceptions;
using MlkAdmin._1_Domain.Interfaces;
using MlkAdmin._1_Domain.Enums;
using MlkAdmin.Shared.Constants;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._2_Application.Commands.SetupGuildVoiceRoom;

public class SetupGuildVoiceRoomCommandHandler(
    ILogger<SetupGuildVoiceRoomCommandHandler> logger,
    IGuildMembersRepository membersRepository) : IRequestHandler<SetupGuildVoiceRoomCommand, BaseResult>
{
    public async Task<BaseResult> Handle(SetupGuildVoiceRoomCommand request, CancellationToken token)
    {
        try
        {
            await membersRepository.UpdateVoiceRoomNameAsync(request.GuildMemberDiscordId, request.GuildVoiceRoomName, token);

            logger.LogInformation(
                "Имя голосовой комнаты успешно изменено для участника {GuildMemberDiscordId}",
                request.GuildMemberDiscordId);

            return BaseResult.Success(
                $"Вы успешно поменяли имя личной комнаты на {request.GuildVoiceRoomName}!");
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