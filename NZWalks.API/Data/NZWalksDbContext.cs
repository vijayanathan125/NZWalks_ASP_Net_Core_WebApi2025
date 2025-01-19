using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDBContext:DbContext
    {
        public NZWalksDBContext(DbContextOptions<NZWalksDBContext> dbContextOptions):base(dbContextOptions)
        {
            
        }

        //DbSet
        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed data for Dificulties
            //Easy, Medium, Hard

            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id= Guid.Parse("1cb50ef7-b5f4-4be4-8d31-70736e09f507"),
                    Name="Easy"
                },
                new Difficulty()
                {
                    Id=Guid.Parse("9bbc1836-0ece-4fde-a159-03aa25978718"),
                    Name="Medium"
                },
                new Difficulty()
                {
                    Id=Guid.Parse("46654a23-81ed-44f2-a946-053278bc1ab6"),
                    Name="Hard"
                },
            };

            //seed difficulties to the database 
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            //Seed data for Region

            var regions = new List<Region>() {
                new Region()
                {
                    Id=Guid.Parse("65d904c3-1096-4398-a6ca-a6b5fd1a5cd9"),
                    Code="AKL",
                    Name="Aukland",
                    RegionImageUrl="Somthing"
                },
                new Region()
                {
                    Id=Guid.Parse("35e9269d-1371-42c6-a416-29bef6cd0783"),
                    Code="NTL",
                    Name="North Land",
                    RegionImageUrl="Somthing",
                },
                new Region()
                {
                    Id=Guid.Parse("d431b438-ea28-44b4-92e2-8d102e776453"),
                    Code="WGL",
                    Name="Wellington",
                    RegionImageUrl="Somthing",
                }
            };
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
