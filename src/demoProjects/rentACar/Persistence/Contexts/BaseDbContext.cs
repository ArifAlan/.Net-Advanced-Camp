using Core.Security.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }




        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //    base.OnConfiguring(
            //        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SomeConnectionString")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(a =>
            {
                a.ToTable("Brands").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.Name).HasColumnName("Name");
                a.HasMany(p => p.Models);//birçok modeli var anlamına geliyor
            });

            modelBuilder.Entity<Model>(a =>
            {
                a.ToTable("Models").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.BrandId).HasColumnName("BrandId");
                a.Property(p => p.Name).HasColumnName("Name");
                a.Property(p => p.DailyPrice).HasColumnName("DailyPrice");
                a.Property(p => p.ImageUrl).HasColumnName("ImageUrl");

                a.HasOne(p => p.Brand);
            });
            modelBuilder.Entity<User>(x => {
                x.ToTable("Users").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("Id");
                x.Property(p => p.FirstName).HasColumnName("FirstName");
                x.Property(p => p.LastName).HasColumnName("LastName");
                x.Property(p => p.Email).HasColumnName("Email");
                x.Property(p => p.PasswordSalt).HasColumnName("PasswordSalt");
                x.Property(p => p.PasswordHash).HasColumnName("PasswordHash");
                x.Property(p => p.Status).HasColumnName("Status");
                x.Property(p => p.AuthenticatorType).HasColumnName("AuthenticatorType");

            });

            modelBuilder.Entity<OperationClaim>(x => {
                x.ToTable("OperationClaims").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("Id");
                x.Property(p => p.Name).HasColumnName("Name");

            });

            modelBuilder.Entity<UserOperationClaim>(x => {
                x.ToTable("UserOperationClaims").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("Id");
                x.Property(p => p.UserId).HasColumnName("UserId");
                x.Property(p => p.OperationClaimId).HasColumnName("OperationClaimId");

            });

            modelBuilder.Entity<RefreshToken>(x => {
                x.ToTable("RefreshTokens").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("Id");
                x.Property(p => p.UserId).HasColumnName("UserId");
                x.Property(p => p.Token).HasColumnName("Token");
                x.Property(p => p.Expires).HasColumnName("Expires");
                x.Property(p => p.Created).HasColumnName("Created");
                x.Property(p => p.CreatedByIp).HasColumnName("CreatedByIp");
                x.Property(p => p.Revoked).HasColumnName("Revoked");
                x.Property(p => p.RevokedByIp).HasColumnName("RevokedByIp");
                x.Property(p => p.ReplacedByToken).HasColumnName("ReplacedByToken");
                x.Property(p => p.ReasonRevoked).HasColumnName("ReasonRevoked");
            });




            Brand[] brandEntitySeeds = { new(1, "BMW"), new(2, "MERCEDES") };
            modelBuilder.Entity<Brand>().HasData(brandEntitySeeds);

            Model[] modelEntitySeeds = { new(1,1, "Series 4",1500,""), new(2, 1 ,"Series 3", 1200, "") ,new(3, 2,"A180", 1000, "") };
            modelBuilder.Entity<Model>().HasData(modelEntitySeeds);
           
        }
    }
}
