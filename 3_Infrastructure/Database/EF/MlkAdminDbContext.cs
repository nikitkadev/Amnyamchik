using Microsoft.EntityFrameworkCore;
using MlkAdmin._1_Domain.Entities;

namespace MlkAdmin._3_Infrastructure.DataBase.EF;

public class MlkAdminDbContext(DbContextOptions<MlkAdminDbContext> options) : DbContext(options)
{
    public DbSet<GuildMember> GuildMembers { get; set; }
    public DbSet<GuildVoiceChannel> VChannels { get; set; }
    public DbSet<GuildTextChannel> TChannels { get; set; }
    public DbSet<GuildMessage> GuildMessages { get; set; }
    public DbSet<GuildVoiceSession> GuildVoiceSessions { get; set; }
    public DbSet<GuildMemberMetric> GuildMemberMetrics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        GuildMemberModelCreating(modelBuilder);
        GuildMessagModelCreating(modelBuilder);
        GuildTextChannelModelCreating(modelBuilder);
        GuildVoiceChannelModelCreating(modelBuilder);
        GuildVoiceSessionModelCreating(modelBuilder);
        GuildMemberMetricsModelCreating(modelBuilder);
    }

    private static void GuildMemberModelCreating(ModelBuilder builder)
    {
        builder.Entity<GuildMember>()
            .ToTable("guild_member")
            .HasKey(entity => entity.Id);

        builder.Entity<GuildMember>()
            .Property(prop => prop.DiscordId)
            .HasColumnName("discord_id")
            .IsRequired(true);

        builder.Entity<GuildMember>()
            .Property(prop => prop.DisplayName)
            .HasColumnName("display_name")
            .IsRequired(true);

        builder.Entity<GuildMember>()
            .Property(prop => prop.IsAuthorized)
            .HasColumnName("is_authorized")
            .IsRequired(true);

        builder.Entity<GuildMember>()
            .Property(prop => prop.JoinedAt)
            .HasColumnName("joined_at")
            .IsRequired();

        builder.Entity<GuildMember>()
            .Property(prop => prop.VoiceRoomName)
            .HasColumnName("voice_room_name")
            .IsRequired(false);

        builder.Entity<GuildMember>()
            .Property(prop => prop.RealName)
            .HasColumnName("real_name")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Entity<GuildMember>()
            .Property(prop => prop.TgName)
            .HasColumnName("tg_name")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Entity<GuildMember>()
            .Property(prop => prop.Birthday)
            .HasColumnName("birthday")
            .IsRequired(false);
    }
    private static void GuildMessagModelCreating(ModelBuilder builder)
    {
        builder.Entity<GuildMessage>()
            .ToTable("guild_messages")
            .HasKey(entity => entity.Id);

        builder.Entity<GuildMessage>()
            .Property(prop => prop.MessageDiscordId)
            .HasColumnName("message_discord_id")
            .IsRequired(true);

        builder.Entity<GuildMessage>()
            .Property(prop => prop.SenderDiscordId)
            .HasColumnName("sender_discord_id")
            .IsRequired(true);

        builder.Entity<GuildMessage>()
            .Property(prop => prop.Content)
            .HasColumnName("content")
            .IsRequired(false);

        builder.Entity<GuildMessage>()
            .Property(prop => prop.SentAt)
            .HasColumnName("sent_at")
            .IsRequired(true);

        builder.Entity<GuildMessage>()
            .Property(prop => prop.TChannelId)
            .HasColumnName("from_t_channel_id")
            .IsRequired(true);

        builder.Entity<GuildMessage>()
            .Property(prop => prop.TChannelName)
            .HasColumnName("from_t_channel_name")
            .IsRequired(true);
    }
    private static void GuildTextChannelModelCreating(ModelBuilder builder)
    {
        builder.Entity<GuildTextChannel>()
            .ToTable("text_channels")
            .HasKey(entity => entity.Id);

        builder.Entity<GuildTextChannel>()
            .Property(prop => prop.DiscordId)
            .HasColumnName("discord_id")
            .IsRequired(true);

        builder.Entity<GuildTextChannel>()
            .Property(prop => prop.Name)
            .HasColumnName("name")
            .IsRequired(true);

        builder.Entity<GuildTextChannel>()
            .Property(prop => prop.Category)
            .HasColumnName("category")
            .IsRequired(false);
    }
    private static void GuildVoiceChannelModelCreating(ModelBuilder builder)
    {
        builder.Entity<GuildVoiceChannel>()
            .ToTable("voice_channels")
            .HasKey(entity => entity.Id);

        builder.Entity<GuildVoiceChannel>()
            .Property(prop => prop.DiscordId)
            .HasColumnName("discord_id")
            .IsRequired(true);

        builder.Entity<GuildVoiceChannel>()
            .Property(prop => prop.Name)
            .HasColumnName("name")
            .IsRequired(true);

        builder.Entity<GuildVoiceChannel>()
            .Property(prop => prop.Category)
            .HasColumnName("category")
            .IsRequired(false);

        builder.Entity<GuildVoiceChannel>()
            .Property(prop => prop.IsGen)
            .HasColumnName("is_gen")
            .IsRequired(true);

        builder.Entity<GuildVoiceChannel>()
            .Property(prop => prop.IsTemp)
            .HasColumnName("is_temp")
            .IsRequired(true);
    }
    private static void GuildVoiceSessionModelCreating(ModelBuilder builder)
    {
        builder.Entity<GuildVoiceSession>()
            .ToTable("guild_voice_session")
            .HasKey(entity => entity.Id);

        builder.Entity<GuildVoiceSession>()
            .Property(prop => prop.VChannelDiscordId)
            .HasColumnName("vchannel_discord_id")
            .IsRequired(true);

        builder.Entity<GuildVoiceSession>()
            .Property(prop => prop.Name)
            .HasColumnName("name")
            .IsRequired(true);

        builder.Entity<GuildVoiceSession>()
            .Property(prop => prop.StartingAt)
            .HasColumnName("starting_at")
            .IsRequired(true);

        builder.Entity<GuildVoiceSession>()
            .Property(prop => prop.TotalSeconds)
            .HasColumnName("time_in_seconds")
            .IsRequired(true);
    }
    private static void GuildMemberMetricsModelCreating(ModelBuilder builder)
    {
        builder.Entity<GuildMemberMetric>()
            .ToTable("guilb_member_metrics")
            .HasKey(key => key.MemberDiscordId);

        builder.Entity<GuildMemberMetric>()
            .Property(prop => prop.MessageSentCount)
            .HasColumnName("messages_sent_count")
            .IsRequired(true);

        builder.Entity<GuildMemberMetric>()
            .Property(prop => prop.ReactionAddedCount)
            .HasColumnName("reaction_added_count")
            .IsRequired(true);

        builder.Entity<GuildMemberMetric>()
            .Property(prop => prop.CommandsSentCount)
            .HasColumnName("commands_sent_count")
            .IsRequired(true);

        builder.Entity<GuildMemberMetric>()
            .Property(prop => prop.StickersSentCount)
            .HasColumnName("stickers_sent_count")
            .IsRequired(true);

        builder.Entity<GuildMemberMetric>()
            .Property(prop => prop.GifsSentCount)
            .HasColumnName("gifs_sent_count")
            .IsRequired(true);

        builder.Entity<GuildMemberMetric>()
            .Property(prop => prop.PngPicturesSentCount)
            .HasColumnName("pngpictures_sent_count")
            .IsRequired(true);

        builder.Entity<GuildMemberMetric>()
            .Property(prop => prop.FirstMessage)
            .HasColumnName("first_message_date")
            .IsRequired(false);

        builder.Entity<GuildMemberMetric>()
            .Property(prop => prop.LastMessage)
            .HasColumnName("last_message_date")
            .IsRequired(false);
    }
}
