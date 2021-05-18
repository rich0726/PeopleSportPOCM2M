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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PeopleSports;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var myAppconverter = new EnumCollectionJsonValueConverter<Sport>();
            var myAppcomparer = new CollectionValueComparer<Sport>();

            modelBuilder
              .Entity<Person>()
              .Property(e => e.Sports)
              .HasConversion(myAppconverter)
              .Metadata.SetValueComparer(myAppcomparer);
        }



        }
    }
