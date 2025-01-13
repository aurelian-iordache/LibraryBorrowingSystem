using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.StateMachines;

namespace OrderService.Database
{
    public class OrderStateDbContext : SagaDbContext
    {
        public OrderStateDbContext(DbContextOptions<OrderStateDbContext> options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new OrderSagaStateMap();
            }
        }
    }

    public class OrderSagaStateMap : SagaClassMap<OrderSagaState>
    {
        protected override void Configure(EntityTypeBuilder<OrderSagaState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
            entity.Property(x => x.CustomerId).HasMaxLength(128);
            entity.Property(x => x.OrderCreatedDate);
            entity.Property(x => x.OrderProcessedDate);
            entity.Property(x => x.Status).HasMaxLength(64);
        }
    }
}