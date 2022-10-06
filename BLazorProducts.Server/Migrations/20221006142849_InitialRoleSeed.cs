using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BLazorProducts.Server.Migrations
{
    public partial class InitialRoleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "260c99ce-4312-487f-b1f9-121f03f2a720", "863c8bb4-ac63-4cad-9182-f622d415b9ec", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6c966b7b-6785-47df-ba3c-a2869b9311c0", "a8857e6a-525c-4f56-a0f2-133d2722ed87", "Viewer", "VIEWER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "260c99ce-4312-487f-b1f9-121f03f2a720");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6c966b7b-6785-47df-ba3c-a2869b9311c0");
        }
    }
}
