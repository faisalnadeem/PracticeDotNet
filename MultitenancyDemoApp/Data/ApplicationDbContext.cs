using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultitenancyDemoApp.Models;

namespace MultitenancyDemoApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly AppTenant _tenant;

        public ApplicationDbContext(AppTenant tenant)
        {
            _tenant = tenant;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_tenant.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    : base(options)
        //{
        //}
    }
}
