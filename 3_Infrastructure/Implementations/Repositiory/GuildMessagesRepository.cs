using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Interfaces;
using Amnyam._3_Infrastructure.DataBase.EF;

namespace Amnyam._3_Infrastructure.Implementations.Services;

public class GuildMessagesRepository(
    ILogger<GuildMessagesRepository> logger,
    MlkAdminDbContext mlkAdminDbContext) : IGuildMessagesRepository
{
    public async Task AddMessageAsync(GuildMessage message, CancellationToken token)
    {
        if (message is null)
        {
            logger.LogWarning(
                "Переданная сущность message является null");

            return;
        }

        await mlkAdminDbContext.GuildMessages.AddAsync(message, token);
        await mlkAdminDbContext.SaveChangesAsync(token);

        logger.LogInformation(
            "Сообщение {messageId} успешно записано в базу данных",
            message.MessageDiscordId);
		
    }

    public async Task<IReadOnlyCollection<string?>> GetMessagesColectionByMemberAsync(ulong guildMemberDiscordId)
    {
        var collection = await mlkAdminDbContext.GuildMessages
            .Where(msg => msg.SenderDiscordId == guildMemberDiscordId && msg.Content != string.Empty)
            .OrderBy(msg => msg.SentAt)
            .Select(msg => msg.Content)
            .Take(100)
            .ToListAsync();

        return collection.AsReadOnly();
    }
}
