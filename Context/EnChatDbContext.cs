using System;
using System.Collections.Generic;
using EnChat.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace EnChat.Context;

public partial class EnChatDbContext : DbContext
{
    public EnChatDbContext()
    {
    }

    public EnChatDbContext(DbContextOptions<EnChatDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pending> Pendings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(Helper.ReadEmbeddedAssets("CString").GetAwaiter().GetResult(), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.3-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Pending>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("pending");

            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.From).HasMaxLength(36);
            entity.Property(e => e.To).HasMaxLength(36);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("user", tb => tb.HasComment("User info"));

            entity.HasIndex(e => e.Username, "username_unique").IsUnique();

            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.Contacts).HasColumnName("contacts");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Pending).HasColumnName("pending");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
