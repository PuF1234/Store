using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Store.Data.EF
{
    public class StoreDbContext : DbContext
    {
        public DbSet<BicycleDto> Bicycles { get; set; }

        public DbSet<OrderDto> Orders { get; set; }

        public DbSet<OrderItemDto> OrderItems { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildBicycles(modelBuilder);
            BuildOrders(modelBuilder);
            BuildOrderItems(modelBuilder);
        }

        private void BuildOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemDto>(action =>
            {
                action.Property(dto => dto.Price)
                      .HasColumnType("money");

                action.HasOne(dto => dto.Order)
                      .WithMany(dto => dto.Items)
                      .IsRequired();
            });
        }

        private static void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDto>(action =>
            {
                action.Property(dto => dto.CellPhone)
                      .HasMaxLength(20);

                action.Property(dto => dto.DeliveryUniqueCode)
                      .HasMaxLength(40);

                action.Property(dto => dto.DeliveryPrice)
                      .HasColumnType("money");

                action.Property(dto => dto.PaymentServiceName)
                      .HasMaxLength(40);

                action.Property(dto => dto.DeliveryParameters)
                      .HasConversion(
                          value => JsonConvert.SerializeObject(value),
                          value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);

                action.Property(dto => dto.PaymentParameters)
                      .HasConversion(
                          value => JsonConvert.SerializeObject(value),
                          value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);
            });
        }

        private static readonly ValueComparer DictionaryComparer =
            new ValueComparer<Dictionary<string, string>>(
                (dictionary1, dictionary2) => dictionary1.SequenceEqual(dictionary2),
                dictionary => dictionary.Aggregate(
                    0,
                    (a, p) => HashCode.Combine(HashCode.Combine(a, p.Key.GetHashCode()), p.Value.GetHashCode())
                )
            );

        private static void BuildBicycles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BicycleDto>(action =>
            {
                action.Property(dto => dto.Serial_number)
                      .HasMaxLength(17)
                      .IsRequired();
                action.Property(dto => dto.Title)
                      .IsRequired();
                action.Property(dto => dto.Price)
                      .HasColumnType("money");
                action.HasData(
                    new BicycleDto
                    {
                        ID = 1,
                        Title = "Tsunami snm-100",
                        Serial_number = "Serial: 1122334",
                        Producer = "Tsunami",
                        Description = "Fixie Fixed Gear Bike Tisunami Single Speed Bicycle\r\n\r\nFrame: Aluminum Alloy\r\n\r\nFork: Aluminum Alloy\r\n\r\nSize: 49/52/55/58cm\r\n\r\nFit Height: 163cm-190cm\r\n\r\nWeight: 9.8kg\r\n\r\nWheel Size: 700C\r\n\r\nBrake: Fits V-brake(Not include)\r\n\r\nAll of This Bicycle Cycling Parts are Customizable",
                        Price = 452.79m
                    },

                    new BicycleDto
                    {
                        ID = 2,
                        Title = "6ku Urban Track",
                        Serial_number = "Serial: 1111111",
                        Producer = "6ku",
                        Description = "Do you want a lightweight track bike that won’t break the bank? Then the 6KU Urban Track is the bike for you. Our Urban Track features a lightweight aluminum frame and fork. This is one purchase you won’t regret.\r\n\r\nAll 6KU Urban Track bikes include FREE assembly tools. $30 Value of free tools that is all you will need to assemble your new bike!\r\n\r\n \r\n\r\nLightweight Full Aluminum Frame and Fork\r\n30mm Deep V Double-Walled Alloy Wheels\r\nRide Fixed Gear or Freewheel with a Flip-Flop Hub\r\nEasy Maintenance and Upkeep\r\n30-Day Hassle-Free Return Policy",
                        Price = 259.99m
                    },

                    new BicycleDto
                    {
                        ID = 3,
                        Title = "6ku Fixie BIke",
                        Serial_number = "Serial: 7777777",
                        Producer = "6ku",
                        Description = "Looking for a good bike at a low price? Then the 6KU Fixie is what you’re looking for. It’s the dream single-speed bike that is well-built, sturdy, and ideal for short commutes. Buy it, ride it, and we promise you’ll have a smile on your face. There is no other fixie out there like the 6KU.  \r\n\r\nAll 6KU Fixies come with a $30 value of FREE tools included with every bike purchase.\r\n\r\nComfortable Steel Frame\r\nReliable Front and Rear Brakes\r\nFixed or Freewheel with a Flip-Flop Hub\r\nEasy Maintenance and Upkeep\r\n30-Day Hassle-Free Return Policy",
                        Price = 249.00m
                    }
                );
            });
        }
    }
}
