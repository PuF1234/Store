﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Store.Data.EF;

#nullable disable

namespace Store.Data.EF.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20240207043021_FullTextSearch")]
    partial class FullTextSearch
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Store.Data.BicycleDto", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<string>("Producer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Serial_number")
                        .IsRequired()
                        .HasMaxLength(17)
                        .HasColumnType("nvarchar(17)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Bicycles");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Description = "Fixie Fixed Gear Bike Tisunami Single Speed Bicycle\r\n\r\nFrame: Aluminum Alloy\r\n\r\nFork: Aluminum Alloy\r\n\r\nSize: 49/52/55/58cm\r\n\r\nFit Height: 163cm-190cm\r\n\r\nWeight: 9.8kg\r\n\r\nWheel Size: 700C\r\n\r\nBrake: Fits V-brake(Not include)\r\n\r\nAll of This Bicycle Cycling Parts are Customizable",
                            Price = 452.79m,
                            Producer = "Tsunami",
                            Serial_number = "Serial: 1122334",
                            Title = "Tsunami snm-100"
                        },
                        new
                        {
                            ID = 2,
                            Description = "Do you want a lightweight track bike that won’t break the bank? Then the 6KU Urban Track is the bike for you. Our Urban Track features a lightweight aluminum frame and fork. This is one purchase you won’t regret.\r\n\r\nAll 6KU Urban Track bikes include FREE assembly tools. $30 Value of free tools that is all you will need to assemble your new bike!\r\n\r\n \r\n\r\nLightweight Full Aluminum Frame and Fork\r\n30mm Deep V Double-Walled Alloy Wheels\r\nRide Fixed Gear or Freewheel with a Flip-Flop Hub\r\nEasy Maintenance and Upkeep\r\n30-Day Hassle-Free Return Policy",
                            Price = 259.99m,
                            Producer = "6ku",
                            Serial_number = "Serial: 1111111",
                            Title = "6ku Urban Track"
                        },
                        new
                        {
                            ID = 3,
                            Description = "Looking for a good bike at a low price? Then the 6KU Fixie is what you’re looking for. It’s the dream single-speed bike that is well-built, sturdy, and ideal for short commutes. Buy it, ride it, and we promise you’ll have a smile on your face. There is no other fixie out there like the 6KU.  \r\n\r\nAll 6KU Fixies come with a $30 value of FREE tools included with every bike purchase.\r\n\r\nComfortable Steel Frame\r\nReliable Front and Rear Brakes\r\nFixed or Freewheel with a Flip-Flop Hub\r\nEasy Maintenance and Upkeep\r\n30-Day Hassle-Free Return Policy",
                            Price = 249.00m,
                            Producer = "6ku",
                            Serial_number = "Serial: 7777777",
                            Title = "6ku Fixie BIke"
                        });
                });

            modelBuilder.Entity("Store.Data.OrderDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CellPhone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("DeliveryDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryParameters")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DeliveryPrice")
                        .HasColumnType("money");

                    b.Property<string>("DeliveryUniqueCode")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PaymentParameters")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentServiceName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PaymqntDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Store.Data.OrderItemDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BicycleId")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Store.Data.OrderItemDto", b =>
                {
                    b.HasOne("Store.Data.OrderDto", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Store.Data.OrderDto", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
