using Event_Management.Domain.Entity;
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
        public virtual DbSet<Logo> Logos { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
       
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<PaymentSignature> PaymentSignatures { get; set; } = null!;
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleEvent> RoleEvents { get; set; } = null!;
        public virtual DbSet<SponsorEvent> SponsorEvents { get; set; } = null!;
        public virtual DbSet<SponsorMethod> SponsorMethods { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserValidation> UserValidations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }
        private static string GetConnectionString()
        {
            IWebHostEnvironment? environment = new HttpContextAccessor().HttpContext?.RequestServices
                                    .GetRequiredService<IWebHostEnvironment>();
            IConfiguration config = new ConfigurationBuilder()

            .AddJsonFile("appsettings.json", true, true)

            .Build();
            if (environment?.IsProduction() ?? true)
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
                entity.ToTable("Event");

                entity.Property(e => e.EventId)
                    .ValueGeneratedNever()
                    .HasColumnName("EventID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EventName).HasMaxLength(250);

                entity.Property(e => e.Image)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Location).HasMaxLength(500);

                entity.Property(e => e.LocationAddress).HasMaxLength(1000);

                entity.Property(e => e.LocationCoord)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LocationId)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.LocationUrl)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Event__LocationA__403A8C7D");
            });

            modelBuilder.Entity<EventMailSystem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EventMailSystem");

                entity.Property(e => e.Body).IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(250);

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
                    .HasConstraintName("FK__EventMail__Event__5441852A");
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
                    .HasConstraintName("FK__EventPaym__Event__6B24EA82");

                entity.HasOne(d => d.Payment)
                    .WithMany()
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__EventPaym__Payme__6A30C649");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.EventId })
                    .HasName("PK__Feedback__001C802B117D8D3E");

                entity.ToTable("Feedback");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.CreatedAt).HasColumnType("date");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__EventI__5070F446");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__UserID__4F7CD00D");
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
                        l => l.HasOne<Event>().WithMany().HasForeignKey("EventId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventLogo__Event__60A75C0F"),
                        r => r.HasOne<Logo>().WithMany().HasForeignKey("LogoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventLogo__LogoI__619B8048"),
                        j =>
                        {
                            j.HasKey("LogoId", "EventId").HasName("PK__EventLog__D1B4590A36BDC71B");

                            j.ToTable("EventLogo");

                            j.IndexerProperty<Guid>("EventId").HasColumnName("EventID");
                        });
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__UserI__5BE2A6F2");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.EventId })
                    .HasName("PK__Particip__001C802B306A0BCF");

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
                    .HasConstraintName("FK__Participa__Event__4BAC3F29");

                entity.HasOne(d => d.RoleEvent)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.RoleEventId)
                    .HasConstraintName("FK__Participa__RoleE__4AB81AF0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__UserI__4CA06362");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId)
                    .ValueGeneratedNever()
                    .HasColumnName("PaymentID");

                

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.PaidAmount).HasColumnType("decimal(19, 2)");

                entity.Property(e => e.PaymentContent)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentCurrency)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDestinationId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentLanguage)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentLastMessage).IsUnicode(false);

                entity.Property(e => e.PaymentRefId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentPurpose)
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.RequiredAmount).HasColumnType("decimal(19, 2)");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Payment__Created__68487DD7");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentMethodName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaymentSignature>(entity =>
            {
                entity.ToTable("PaymentSignature");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.SignAlgo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SignDate).HasColumnType("datetime");

                entity.Property(e => e.SignOwn)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SignValue)
                    .HasMaxLength(100)
                    .IsUnicode(false);

               
            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.ToTable("PaymentTransaction");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TranAmount).HasColumnType("decimal(19, 2)");

                entity.Property(e => e.TranDate).HasColumnType("datetime");

                entity.Property(e => e.TranMessage).IsUnicode(false);

                

                entity.Property(e => e.TranStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.PaymentTransactions)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__PaymentTr__Payme__70DDC3D8");
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

                entity.Property(e => e.Token).HasMaxLength(300);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__RefreshTo__UserI__59063A47");
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
                    .HasConstraintName("FK__SponsorEv__Event__6383C8BA");

                entity.HasOne(d => d.SponsorMethod)
                    .WithMany()
                    .HasForeignKey(d => d.SponsorMethodId)
                    .HasConstraintName("FK__SponsorEv__Spons__6477ECF3");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__SponsorEv__UserI__656C112C");
            });

            modelBuilder.Entity<SponsorMethod>(entity =>
            {
                entity.ToTable("SponsorMethod");

                entity.Property(e => e.SponsorMethodName).HasMaxLength(255);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.TagName).HasMaxLength(255);

                entity.HasMany(d => d.Events)
                    .WithMany(p => p.Tags)
                    .UsingEntity<Dictionary<string, object>>(
                        "EventTag",
                        l => l.HasOne<Event>().WithMany().HasForeignKey("EventId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventTag__EventI__44FF419A"),
                        r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventTag__TagID__45F365D3"),
                        j =>
                        {
                            j.HasKey("TagId", "EventId").HasName("PK__EventTag__72E8B6CBB7CA4752");

                            j.ToTable("EventTag");

                            j.IndexerProperty<int>("TagId").HasColumnName("TagID");

                            j.IndexerProperty<Guid>("EventId").HasColumnName("EventID");
                        });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("date");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__RoleID__398D8EEE");
            });

            modelBuilder.Entity<UserValidation>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserVali__1788CCAC2DF61615");

                entity.ToTable("UserValidation");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.ExpiredAt).HasColumnType("datetime");

                entity.Property(e => e.Otp)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("OTP");

                entity.Property(e => e.VerifyToken).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
