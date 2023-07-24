using AlumniProject.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AlumniProject.Data;

public class AlumniDbContext : DbContext
{
    public AlumniDbContext(DbContextOptions<AlumniDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Specify the desired delete behavior
        modelBuilder
             .Entity<EventParticipant>()
             .HasOne(p => p.Alumni)
             .WithMany(a => a.EventParticipants)
             .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<News>()
            .HasOne(n => n.Alumni)
            .WithMany()
            .HasForeignKey(n => n.AlumniId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Post>()
            .HasOne(p => p.School)
            .WithMany(s => s.Post)
            .HasForeignKey(p => p.SchoolId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<NewsTagNew>()
            .HasOne(ntn => ntn.Tags)
            .WithMany(t => t.NewsTagNews)
            .HasForeignKey(ntn => ntn.TagsId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<AccessRequest>()
            .HasOne(ar => ar.AlumniClass)
            .WithMany(ac => ac.AccessRequests)
            .HasForeignKey(ar => ar.AlumniClassId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AccessRequest>()
            .HasOne(ar => ar.School)
            .WithMany(s => s.AccessRequests)
            .HasForeignKey(ar => ar.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<AlumniToClass>()
         .HasOne(atc => atc.Alumni)
         .WithMany(a => a.AlumniToClasses)
         .HasForeignKey(atc => atc.AlumniId)
         .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AlumniToClass>()
            .HasOne(atc => atc.Class)
            .WithMany(c => c.AlumniToClasses)
            .HasForeignKey(atc => atc.ClassId)
            .OnDelete(DeleteBehavior.Restrict);
        // Other configurations...z

        base.OnModelCreating(modelBuilder);
    }
    public virtual DbSet<AccessRequest> AccessRequest { get; set; }
    public virtual DbSet<Alumni> Alumni { get; set; }
    public virtual DbSet<AlumniClass> AlumniClasse { get; set; }
    public virtual DbSet<Major> Major { get; set; }
    public virtual DbSet<Education> Education { get; set; }
    public virtual DbSet<Events> Event { get; set; }
    public virtual DbSet<Fund> Fund { get; set; }
    public virtual DbSet<EventParticipant> EventParticipant { get; set; }
    public virtual DbSet<Grade> Grade { get; set; }
    public virtual DbSet<News> New { get; set; }
    public virtual DbSet<TagsNew> TagsNew { get; set; }
    public virtual DbSet<Post> Post { get; set; }
    public virtual DbSet<School> School { get; set; }
    public virtual DbSet<AlumniToClass> AlumniToClass { get; set; }
    public virtual DbSet<NewsTagNew> NewsTagNew { get; set; }
    public virtual DbSet<Role> Role { get; set; }
    public virtual DbSet<CardInSchool> CardInSchool { get; set; }
    public virtual DbSet<Attributes> Attributes { get; set; }



}
