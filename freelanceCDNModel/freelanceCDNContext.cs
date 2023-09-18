﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CDNAPI.freelanceCDNModel;

public partial class freelanceCDNContext : DbContext
{
    public freelanceCDNContext()
    {
    }

    public freelanceCDNContext(DbContextOptions<freelanceCDNContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Hobby> Hobbies { get; set; }
    public virtual DbSet<Skill> Skills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=mysqlservercdn01.mysql.database.azure.com;database=freelancecdn;user id=freelancecdnadmin;password=Test1234;persist security info=True;connection timeout=60", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Hobby>(entity =>
        {
            entity.HasKey(e => e.HobbyId).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}