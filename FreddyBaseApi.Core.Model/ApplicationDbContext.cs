using FreddyBaseApi.Core.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreddyBaseApi.Core.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<DummyEntity> DummyEntities { get; set; }
        public DbSet<DummyNestedEntity> DummyNestedEntities { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer();
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
