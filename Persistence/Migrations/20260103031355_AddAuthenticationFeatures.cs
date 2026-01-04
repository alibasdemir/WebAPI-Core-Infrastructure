using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthenticationFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationTokenExpires",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginIp",
                table: "Users",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpires",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmailVerificationToken", "EmailVerificationTokenExpires", "LastLoginDate", "LastLoginIp", "PasswordHash", "PasswordResetToken", "PasswordResetTokenExpires", "PasswordSalt" },
                values: new object[] { null, null, null, null, new byte[] { 174, 116, 166, 213, 172, 183, 34, 210, 9, 193, 42, 142, 227, 69, 178, 253, 115, 137, 133, 240, 129, 228, 126, 85, 180, 64, 230, 24, 214, 107, 251, 31, 23, 193, 208, 222, 206, 15, 81, 221, 25, 163, 214, 245, 75, 208, 156, 208, 61, 198, 213, 236, 234, 49, 34, 225, 65, 80, 69, 82, 202, 66, 38, 203 }, null, null, new byte[] { 52, 85, 183, 206, 6, 40, 232, 121, 133, 165, 238, 43, 46, 209, 138, 115, 101, 219, 3, 47, 36, 152, 215, 64, 9, 111, 117, 229, 54, 251, 239, 243, 173, 40, 157, 91, 90, 102, 154, 125, 48, 141, 16, 23, 191, 38, 185, 214, 222, 76, 23, 156, 209, 13, 120, 205, 186, 117, 178, 183, 53, 106, 14, 33, 14, 237, 121, 249, 249, 83, 120, 26, 159, 54, 112, 220, 188, 56, 165, 201, 128, 191, 97, 213, 103, 143, 103, 19, 158, 201, 228, 118, 118, 47, 247, 68, 20, 240, 128, 4, 193, 244, 66, 77, 79, 113, 96, 214, 152, 122, 93, 134, 189, 34, 52, 153, 94, 89, 64, 84, 167, 105, 96, 151, 219, 123, 75, 71 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EmailVerificationToken", "EmailVerificationTokenExpires", "LastLoginDate", "LastLoginIp", "PasswordHash", "PasswordResetToken", "PasswordResetTokenExpires", "PasswordSalt" },
                values: new object[] { null, null, null, null, new byte[] { 5, 103, 78, 53, 240, 103, 225, 95, 208, 1, 214, 203, 121, 197, 208, 133, 239, 128, 85, 125, 78, 198, 140, 209, 244, 71, 71, 161, 94, 144, 180, 226, 113, 9, 41, 121, 39, 228, 160, 136, 235, 135, 92, 86, 213, 193, 245, 64, 166, 237, 86, 24, 250, 134, 74, 15, 179, 92, 254, 192, 39, 235, 81, 190 }, null, null, new byte[] { 43, 221, 245, 78, 114, 134, 143, 9, 53, 27, 199, 6, 4, 129, 161, 7, 100, 39, 31, 141, 68, 34, 14, 117, 85, 97, 62, 190, 201, 11, 129, 125, 161, 81, 61, 10, 35, 143, 18, 71, 152, 2, 99, 252, 203, 171, 129, 255, 226, 204, 29, 85, 119, 171, 222, 64, 215, 253, 40, 19, 159, 126, 176, 92, 158, 10, 251, 249, 116, 40, 73, 90, 195, 85, 9, 77, 36, 210, 187, 7, 87, 117, 27, 79, 185, 15, 142, 3, 203, 153, 184, 10, 46, 129, 75, 55, 74, 46, 200, 114, 178, 175, 208, 53, 122, 226, 253, 22, 44, 133, 196, 166, 235, 200, 51, 209, 153, 68, 106, 53, 189, 107, 130, 62, 92, 248, 255, 175 } });

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailVerificationToken",
                table: "Users",
                column: "EmailVerificationToken");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordResetToken",
                table: "Users",
                column: "PasswordResetToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_EmailVerificationToken",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailVerificationTokenExpires",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginIp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpires",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 54, 12, 135, 29, 154, 47, 29, 41, 248, 60, 94, 199, 221, 86, 171, 24, 210, 55, 15, 172, 70, 54, 194, 70, 185, 40, 213, 4, 218, 196, 133, 178, 91, 97, 96, 125, 54, 102, 235, 149, 194, 2, 227, 18, 52, 156, 133, 59, 20, 177, 163, 151, 210, 221, 63, 186, 10, 8, 245, 175, 131, 234, 216, 157 }, new byte[] { 242, 191, 37, 234, 71, 15, 77, 206, 188, 97, 106, 87, 250, 190, 67, 145, 194, 243, 91, 140, 6, 124, 230, 255, 157, 117, 161, 191, 249, 51, 58, 201, 87, 235, 148, 142, 235, 45, 168, 102, 239, 160, 186, 211, 68, 69, 44, 229, 233, 220, 37, 131, 113, 119, 130, 44, 56, 213, 247, 30, 245, 234, 239, 108, 189, 205, 67, 205, 16, 5, 115, 59, 193, 212, 237, 166, 119, 112, 206, 226, 31, 141, 83, 24, 80, 124, 248, 158, 79, 197, 34, 40, 166, 159, 211, 46, 95, 92, 162, 162, 45, 102, 176, 183, 162, 157, 70, 112, 46, 116, 100, 131, 254, 138, 192, 57, 156, 0, 211, 194, 22, 208, 165, 18, 128, 159, 84, 58 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 147, 204, 40, 255, 63, 97, 87, 197, 152, 91, 159, 116, 51, 192, 91, 26, 89, 4, 211, 170, 229, 233, 7, 165, 65, 30, 1, 110, 84, 118, 159, 242, 198, 216, 136, 203, 21, 42, 7, 183, 134, 111, 238, 251, 84, 31, 139, 144, 240, 242, 150, 44, 203, 153, 25, 255, 135, 253, 174, 200, 187, 191, 22, 236 }, new byte[] { 22, 6, 137, 116, 109, 55, 52, 137, 234, 46, 34, 160, 172, 151, 192, 70, 186, 236, 243, 59, 22, 103, 162, 35, 198, 53, 148, 214, 46, 115, 80, 222, 120, 80, 34, 223, 67, 112, 118, 216, 71, 235, 139, 43, 119, 157, 84, 50, 253, 158, 114, 33, 180, 36, 149, 9, 156, 35, 213, 185, 204, 205, 221, 244, 149, 196, 129, 27, 38, 13, 30, 33, 81, 134, 167, 39, 155, 210, 172, 227, 98, 250, 54, 142, 36, 226, 229, 4, 204, 223, 79, 205, 195, 244, 158, 91, 200, 65, 219, 121, 26, 16, 224, 0, 123, 208, 186, 49, 224, 133, 26, 11, 82, 65, 255, 116, 210, 205, 87, 16, 111, 101, 213, 93, 177, 253, 201, 215 } });
        }
    }
}
