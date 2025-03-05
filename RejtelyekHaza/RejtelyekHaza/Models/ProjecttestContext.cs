using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RejtelyekHaza.Models;

public partial class ProjecttestContext : DbContext
{
    public ProjecttestContext()
    {
    }

    public ProjecttestContext(DbContextOptions<ProjecttestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;database=projecttest;user=root;password=;sslmode=none;");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.UserName, "username").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.Email).HasMaxLength(64);
            entity.Property(e => e.Hash)
                .HasMaxLength(64)
                .HasColumnName("HASH");
            entity.Property(e => e.Name).HasMaxLength(64);
            entity.Property(e => e.Permission).HasColumnType("int(1)");
            entity.Property(e => e.PhoneNumber).HasMaxLength(16);
            entity.Property(e => e.Salt)
                .HasMaxLength(64)
                .HasColumnName("SALT");
            entity.Property(e => e.UserName).HasMaxLength(64);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
