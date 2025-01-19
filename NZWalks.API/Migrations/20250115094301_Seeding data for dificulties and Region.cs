using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingdatafordificultiesandRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1cb50ef7-b5f4-4be4-8d31-70736e09f507"), "Easy" },
                    { new Guid("46654a23-81ed-44f2-a946-053278bc1ab6"), "Hard" },
                    { new Guid("9bbc1836-0ece-4fde-a159-03aa25978718"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("35e9269d-1371-42c6-a416-29bef6cd0783"), "NTL", "North Land", "Somthing" },
                    { new Guid("65d904c3-1096-4398-a6ca-a6b5fd1a5cd9"), "AKL", "Aukland", "Somthing" },
                    { new Guid("d431b438-ea28-44b4-92e2-8d102e776453"), "WGL", "Wellington", "Somthing" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("1cb50ef7-b5f4-4be4-8d31-70736e09f507"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("46654a23-81ed-44f2-a946-053278bc1ab6"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("9bbc1836-0ece-4fde-a159-03aa25978718"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("35e9269d-1371-42c6-a416-29bef6cd0783"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("65d904c3-1096-4398-a6ca-a6b5fd1a5cd9"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("d431b438-ea28-44b4-92e2-8d102e776453"));
        }
    }
}
