



using Event_Management.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Event_Management.Infrastructure.DBContext
{
    public partial class EventManagementContext : DbContext
    {
        public EventManagementContext()
        {
        }

        public EventManagementContext(DbContextOptions<EventManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<EventMailSystem> EventMailSystems { get; set; } = null!;
        public virtual DbSet<EventPayment> EventPayments { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<Perk> Perks { get; set; } = null!;
        public virtual DbSet<Policy> Policies { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleEvent> RoleEvents { get; set; } = null!;
        public virtual DbSet<SponsorMethod> SponsorMethods { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private static string GetConnectionString()
        {
            IWebHostEnvironment environment = new HttpContextAccessor().HttpContext!.RequestServices
                                    .GetRequiredService<IWebHostEnvironment>();
            IConfiguration config = new ConfigurationBuilder()

            .SetBasePath(Directory.GetCurrentDirectory())

            .AddJsonFile("appsettings.json", true, true)

            .Build();
            if (environment.IsProduction())
            {
                return config["ConnectionStrings:SQL"]!;
            }
            else
            {
                return config["LocalDB:SQL"]!;
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId).ValueGeneratedNever();

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Event__CreatedBy__3E52440B");
            });

            modelBuilder.Entity<EventMailSystem>(entity =>
            {
                entity.HasOne(d => d.Event)
                    .WithMany()
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventMail__Event__60A75C0F");
            });

            modelBuilder.Entity<EventPayment>(entity =>
            {
                entity.HasOne(d => d.Event)
                    .WithMany()
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventPaym__Event__6383C8BA");

                entity.HasOne(d => d.Payment)
                    .WithMany()
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__EventPaym__Payme__628FA481");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.EventId })
                    .HasName("PK__Feedback__001C802BE41D01AD");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__EventI__4E88ABD4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__UserID__4D94879B");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.Property(e => e.PackageId).ValueGeneratedNever();

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Packages)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Package__EventID__5BE2A6F2");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.EventId })
                    .HasName("PK__Particip__001C802BBBFEAE01");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__Event__49C3F6B7");

                entity.HasOne(d => d.RoleEvent)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.RoleEventId)
                    .HasConstraintName("FK__Participa__RoleE__48CFD27E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__UserI__4AB81AF0");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId).ValueGeneratedNever();

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK__Payment__Payment__534D60F1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Payment__UserID__5441852A");
            });

            modelBuilder.Entity<Perk>(entity =>
            {
                entity.HasOne(d => d.Package)
                    .WithMany()
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK__Perks__PackageID__5DCAEF64");

                entity.HasOne(d => d.Policy)
                    .WithMany()
                    .HasForeignKey(d => d.PolicyId)
                    .HasConstraintName("FK__Perks__PolicyID__5EBF139D");
            });

            modelBuilder.Entity<SponsorMethod>(entity =>
            {
                entity.HasOne(d => d.Package)
                    .WithMany(p => p.SponsorMethods)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK__SponsorMe__Packa__66603565");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasMany(d => d.Events)
                    .WithMany(p => p.Tags)
                    .UsingEntity<Dictionary<string, object>>(
                        "EventTag",
                        l => l.HasOne<Event>().WithMany().HasForeignKey("EventId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventTag__EventI__4316F928"),
                        r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventTag__TagID__440B1D61"),
                        j =>
                        {
                            j.HasKey("TagId", "EventId").HasName("PK__EventTag__72E8B6CB9BC02B63");

                            j.ToTable("EventTag");

                            j.IndexerProperty<int>("TagId").HasColumnName("TagID");

                            j.IndexerProperty<Guid>("EventId").HasColumnName("EventID");
                        });
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Transacti__Event__59063A47");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__Transacti__Payme__571DF1D5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Transacti__UserI__5812160E");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__RoleID__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
