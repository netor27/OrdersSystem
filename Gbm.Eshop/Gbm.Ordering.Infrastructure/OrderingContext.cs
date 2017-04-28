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

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<PaymentMethod> Payments { get; set; }

        public DbSet<Buyer> Buyers { get; set; }

        public DbSet<CardType> CardTypes { get; set; }

        public DbSet<OrderStatus> OrderStatus { get; set; }

        public OrderingContext(DbContextOptions options, IMediator mediator):base(options)
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
            paymentConfiguration.Property<int>("BuyerId");
            paymentConfiguration.Property<string>("CardHolderName").HasMaxLength(200).IsRequired();
            paymentConfiguration.Property<string>("Alias").HasMaxLength(200).IsRequired();
            paymentConfiguration.Property<string>("CardNumber").HasMaxLength(25).IsRequired();
            paymentConfiguration.Property<DateTime>("Expiration").IsRequired();
        }

        private void ConfigureOrder(EntityTypeBuilder<Order> paymentConfiguration)
        {
            throw new NotImplementedException();
        }

        private void ConfigureOrderItem(EntityTypeBuilder<OrderItem> paymentConfiguration)
        {
            throw new NotImplementedException();
        }

        private void ConfigureCardType(EntityTypeBuilder<CardType> paymentConfiguration)
        {
            throw new NotImplementedException();
        }

        private void ConfigureOrderStatus(EntityTypeBuilder<OrderStatus> paymentConfiguration)
        {
            throw new NotImplementedException();
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

        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
