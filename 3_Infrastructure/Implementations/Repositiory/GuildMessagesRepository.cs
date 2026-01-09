using Microsoft.Extensions.Logging;
using MlkAdmin._1_Domain.Entities;
using MlkAdmin._1_Domain.Enums;
using MlkAdmin._1_Domain.Interfaces;
using MlkAdmin._3_Infrastructure.DataBase.EF;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._3_Infrastructure.Implementations.Services;

public class GuildMessagesRepository(
    ILogger<GuildMessagesRepository> logger,
    MlkAdminDbContext mlkAdminDbContext) : IGuildMessagesRepository
{
    public async Task<BaseResult> AddMessageAsync(GuildMessage message, CancellationToken token)
    {
		try
		{
            if (message is null)
            {
                logger.LogWarning(
                    "Переданная сущность message является null");

                return BaseResult.Fail(
                    "Сообщение является null", 
                    new(
                        ErrorCodes.VARIABLE_IS_NULL, 
                        "Неизвестно как, но сообщение null"));
            }

            await mlkAdminDbContext.GuildMessages.AddAsync(message, token);
            await mlkAdminDbContext.SaveChangesAsync(token);

            logger.LogInformation(
                "Сообщение {messageId} успешно записано в базу данных",
                message.MessageDiscordId);

            return BaseResult.Success(
                "Сущность message успешно записана в базу данных");
        }
		catch (Exception exception)
		{
            logger.LogError(
                exception,
                "Ошибка при попытке добавить запись о сообщение в базу данных\nСообщение: {ErrorMessage}",
                exception.Message);


            return BaseResult.Fail(
                    "Ошибка при попытке добавить запись о сообщение в базу данных",
                    new(
                        ErrorCodes.VARIABLE_IS_NULL,
                        exception.Message));
        }
    }
}
