using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bicycles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Serial_number = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    Producer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bicycles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CellPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DeliveryUniqueCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DeliveryDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryPrice = table.Column<decimal>(type: "money", nullable: false),
                    DeliveryParameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentServiceName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PaymqntDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentParameters = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BicycleId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "ID", "Description", "Price", "Producer", "Serial_number", "Title" },
                values: new object[,]
                {
                    { 1, "Fixie Fixed Gear Bike Tisunami Single Speed Bicycle\r\n\r\nFrame: Aluminum Alloy\r\n\r\nFork: Aluminum Alloy\r\n\r\nSize: 49/52/55/58cm\r\n\r\nFit Height: 163cm-190cm\r\n\r\nWeight: 9.8kg\r\n\r\nWheel Size: 700C\r\n\r\nBrake: Fits V-brake(Not include)\r\n\r\nAll of This Bicycle Cycling Parts are Customizable", 452.79m, "Tsunami", "Serial: 1122334", "Tsunami snm-100" },
                    { 2, "Do you want a lightweight track bike that won’t break the bank? Then the 6KU Urban Track is the bike for you. Our Urban Track features a lightweight aluminum frame and fork. This is one purchase you won’t regret.\r\n\r\nAll 6KU Urban Track bikes include FREE assembly tools. $30 Value of free tools that is all you will need to assemble your new bike!\r\n\r\n \r\n\r\nLightweight Full Aluminum Frame and Fork\r\n30mm Deep V Double-Walled Alloy Wheels\r\nRide Fixed Gear or Freewheel with a Flip-Flop Hub\r\nEasy Maintenance and Upkeep\r\n30-Day Hassle-Free Return Policy", 259.99m, "6ku", "Serial: 1111111", "6ku Urban Track" },
                    { 3, "Looking for a good bike at a low price? Then the 6KU Fixie is what you’re looking for. It’s the dream single-speed bike that is well-built, sturdy, and ideal for short commutes. Buy it, ride it, and we promise you’ll have a smile on your face. There is no other fixie out there like the 6KU.  \r\n\r\nAll 6KU Fixies come with a $30 value of FREE tools included with every bike purchase.\r\n\r\nComfortable Steel Frame\r\nReliable Front and Rear Brakes\r\nFixed or Freewheel with a Flip-Flop Hub\r\nEasy Maintenance and Upkeep\r\n30-Day Hassle-Free Return Policy", 249.00m, "6ku", "Serial: 7777777", "6ku Fixie BIke" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bicycles");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
