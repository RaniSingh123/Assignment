using HospitalOPD.Api.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;


namespace HospitalOPD.Api.Data
{
    public class HospitalDBContext : DbContext
    {
        public HospitalDBContext(DbContextOptions<HospitalDBContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasIndex(x => x.UHID)
                .IsUnique();

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.AppointmentTime })
                .IsUnique();
    
            modelBuilder.Entity<Users>()
                .HasKey(u => u.UserID);  
        

            base.OnModelCreating(modelBuilder);
        }
    }
}
