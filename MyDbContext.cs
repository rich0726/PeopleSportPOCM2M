using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleSportsSandbox
{
    public class MyDbContext : DbContext
    {

        public MyDbContext()
        {

        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
          : base(options)
        {
        }

        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Sports> Sports { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PeopleSportsM2M;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }


        public class EnumCollectionJsonValueConverter<T> : ValueConverter<ICollection<T>, string> where T : Enum
        {
            public EnumCollectionJsonValueConverter() : base(
              v => JsonConvert
                .SerializeObject(v.Select(e => e.ToString()).ToList()),
              v => JsonConvert
                .DeserializeObject<ICollection<string>>(v)
                .Select(e => (T)Enum.Parse(typeof(T), e)).ToList())
            {
            }
        }

        public class CollectionValueComparer<T> : ValueComparer<ICollection<T>>
        {
            public CollectionValueComparer() : base((c1, c2) => c1.SequenceEqual(c2),
              c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), c => (ICollection<T>)c.ToHashSet())
            {
            }
        }

        //public abstract class ConvertSportsToString<T> : ValueConverter<ICollection<Sports>, List<Sport>> where T: ICollection<Sports>
        //{
        //    public ConvertSportsToString() : base(
        //         v => JsonConvert.SerializeObject(v.Select(e => e.Sport.ToString()).ToHashSet()),
        //         v => JsonConvert.DeserializeObject<List<Sport>>(v)) 
            
        //    { 
            
        //    }
        //}
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<Sports>()
            .Property(d => d.Sport)
            .HasConversion(new EnumToStringConverter<Sport>());

            modelBuilder
            .Entity<Sports>().HasData(
                Enum.GetValues(typeof(Sport))
                .Cast<Sport>()
                .Select(e => new Sports()
                 {
                     Sport = e
                 })
            );

            //modelBuilder
            //    .Entity<Person>()
            //    .Property(d => d.Sports)
            //    .HasConversion(new ConvertSportsToString<ICollection<Sports>>());
        }
   }
}
