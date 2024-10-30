using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Messaging
{
    public class MassTransitOutboxContext : DbContext
    {
       
        public MassTransitOutboxContext(DbContextOptions<MassTransitOutboxContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("MassTransit");

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
