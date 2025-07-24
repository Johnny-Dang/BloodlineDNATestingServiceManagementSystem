using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

public partial class DnatestingServiceContext : DbContext
{
    public DnatestingServiceContext()
    {
    }

    public DnatestingServiceContext(DbContextOptions<DnatestingServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Consultant> Consultants { get; set; }

    public virtual DbSet<DetailResult> DetailResults { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Participant> Participants { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sample> Samples { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<SurchargePrice> SurchargePrices { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=1234567890;database=DNATestingService;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BlogPost");

            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedDate).HasColumnName("createdDate");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.PostId)
                .ValueGeneratedOnAdd()
                .HasColumnName("postID");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
            entity.Property(e => e.UpdatedDate).HasColumnName("updatedDate");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BlogPost__userID__5070F446");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__C6D03BED735C6CA1");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("bookingID");
            entity.Property(e => e.AppointmentDate).HasColumnName("appointmentDate");
            entity.Property(e => e.BookingDate).HasColumnName("bookingDate");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.NumberSample).HasColumnName("numberSample");
            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasDefaultValue("Chờ xác nhận")
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("numeric(38, 2)")
                .HasColumnName("totalPrice");
            entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
            entity.Property(e => e.UpdateDate).HasColumnName("updateDate");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Service).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__service__35BCFE0A");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.BookingUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK__Booking__updateB__36B12243");

            entity.HasOne(d => d.User).WithMany(p => p.BookingUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__userID__34C8D9D1");
        });

        modelBuilder.Entity<Consultant>(entity =>
        {
            entity.HasKey(e => e.ConsultantId).HasName("PK__Consulta__8E3CA2DFA05216C0");

            entity.ToTable("Consultant");

            entity.Property(e => e.ConsultantId).HasColumnName("consultantID");
            entity.Property(e => e.ConfirmBy).HasColumnName("confirmBy");
            entity.Property(e => e.ConsultantDate).HasColumnName("consultantDate");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedDate).HasColumnName("createdDate");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Consultants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Consultan__userI__534D60F1");
        });

        modelBuilder.Entity<DetailResult>(entity =>
        {
            entity.HasKey(e => e.DetailResultId).HasName("PK__DetailRe__7B658D46718D0C83");

            entity.ToTable("DetailResult");

            entity.Property(e => e.DetailResultId).HasColumnName("detailResultID");
            entity.Property(e => e.LocusName)
                .HasMaxLength(50)
                .HasColumnName("locusName");
            entity.Property(e => e.P1Allele1)
                .HasMaxLength(50)
                .HasColumnName("p1Allele1");
            entity.Property(e => e.P1Allele2)
                .HasMaxLength(50)
                .HasColumnName("p1Allele2");
            entity.Property(e => e.P2Allele1)
                .HasMaxLength(50)
                .HasColumnName("p2Allele1");
            entity.Property(e => e.P2Allele2)
                .HasMaxLength(50)
                .HasColumnName("p2Allele2");
            entity.Property(e => e.PaternityIndex)
                .HasColumnType("decimal(18, 9)")
                .HasColumnName("paternityIndex");
            entity.Property(e => e.TestResultId).HasColumnName("testResultID");

            entity.HasOne(d => d.TestResult).WithMany(p => p.DetailResults)
                .HasForeignKey(d => d.TestResultId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetailRes__testR__47DBAE45");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC47428A57A");

            entity.ToTable("Feedback");

            entity.HasIndex(e => e.BookingId, "UQ__Feedback__C6D03BEC18723438").IsUnique();

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.Answers).HasColumnName("answers");
            entity.Property(e => e.BookingId).HasColumnName("bookingID");
            entity.Property(e => e.Comments).HasColumnName("comments");
            entity.Property(e => e.CreateDate).HasColumnName("createDate");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReturnDate).HasColumnName("returnDate");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Hoạt động")
                .HasColumnName("status");

            entity.HasOne(d => d.Booking).WithOne(p => p.Feedback)
                .HasForeignKey<Feedback>(d => d.BookingId)
                .HasConstraintName("FK__Feedback__bookin__412EB0B6");
        });

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(e => e.ParticipantId).HasName("PK__Particip__4EE792308B9F7FFD");

            entity.ToTable("Participant");

            entity.Property(e => e.ParticipantId).HasColumnName("participantID");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.CollectionMethod)
                .HasMaxLength(255)
                .HasColumnName("collectionMethod");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("identityNumber");
            entity.Property(e => e.QuestionalbleRelationship)
                .HasMaxLength(100)
                .HasColumnName("questionalbleRelationship");
            entity.Property(e => e.RelationshipToCustomer)
                .HasMaxLength(100)
                .HasColumnName("relationshipToCustomer");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__A0D9EFA693888D36");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.BookingId, "UQ__Payment__C6D03BEC52329F2D").IsUnique();

            entity.Property(e => e.PaymentId).HasColumnName("paymentID");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("bookingID");
            entity.Property(e => e.PaymentDate).HasColumnName("paymentDate");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Chờ xác nhận")
                .HasColumnName("status");

            entity.HasOne(d => d.Booking).WithOne(p => p.Payment)
                .HasForeignKey<Payment>(d => d.BookingId)
                .HasConstraintName("FK__Payment__booking__3B75D760");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98460A65E2B3C3");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<Sample>(entity =>
        {
            entity.HasKey(e => e.SampleId).HasName("PK__Sample__3FD4F24B4EB5FABC");

            entity.ToTable("Sample");

            entity.Property(e => e.SampleId).HasColumnName("sampleID");
            entity.Property(e => e.BookingId).HasColumnName("bookingID");
            entity.Property(e => e.ParticipantId).HasColumnName("participantID");
            entity.Property(e => e.ReceivedDate).HasColumnName("receivedDate");
            entity.Property(e => e.SampleType)
                .HasMaxLength(100)
                .HasColumnName("sampleType");
            entity.Property(e => e.TypeOfCollection)
                .HasMaxLength(100)
                .HasColumnName("typeOfCollection");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Booking).WithMany(p => p.Samples)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sample__bookingI__4D94879B");

            entity.HasOne(d => d.Participant).WithMany(p => p.Samples)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sample__particip__4E88ABD4");

            entity.HasOne(d => d.User).WithMany(p => p.Samples)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Sample__userID__4CA06362");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__4550733F3DC3F25F");

            entity.ToTable("Service", tb => tb.HasTrigger("trg_AfterInsert_Service"));

            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.ExtraSampleFee)
                .HasColumnType("money")
                .HasColumnName("extraSampleFee");
            entity.Property(e => e.PackageType)
                .HasMaxLength(100)
                .HasColumnName("packageType");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(100)
                .HasColumnName("serviceName");
            entity.Property(e => e.ServiceType)
                .HasMaxLength(100)
                .HasColumnName("serviceType");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasDefaultValue("Hoạt động")
                .HasColumnName("status");
        });

        modelBuilder.Entity<SurchargePrice>(entity =>
        {
            entity.HasKey(e => e.SurchargeId).HasName("PK__Surcharg__F9327BEAB29C560D");

            entity.ToTable("SurchargePrice", tb => tb.HasTrigger("trg_AfterInsert_Surcharge"));

            entity.Property(e => e.SurchargeId).HasColumnName("surchargeID");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.SampleType)
                .HasMaxLength(100)
                .HasColumnName("sampleType");
            entity.Property(e => e.Surcharge)
                .HasColumnType("money")
                .HasColumnName("surcharge");

            entity.HasMany(d => d.Services).WithMany(p => p.Surcharges)
                .UsingEntity<Dictionary<string, object>>(
                    "IncludeSurcharge",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__IncludeSu__servi__300424B4"),
                    l => l.HasOne<SurchargePrice>().WithMany()
                        .HasForeignKey("SurchargeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__IncludeSu__surch__30F848ED"),
                    j =>
                    {
                        j.HasKey("SurchargeId", "ServiceId").HasName("PK__IncludeS__1D677CD9E5ADDEC0");
                        j.ToTable("IncludeSurcharge");
                        j.IndexerProperty<int>("SurchargeId").HasColumnName("surchargeID");
                        j.IndexerProperty<int>("ServiceId").HasColumnName("serviceID");
                    });
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.TestResultId).HasName("PK__TestResu__DD1FEAAD9D3F189A");

            entity.ToTable("TestResult");

            entity.HasIndex(e => e.BookingId, "UQ__TestResu__C6D03BECEBF34560").IsUnique();

            entity.Property(e => e.TestResultId).HasColumnName("testResultID");
            entity.Property(e => e.BookingId).HasColumnName("bookingID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate).HasColumnName("createdDate");
            entity.Property(e => e.ResultConclution)
                .HasColumnType("text")
                .HasColumnName("resultConclution");
            entity.Property(e => e.ResultDate).HasColumnName("resultDate");
            entity.Property(e => e.ResultFile)
                .HasMaxLength(255)
                .HasColumnName("resultFile");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate).HasColumnName("updatedDate");

            entity.HasOne(d => d.Booking).WithOne(p => p.TestResult)
                .HasForeignKey<TestResult>(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestResul__booki__44FF419A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__CB9A1CDF7173C51B");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E6164D8437F0F").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Hoạt động")
                .HasColumnName("status");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__roleID__286302EC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
