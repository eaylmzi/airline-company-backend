using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace airlinecompany.Data.Models
{
    public partial class AirlineCompanyDBContext : DbContext
    {
        public AirlineCompanyDBContext()
        {
        }

        public AirlineCompanyDBContext(DbContextOptions<AirlineCompanyDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Flight> Flights { get; set; } = null!;
        public virtual DbSet<FlightAttendant> FlightAttendants { get; set; } = null!;
        public virtual DbSet<Passenger> Passengers { get; set; } = null!;
        public virtual DbSet<Plane> Planes { get; set; } = null!;
        public virtual DbSet<Point> Points { get; set; } = null!;
        public virtual DbSet<SessionPassenger> SessionPassengers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLSERVER;Database=AirlineCompanyDB;Trusted_Connection=true;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("company");

                entity.HasIndex(e => e.Name, "UQ__company__72E12F1B5F7B3848")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.TotalMoney).HasColumnName("total_money");
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.ToTable("flight");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.FinalPoint).HasColumnName("final_point");

                entity.Property(e => e.FlightAttendantId).HasColumnName("flight_attendant_id");

                entity.Property(e => e.FlightNumber)
                    .HasMaxLength(10)
                    .HasColumnName("flight_number");

                entity.Property(e => e.PlaneNumber).HasColumnName("plane_number");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.StartPoint).HasColumnName("start_point");
                entity.Property(e => e.PassengerCount).HasColumnName("passenger_count");

            });

            modelBuilder.Entity<FlightAttendant>(entity =>
            {
                entity.ToTable("flight_attendant");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .HasColumnName("surname");
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.ToTable("passenger");

                entity.HasIndex(e => e.UserName, "UQ__passenge__7C9273C413F2919D")
                    .IsUnique();

                entity.HasIndex(e => e.Token, "UQ__passenge__CA90DA7AC24D110A")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Money).HasColumnName("money");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");


                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(512)
                    .HasColumnName("password_hash");

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(512)
                    .HasColumnName("password_salt");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .HasColumnName("surname");

                entity.Property(e => e.Token)
                    .HasMaxLength(510)
                    .HasColumnName("token");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("user_name");
            });

            modelBuilder.Entity<Plane>(entity =>
            {
                entity.ToTable("plane");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Model)
                    .HasMaxLength(50)
                    .HasColumnName("model");

                entity.Property(e => e.SeatNumber).HasColumnName("seat_number");
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.ToTable("point");

                entity.HasIndex(e => e.Name, "UQ__point__72E12F1BA19C1CE6")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<SessionPassenger>(entity =>
            {
                entity.ToTable("session_passenger");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FlightId).HasColumnName("flight_id");

                entity.Property(e => e.PassengerId).HasColumnName("passenger_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
