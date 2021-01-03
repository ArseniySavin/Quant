using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Quant.DB.CLI.Models
{
    public partial class QuantContext : DbContext
    {
        public QuantContext()
        {
        }

        public QuantContext(DbContextOptions<QuantContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blobs> Blobs { get; set; }
        public virtual DbSet<BlobsMap> BlobsMap { get; set; }
        public virtual DbSet<Payload> Payload { get; set; }
        public virtual DbSet<TransactionAwait> TransactionAwait { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<TransactionsHistory> TransactionsHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=hqw6001;Database=Quant;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blobs>(entity =>
            {
                entity.Property(e => e.BlobsId).HasColumnName("Blobs_ID");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("File_Name")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<BlobsMap>(entity =>
            {
                entity.ToTable("Blobs_map");

                entity.Property(e => e.BlobsMapId).HasColumnName("Blobs_map_ID");

                entity.Property(e => e.BlobsIdRef).HasColumnName("Blobs_ID_REF");

                entity.Property(e => e.CorrelationIdRef).HasColumnName("Correlation_ID_REF");

                entity.Property(e => e.MapType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlobsIdRefNavigation)
                    .WithMany(p => p.BlobsMap)
                    .HasForeignKey(d => d.BlobsIdRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Blobs_map_Blobs");
            });

            modelBuilder.Entity<Payload>(entity =>
            {
                entity.Property(e => e.PayloadId).HasColumnName("Payload_ID");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TransactionAwait>(entity =>
            {
                entity.Property(e => e.TransactionAwaitId).HasColumnName("TransactionAwait_ID");

                entity.Property(e => e.Chanel)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CorrelationReference)
                    .IsRequired()
                    .HasColumnName("Correlation_Reference")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CorrelationSystem)
                    .IsRequired()
                    .HasColumnName("Correlation_System")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NextCall)
                    .HasColumnName("Next_Call")
                    .HasColumnType("datetime");

                entity.Property(e => e.PayloadIdRef).HasColumnName("Payload_ID_REF");

                entity.Property(e => e.RetryCount).HasColumnName("Retry_Count");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionIdRef).HasColumnName("Transaction_ID_REF");

                entity.HasOne(d => d.PayloadIdRefNavigation)
                    .WithMany(p => p.TransactionAwait)
                    .HasForeignKey(d => d.PayloadIdRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionAwait_Payload");

                entity.HasOne(d => d.TransactionIdRefNavigation)
                    .WithMany(p => p.TransactionAwait)
                    .HasForeignKey(d => d.TransactionIdRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionAwait_Transactions");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.Property(e => e.InstanceId).HasColumnName("Instance_ID");

                entity.Property(e => e.ParentIdRef).HasColumnName("Parent_ID_REF");

                entity.Property(e => e.SystemCode)
                    .HasColumnName("System_Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SystemReference)
                    .HasColumnName("System_Reference")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionCode)
                    .HasColumnName("Transaction_Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransactionsHistory>(entity =>
            {
                entity.Property(e => e.TransactionsHistoryId).HasColumnName("TransactionsHistory_ID");

                entity.Property(e => e.App)
                    .IsRequired()
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.Property(e => e.Chanel)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClasterId)
                    .IsRequired()
                    .HasColumnName("Claster_ID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PayloadIdRef).HasColumnName("Payload_ID_REF");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TransactionIdRef).HasColumnName("Transaction_ID_REF");

                entity.HasOne(d => d.PayloadIdRefNavigation)
                    .WithMany(p => p.TransactionsHistory)
                    .HasForeignKey(d => d.PayloadIdRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionHistory_Payload");

                entity.HasOne(d => d.TransactionIdRefNavigation)
                    .WithMany(p => p.TransactionsHistory)
                    .HasForeignKey(d => d.TransactionIdRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionHistory_Transactions");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
