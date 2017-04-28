using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MediatR;

using Gbm.Ordering.Domain.Common;
using Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate;
using Gbm.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Gbm.Ordering.Infrastructure
{
    public class OrderingContext : DbContext, IUnitOfWork
    {
        const string DEFAULT_SCHEMA = "ordering";

        private readonly IMediator mediator;

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<PaymentMethod> Payments { get; set; }

        public DbSet<Buyer> Buyers { get; set; }

        public DbSet<CardType> CardTypes { get; set; }

        public DbSet<OrderStatus> OrderStatus { get; set; }

        public OrderingContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            this.mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientRequest>(ConfigureRequest);
            modelBuilder.Entity<Address>(ConfigureAddress);
            modelBuilder.Entity<PaymentMethod>(ConfigurePayment);
            modelBuilder.Entity<Order>(ConfigureOrder);
            modelBuilder.Entity<OrderItem>(ConfigureOrderItem);
            modelBuilder.Entity<CardType>(ConfigureCardType);
            modelBuilder.Entity<OrderStatus>(ConfigureOrderStatus);
            modelBuilder.Entity<Buyer>(ConfigureBuyer);
        }

        private void ConfigureRequest(EntityTypeBuilder<ClientRequest> requestConfiguration)
        {
            requestConfiguration.ToTable("requests", DEFAULT_SCHEMA);
            requestConfiguration.HasKey(cr => cr.Id);
            requestConfiguration.Property(cr => cr.Name).IsRequired();
            requestConfiguration.Property(cr => cr.Time).IsRequired();
        }

        private void ConfigureAddress(EntityTypeBuilder<Address> addressConfiguration)
        {
            addressConfiguration.ToTable("address", DEFAULT_SCHEMA);
            addressConfiguration.Property<int>("Id").IsRequired();
            addressConfiguration.HasKey("Id");
        }

        private void ConfigurePayment(EntityTypeBuilder<PaymentMethod> paymentConfiguration)
        {
            paymentConfiguration.ToTable("paymentMethods", DEFAULT_SCHEMA);
            paymentConfiguration.HasKey(p => p.Id);
            paymentConfiguration.Ignore(p => p.DomainEvents);
            paymentConfiguration.Property(p => p.Id).ForSqlServerUseSequenceHiLo("paymentSeq", DEFAULT_SCHEMA);

            // TODO: Refactor this to invoke a lambda instead of the property name
            paymentConfiguration.Property<int>("BuyerId");
            paymentConfiguration.Property<string>("CardHolderName").HasMaxLength(200).IsRequired();
            paymentConfiguration.Property<string>("Alias").HasMaxLength(200).IsRequired();
            paymentConfiguration.Property<string>("CardNumber").HasMaxLength(25).IsRequired();
            paymentConfiguration.Property<DateTime>("Expiration").IsRequired();
            paymentConfiguration.Property(p => p.CardType.Id).HasMaxLength(25).IsRequired();
            paymentConfiguration.HasOne(p => p.CardType).WithMany().HasForeignKey(b => b.Id);

        }

        private void ConfigureOrder(EntityTypeBuilder<Order> orderConfiguration)
        {
            orderConfiguration.ToTable("orders", DEFAULT_SCHEMA);
            orderConfiguration.HasKey(o => o.Id);
            orderConfiguration.Ignore(o => o.DomainEvents);
            orderConfiguration.Property(o => o.Id).ForSqlServerUseSequenceHiLo("orderseq", DEFAULT_SCHEMA);
            orderConfiguration.Property<DateTime>("OrderDate").IsRequired();
            orderConfiguration.Property<int>("BuyerId").IsRequired();
            orderConfiguration.Property<int>("PaymentMethod").IsRequired();
            orderConfiguration.Property(o => o.OrderStatus.Id).IsRequired();

            var navigation = orderConfiguration.Metadata.FindNavigation(nameof(Order.OrderItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            orderConfiguration.HasOne<PaymentMethod>().WithMany().HasForeignKey("PaymentMethodId")
                .IsRequired(false).OnDelete(DeleteBehavior.Restrict);

            orderConfiguration.HasOne<Buyer>().WithMany().HasForeignKey("BuyerId").IsRequired(false);
            orderConfiguration.HasOne(o => o.OrderStatus).WithMany().HasForeignKey(o => o.OrderStatus.Id);
        }

        private void ConfigureOrderItem(EntityTypeBuilder<OrderItem> orderItemConfiguration)
        {
            orderItemConfiguration.ToTable("orderItems", DEFAULT_SCHEMA);
            orderItemConfiguration.HasKey(o => o.Id);
            orderItemConfiguration.Ignore(o => o.DomainEvents);
            orderItemConfiguration.Property(o => o.Id).ForSqlServerUseSequenceHiLo("orderitemseq");
            orderItemConfiguration.Property(o => o.Id).IsRequired();
            orderItemConfiguration.Property<decimal>("Discount").IsRequired();
            orderItemConfiguration.Property(o => o.ProductId).IsRequired();
            orderItemConfiguration.Property<string>("ProductName").IsRequired();
            orderItemConfiguration.Property<decimal>("UnitPrice").IsRequired();
            orderItemConfiguration.Property<int>("Units").IsRequired();
            orderItemConfiguration.Property<string>("PictureUri").IsRequired(false);
        }

        private void ConfigureCardType(EntityTypeBuilder<CardType> cardTypeConfiguration)
        {
            cardTypeConfiguration.ToTable("cardtypes", DEFAULT_SCHEMA);
            cardTypeConfiguration.HasKey(c => c.Id);
            cardTypeConfiguration.Property(c => c.Id).HasDefaultValue(1).ValueGeneratedNever().IsRequired();
            cardTypeConfiguration.Property(c => c.Name).HasMaxLength(25).IsRequired();
        }

        private void ConfigureOrderStatus(EntityTypeBuilder<OrderStatus> orderStatusConfiguration)
        {
            orderStatusConfiguration.ToTable("orderstatus", DEFAULT_SCHEMA);
            orderStatusConfiguration.HasKey(o => o.Id);
            orderStatusConfiguration.Property(o => o.Id).HasDefaultValue(1).ValueGeneratedNever().IsRequired();
            orderStatusConfiguration.Property(o => o.Name).HasMaxLength(25).IsRequired();            
        }

        private void ConfigureBuyer(EntityTypeBuilder<Buyer> buyerConfiguration)
        {
            buyerConfiguration.ToTable("buyer", DEFAULT_SCHEMA);
            buyerConfiguration.HasKey(b => b.Id);
            buyerConfiguration.Ignore(b => b.DomainEvents);
            buyerConfiguration.Property(b => b.Id).ForSqlServerUseSequenceHiLo("buyerseq", DEFAULT_SCHEMA);
            buyerConfiguration.Property(b => b.IdentityGuid).HasMaxLength(200).IsRequired();
            buyerConfiguration.HasIndex("IdentityGuid");
            buyerConfiguration.HasMany(b => b.PaymentMethods).WithOne()
                .HasForeignKey("BuyerId").OnDelete(DeleteBehavior.Cascade);
            var navigation = buyerConfiguration.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await mediator.DispatchDomainEventsAsync(this);
            var result = await base.SaveChangesAsync();
            return true;
        }
    }
}
