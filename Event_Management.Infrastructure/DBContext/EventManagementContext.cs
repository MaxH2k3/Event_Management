
using Microsoft.EntityFrameworkCore;
using Event_Management.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public virtual DbSet<Logo> Logos { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleEvent> RoleEvents { get; set; } = null!;
        public virtual DbSet<SponsorEvent> SponsorEvents { get; set; } = null!;
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

			.AddJsonFile("appsettings.json", true, true)

			.Build();
			if (environment.IsProduction())
			{
				return config["ConnectionStrings:SQL"]!;
			} else
			{
				return config["LocalDB:SQL"]!;
			}

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.EventId)
                    .ValueGeneratedNever()
                    .HasColumnName("EventID");

                entity.Property(e => e.CreatedAt).HasColumnType("date");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.EventName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("date");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Event__CreatedBy__5070F446");
            });

            modelBuilder.Entity<EventMailSystem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EventMailSystem");

                entity.Property(e => e.Body).IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.MethodKey)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeExecute).HasColumnType("date");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Event)
                    .WithMany()
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventMail__Event__5AEE82B9");
            });

            modelBuilder.Entity<EventPayment>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EventPayment");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.HasOne(d => d.Event)
                    .WithMany()
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventPaym__Event__5DCAEF64");

                entity.HasOne(d => d.Payment)
                    .WithMany()
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__EventPaym__Payme__5CD6CB2B");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.EventId })
                    .HasName("PK__Feedback__001C802B2FE97F5F");

                entity.ToTable("Feedback");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.Content)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("date");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__EventI__60A75C0F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__UserID__5FB337D6");
            });

            modelBuilder.Entity<Logo>(entity =>
            {
                entity.ToTable("Logo");

                entity.Property(e => e.LogoUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SponsorBrand)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasMany(d => d.Events)
                    .WithMany(p => p.Logos)
                    .UsingEntity<Dictionary<string, object>>(
                        "EventLogo",
                        l => l.HasOne<Event>().WithMany().HasForeignKey("EventId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventLogo__Event__6A30C649"),
                        r => r.HasOne<Logo>().WithMany().HasForeignKey("LogoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventLogo__LogoI__6B24EA82"),
                        j =>
                        {
                            j.HasKey("LogoId", "EventId").HasName("PK__EventLog__D1B4590A6FD4115B");

                            j.ToTable("EventLogo");

                            j.IndexerProperty<Guid>("EventId").HasColumnName("EventID");
                        });
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__UserI__656C112C");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.EventId })
                    .HasName("PK__Particip__001C802BF1428152");

                entity.ToTable("Participant");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.CheckedIn).HasColumnType("datetime");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.RoleEventId).HasColumnName("RoleEventID");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__Event__5BE2A6F2");

                entity.HasOne(d => d.RoleEvent)
                    .WithMany()
                    .HasForeignKey(d => d.RoleEventId)
                    .HasConstraintName("FK__Participa__RoleE__5AEE82B9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__UserI__5CD6CB2B");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId)
                    .ValueGeneratedNever()
                    .HasColumnName("PaymentID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentOwner)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK__Payment__Payment__656C112C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Payment__UserID__66603565");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentMethodName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentMethodName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permission");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.Token).HasMaxLength(500);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__RefreshTo__UserI__628FA481");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RoleEvent>(entity =>
            {
                entity.ToTable("RoleEvent");

                entity.Property(e => e.RoleEventId).HasColumnName("RoleEventID");

                entity.Property(e => e.RoleEventName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SponsorEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SponsorEvent");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Event)
                    .WithMany()
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__SponsorEv__Event__6D0D32F4");

                entity.HasOne(d => d.SponsorMethod)
                    .WithMany()
                    .HasForeignKey(d => d.SponsorMethodId)
                    .HasConstraintName("FK__SponsorEv__Spons__6E01572D");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__SponsorEv__UserI__6EF57B66");
            });

            modelBuilder.Entity<SponsorMethod>(entity =>
            {
                entity.ToTable("SponsorMethod");

                entity.Property(e => e.SponsorMethodName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.TagName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasMany(d => d.Events)
                    .WithMany(p => p.Tags)
                    .UsingEntity<Dictionary<string, object>>(
                        "EventTag",
                        l => l.HasOne<Event>().WithMany().HasForeignKey("EventId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventTag__EventI__5535A963"),
                        r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventTag__TagID__5629CD9C"),
                        j =>
                        {
                            j.HasKey("TagId", "EventId").HasName("PK__EventTag__72E8B6CBD8FD832F");

                            j.ToTable("EventTag");

                            j.IndexerProperty<int>("TagId").HasColumnName("TagID");

                            j.IndexerProperty<Guid>("EventId").HasColumnName("EventID");
                        });
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Transacti__Event__6B24EA82");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__Transacti__Payme__693CA210");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Transacti__UserI__6A30C649");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.CreatedAt).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("date");

                entity.HasOne(d => d.Role)
                    .WithMany()
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__RoleID__4BAC3F29");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
