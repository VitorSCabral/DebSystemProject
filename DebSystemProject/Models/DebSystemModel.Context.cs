﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DebSystemProject.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DebSystemDatabaseEntities : DbContext
    {
        public DebSystemDatabaseEntities()
            : base("name=DebSystemDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Debt> Debts { get; set; }
    }
}
