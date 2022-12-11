using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingNumbers.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumbersLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderingTimeMilliseconds = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumbersLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Numbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    NumbersListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Numbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Numbers_NumbersLists_NumbersListId",
                        column: x => x.NumbersListId,
                        principalTable: "NumbersLists",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Numbers",
                columns: new[] { "Id", "NumbersListId", "Value" },
                values: new object[] { new Guid("7980f2dc-999c-482d-aea1-11c247c84cca"), null, 0 });

            migrationBuilder.InsertData(
                table: "NumbersLists",
                columns: new[] { "Id", "OrderType", "OrderingTimeMilliseconds" },
                values: new object[] { new Guid("8818db8a-4214-416f-a679-62b640fec184"), "Unsorted", 0L });

            migrationBuilder.CreateIndex(
                name: "IX_Numbers_NumbersListId",
                table: "Numbers",
                column: "NumbersListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Numbers");

            migrationBuilder.DropTable(
                name: "NumbersLists");
        }
    }
}
