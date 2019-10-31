using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace YiLive.Data
{
    public partial class YiyiLiveContext : DbContext
    {
        public YiyiLiveContext()
        {
        }

        public YiyiLiveContext(DbContextOptions<YiyiLiveContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DailyFood> DailyFood { get; set; }
        public virtual DbSet<DailyFoodImg> DailyFoodImg { get; set; }
        public virtual DbSet<KeepFitDiary> KeepFitDiary { get; set; }
        public virtual DbSet<KeepFitDiaryImg> KeepFitDiaryImg { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=YiyiLive;User ID=sa;Password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<DailyFood>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EatTime).HasColumnName("eatTime");

                entity.Property(e => e.Foods)
                    .HasColumnName("foods")
                    .HasMaxLength(200);

                entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");

                entity.Property(e => e.IsMain).HasColumnName("isMain");

                entity.Property(e => e.Kfid).HasColumnName("kfid");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(50);

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.Property(e => e.Updated)
                    .HasColumnName("updated")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<DailyFoodImg>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Dfid).HasColumnName("dfid");

                entity.Property(e => e.DirUrl)
                    .IsRequired()
                    .HasColumnName("dirUrl")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.Updated)
                    .HasColumnName("updated")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<KeepFitDiary>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("datetime");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");

                entity.Property(e => e.MeasureTime).HasColumnName("measureTime");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(200);

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.Property(e => e.Updated)
                    .HasColumnName("updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<KeepFitDiaryImg>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DirUrl)
                    .IsRequired()
                    .HasColumnName("dirUrl")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");

                entity.Property(e => e.Kfid).HasColumnName("kfid");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.Updated)
                    .HasColumnName("updated")
                    .HasColumnType("datetime");
            });
        }
    }
}
