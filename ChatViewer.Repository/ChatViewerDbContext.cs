using ChatViewer.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatViewer.Repository;

public class ChatViewerDbContext: DbContext
{
    public virtual DbSet<ChatEventType> ChatEventType { get; set; }
    public virtual DbSet<ChatEvent> ChatEvent { get; set; }
    public virtual DbSet<Chatter> Chatter { get; set; }

    public ChatViewerDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatEvent>()
            .HasOne(ce => ce.ChatEventType)
            .WithMany(cet => cet.ChatEvents)
            .HasForeignKey(ce => ce.ChatEventName);

        modelBuilder.Entity<ChatEvent>()
            .HasOne(ce => ce.Chatter)
            .WithMany(c => c.ChatEvents)
            .HasForeignKey(ce => ce.ChatterId);
        
        modelBuilder.Entity<ChatEvent>()
            .HasOne(ce => ce.Chatter2)
            .WithMany(c => c.ChatEvents2)
            .HasForeignKey(ce => ce.Chatter2Id)
            .IsRequired(false);
        
        modelBuilder.Entity<ChatEvent>()
            .Property(x => x.Text)
            .IsRequired(false);
        
        modelBuilder.Entity<Chatter>()
            .Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        modelBuilder.Entity<ChatEventType>()
            .Property(x => x.ChatEventName)
            .HasMaxLength(255)
            .IsRequired();
    }

}