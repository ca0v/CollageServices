using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ImageRipper.Test;

public partial class PhotoContext : DbContext
{
    public PhotoContext()
    {
    }

    public PhotoContext(DbContextOptions<PhotoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Collage> Collages { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<Recording> Recordings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseSqlite(configuration.GetConnectionString("PhotoDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collage>(entity =>
        {
            entity.ToTable("collages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.ToTable("photos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Created).HasColumnName("created");
            entity.Property(e => e.Filename).HasColumnName("filename");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<Recording>(entity =>
        {
            entity.ToTable("recordings");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
