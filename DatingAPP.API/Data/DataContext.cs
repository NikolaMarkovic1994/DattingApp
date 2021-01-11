using DatingApp.API.Model;
using DatingAPP.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}// konstruktor

        public DbSet<Auto> Autos { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Like> Likes { get; set; }
         public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder){

            builder.Entity<Like>()
                .HasKey(k => new {k.LikerId,k.LekeeId});

            builder.Entity<Like>()
                .HasOne(u =>u.Likee)
                .WithMany(u =>u.Likers)
                .HasForeignKey(u =>u.LekeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasOne(u =>u.Liker)
                .WithMany(u =>u.Likees)
                .HasForeignKey(u =>u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);    

            builder.Entity<Message>()
                .HasOne(u =>u.Sender)
                .WithMany(u =>u.MessagesSend)
                .OnDelete(DeleteBehavior.Restrict); 
            builder.Entity<Message>()
                .HasOne(u =>u.Recepient)
                .WithMany(u =>u.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict); 
        }

    }
}
