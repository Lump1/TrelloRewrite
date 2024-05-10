﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trello.Models;

public partial class CheloDbContext : DbContext
{
    public CheloDbContext()
    {
    }

    public CheloDbContext(DbContextOptions<CheloDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<BoardStatusColumn> BoardStatusColumns { get; set; }

    public virtual DbSet<BoardTag> BoardTags { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardTag> CardTags { get; set; }

    public virtual DbSet<StatusColumn> StatusColumns { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamUser> TeamUsers { get; set; }

    public virtual DbSet<UserCard> UserCards { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=chelo_db;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("board_pkey");

            entity.ToTable("board");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.IdTeamNavigation).WithMany(p => p.Boards)
                .HasForeignKey(d => d.IdTeam)
                .HasConstraintName("board_id_team_fkey");
        });

        modelBuilder.Entity<BoardStatusColumn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("board_status_column_pkey");

            entity.ToTable("board_status_column");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdBoard).HasColumnName("id_board");
            entity.Property(e => e.IdStatusColumn).HasColumnName("id_status_column");

            entity.HasOne(d => d.IdBoardNavigation).WithMany(p => p.BoardStatusColumns)
                .HasForeignKey(d => d.IdBoard)
                .HasConstraintName("board_status_column_id_board_fkey");

            entity.HasOne(d => d.IdStatusColumnNavigation).WithMany(p => p.BoardStatusColumns)
                .HasForeignKey(d => d.IdStatusColumn)
                .HasConstraintName("board_status_column_id_status_column_fkey");
        });

        modelBuilder.Entity<BoardTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("board_tags_pkey");

            entity.ToTable("board_tags");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdBoard).HasColumnName("id_board");
            entity.Property(e => e.IdTags).HasColumnName("id_tags");

            entity.HasOne(d => d.IdBoardNavigation).WithMany(p => p.BoardTags)
                .HasForeignKey(d => d.IdBoard)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("board_tags_id_board_fkey");

            entity.HasOne(d => d.IdTagsNavigation).WithMany(p => p.BoardTags)
                .HasForeignKey(d => d.IdTags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("board_tags_id_tags_fkey");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("card_pkey");

            entity.ToTable("card");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Deadline).HasColumnName("deadline");
            entity.Property(e => e.IdBoard).HasColumnName("id_board");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdBoardNavigation).WithMany(p => p.Cards)
                .HasForeignKey(d => d.IdBoard)
                .HasConstraintName("card_id_board_fkey");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Cards)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("card_id_status_fkey");
        });

        modelBuilder.Entity<CardTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("card_tags_pkey");

            entity.ToTable("card_tags");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCard).HasColumnName("id_card");
            entity.Property(e => e.IdTags).HasColumnName("id_tags");

            entity.HasOne(d => d.IdCardNavigation).WithMany(p => p.CardTags)
                .HasForeignKey(d => d.IdCard)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("card_tags_id_card_fkey");

            entity.HasOne(d => d.IdTagsNavigation).WithMany(p => p.CardTags)
                .HasForeignKey(d => d.IdTags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("card_tags_id_tags_fkey");
        });

        modelBuilder.Entity<StatusColumn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("status_column");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('status_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tags_pkey");

            entity.ToTable("tags");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.ToTable("task");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCard).HasColumnName("id_card");
            entity.Property(e => e.Iscompleted).HasColumnName("iscompleted");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdCardNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdCard)
                .HasConstraintName("task_id_card_fkey");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_pkey");

            entity.ToTable("team");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TeamUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_user_pkey");

            entity.ToTable("team_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Role)
                .HasMaxLength(25)
                .HasDefaultValueSql("'USER'::character varying")
                .HasColumnName("role");

            entity.HasOne(d => d.IdTeamNavigation).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.IdTeam)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("team_user_id_team_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("team_user_id_user_fkey");
        });

        modelBuilder.Entity<UserCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_card_pkey");

            entity.ToTable("user_card");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCard).HasColumnName("id_card");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdCardNavigation).WithMany(p => p.UserCards)
                .HasForeignKey(d => d.IdCard)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_card_id_card_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserCards)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_card_id_user_fkey");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_info_pkey");

            entity.ToTable("user_info");

            entity.HasIndex(e => e.Email, "user_info_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Guid)
                .HasMaxLength(55)
                .HasColumnName("guid");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
