using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Amnyam._1_Domain.Enums;
using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Exceptions;
using Amnyam._1_Domain.Interfaces;
using Amnyam._2_Application.Interfaces.Services;
using Amnyam._2_Application.Interfaces.Managers;
using Amnyam.Shared.Dtos;
using Amnyam.Shared.Results;
using Amnyam.Shared.Constants;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._2_Application.Implementations.Managers;

public class GuildMembersManager(
    ILogger<GuildMembersManager> logger,
    IJsonProvidersHub providersHub,
    IGuildRolesService roleService,
    IGuildMessagesManager messagesManager,
    IGuildMembersRepository membersRepository,
    IGuildMemberMetricRepository metricRepository,
    IAnalysisService analysisService) : IGuildMembersManager
{
    public async Task<BaseResult<GuildMemberAnalysisResultData>> AnalyzeGuildMemberAsync(ulong guildMemberDiscordId)
    {
        try
        {
            var dbMember = await membersRepository.GetGuildMemberEntityAsync(guildMemberDiscordId);

            logger.LogInformation(
                "Сущность участника успешно получена из базы данных для участника с DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            var metrics = await metricRepository.GetGuildMemberMetricAsync(guildMemberDiscordId);

            logger.LogInformation(
                "Метрика успешно получена из базы данных для участника с DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            var aiAnalysis = await analysisService.GetGuildMemberAIAnalysisAsync(guildMemberDiscordId);

            logger.LogInformation(
                "AI-анализ успешно получен для участника с DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            return BaseResult<GuildMemberAnalysisResultData>.Success(
                new GuildMemberAnalysisResultData()
                {
                    GuildMemberDiscordId = guildMemberDiscordId,

                    JoinedAt = dbMember.JoinedAt,
                    FirstMessageDate = metrics.FirstMessage,
                    LastMessageDate = metrics.LastMessage,
                    DaysSinceJoined = (int)(DateTimeOffset.UtcNow - dbMember.JoinedAt).TotalDays,
                    MessageCount = metrics.MessageSentCount,
                    ReactionCount = metrics.ReactionAddedCount,
                    GifsSentCount = metrics.GifsSentCount,
                    CommandsSentCount = metrics.CommandsSentCount,
                    VoiceChannelsTimeSpent = await membersRepository.GetTotalSecondsInVoiceChannelsByMemberDiscordIdAsync(guildMemberDiscordId),

                    AvgToxicity = aiAnalysis.AvgToxicity,
                    MostToxicMessage = aiAnalysis.MostToxicMessage,
                    SpeechStyle = aiAnalysis.SpeechStyle,
                    Tonality = aiAnalysis.Tonality,
                    AvgCharsInMessage = aiAnalysis.AvgCharsInMessage
                });

        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке получить данные по участнику c DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            return BaseResult<GuildMemberAnalysisResultData>.Fail(
                new Error(
                    ErrorCodes.DB_ERROR, 
                    ex.Message));
                
        }
    }
    public async Task<BaseResult> AuthorizeGuildMemberAsync(ulong guildMemberDiscordId, string guildMemberMention)
    {
        try
        {
            if (await membersRepository.IsAuthorizedAsync(guildMemberDiscordId))
            {
                logger.LogWarning(
                    "Участник с DiscordId {GuildMemberDiscordId} уже авторизован на сервере",
                    guildMemberDiscordId);

                return BaseResult.Success(
                    "Вы уже авторизованы на сервере!");
            }

            await roleService.RemoveRoleAsync(
                guildMemberDiscordId,
                providersHub.GuildConfigProvidersHub.Roles.GetGuildRoleByKey("NotAuthorized").Id);

            await roleService.AssignRolesAsync(
                guildMemberDiscordId,
                [
                    providersHub.GuildConfigProvidersHub.Roles.GetGuildRoleByKey("GuildMember").Id,
                    providersHub.GuildConfigProvidersHub.Roles.GetGuildRoleByKey("PlayersClub").Id
                ]
            );

            await membersRepository.UpsertGuildMemberAsync(
                new GuildMember()
                {
                    DiscordId = guildMemberDiscordId,
                    IsAuthorized = true
                }
            );

            logger.LogInformation(
                "Участник с DiscordId {GuildMemberDiscordId} был успешно авторизован",
                guildMemberDiscordId);

        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке авторизовать пользователя с DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            return BaseResult.Fail(
                MlkAdminConstants.ERROR_CLIENT_MESSAGE,
                new Error(
                    ErrorCodes.INTERNAL_ERROR, 
                    ex.Message)
            );
        }
        catch(GuildMemberNotFoundException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке авторизовать пользователя с DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            return BaseResult.Fail(
                "Пользователь не найден",
                new Error(
                    ErrorCodes.INTERNAL_ERROR,
                    ex.Message)
            );
        }

        try
        {
            await messagesManager.SendLogMessageAsync(
                new LogMessageDto()
                {
                    Title = "Авторизация",
                    Message = $"Участник {guildMemberMention} был успешно авторизован на сервере",
                    Created = DateTimeOffset.UtcNow
                }
            );

            return BaseResult.Success(
            "Вы были успешно авторизованы на сервере!");
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке отправить лог об успешной авторизации участника сервера с DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            return BaseResult.Fail(
                MlkAdminConstants.ERROR_CLIENT_MESSAGE,
                new Error(
                    ErrorCodes.INTERNAL_ERROR,
                    ex.Message)
            );
        }
    }
    public async Task<BaseResult> DeauthorizeGuildMemberAsync(ulong guildMemberDiscordId, string guildMemberName)
    {
        try
        {
            await membersRepository.RemoveGuildMemberEntityFromDbAsync(guildMemberDiscordId);

            logger.LogInformation(
                "Пользователь {memberDiscordId} успешно удален из базы данных",
                guildMemberDiscordId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке удалить участник с DiscordId {GuildMemberDiscordId} из базы данных",
                guildMemberDiscordId);

            return BaseResult.Fail(
                MlkAdminConstants.ERROR_CLIENT_MESSAGE,
                new Error(
                    ErrorCodes.INTERNAL_ERROR, 
                    ex.Message));
        }

        try
        {
            await messagesManager.SendLogMessageAsync(
                new LogMessageDto()
                {
                    Title = "Деавторизация",
                    Message = $"Участник {guildMemberName} успешно деавторизован на сервере",
                    Created = DateTimeOffset.UtcNow
                }
            );

            return BaseResult.Success("Пользователь был деавторизован");
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке отправить лог о деавторизации участника {GuildMemberName}",
                guildMemberName);

            return BaseResult.Fail(
                "Произошла внутренняя ошибка при попытке отправить сообщение в логи!",
                new Error(
                    ErrorCodes.INTERNAL_ERROR, 
                    ex.Message));
        }
    }
    public async Task<BaseResult> UpdateGuildMemberColorRoleAsync(ulong guildMemberDiscordId, string guildColorRoleKey)
    {
        try
        {
            var colorRolesIds = providersHub.GuildConfigProvidersHub.Roles.GetColorRolesIds();

            await roleService.RemoveRolesByFilterModeAsync(guildMemberDiscordId, colorRolesIds, true);

            if(guildColorRoleKey == "remove_color")
            {
                logger.LogInformation(
                    "Все цветные роли были удалены у участника с DiscordId {GuildMemberDiscordId}",
                    guildMemberDiscordId);

                return BaseResult.Success(
                    "Цвет имени успешно удален");
            }

            var selectedColorRoleId = providersHub.GuildConfigProvidersHub.Roles.GetGuildRoleByKey(guildColorRoleKey).Id;

            await roleService.AssignRoleAsync(guildMemberDiscordId, selectedColorRoleId);

            logger.LogInformation(
                "Добавлена цветовая роль {GuildColorRoleDiscordId} участнику {GuildMemberDiscordId}",
                selectedColorRoleId, 
                guildMemberDiscordId);

            return BaseResult.Success(
                "Цвет имени успешно изменен");
        }
        catch (GuildMemberNotFoundException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке обновить цветовую роль участнику c DiscordId {GuildMemberDiscordId}",
                guildMemberDiscordId);

            return BaseResult.Fail(
                "Участник не найден",
                new Error(
                    ErrorCodes.NOT_FOUND,
                    ex.Message));
        }
    }
    public async Task<BaseResult> WelcomeNewMemberAsync(GuildMember guildMember)
    {
        try
        {
            await membersRepository.UpsertGuildMemberAsync(guildMember);
            await roleService.AssignRoleAsync(guildMember.DiscordId, providersHub.GuildConfigProvidersHub.Roles.GetGuildRoleByKey("NotAuthorized").Id);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке добавить нового участника {GuildMemberName}:{GuildMemberDiscordId} в базу данных или назначить роль",
                guildMember.DisplayName,
                guildMember.DiscordId);

            return BaseResult.Fail(
                MlkAdminConstants.ERROR_CLIENT_MESSAGE,
                new Error(
                    ErrorCodes.INTERNAL_ERROR,
                    ex.Message));
        }

        try
        {
            await Task.Delay(1000);

            await messagesManager.SendWelcomeMessageAsync(guildMember.DiscordId);

            return BaseResult.Success(
                "Новый участник успешно поприветствован");
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке отправить приветственное сообщение");

            return BaseResult.Fail(
                MlkAdminConstants.ERROR_CLIENT_MESSAGE,
                new Error(
                    ErrorCodes.INTERNAL_ERROR,
                    ex.Message));
        }
    }
}