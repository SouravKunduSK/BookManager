﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookManager.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BookManagerEntities : DbContext
    {
        public BookManagerEntities()
            : base("name=BookManagerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Author_Book> Author_Book { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookStat> BookStats { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CompletedBook> CompletedBooks { get; set; }
        public virtual DbSet<Lending> Lendings { get; set; }
        public virtual DbSet<ReadingStat> ReadingStats { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }
    }
}