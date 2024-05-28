using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_users_ID",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupon_Booking_BookingId",
                table: "Coupon");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupon_users_UserId",
                table: "Coupon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coupon",
                table: "Coupon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "Coupon",
                newName: "coupons");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "bookings");

            migrationBuilder.RenameIndex(
                name: "IX_Coupon_UserId",
                table: "coupons",
                newName: "IX_coupons_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Coupon_BookingId",
                table: "coupons",
                newName: "IX_coupons_BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_ID",
                table: "bookings",
                newName: "IX_bookings_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_coupons",
                table: "coupons",
                column: "CouponId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookings",
                table: "bookings",
                column: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_users_ID",
                table: "bookings",
                column: "ID",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_coupons_bookings_BookingId",
                table: "coupons",
                column: "BookingId",
                principalTable: "bookings",
                principalColumn: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_coupons_users_UserId",
                table: "coupons",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_users_ID",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_coupons_bookings_BookingId",
                table: "coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_coupons_users_UserId",
                table: "coupons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_coupons",
                table: "coupons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookings",
                table: "bookings");

            migrationBuilder.RenameTable(
                name: "coupons",
                newName: "Coupon");

            migrationBuilder.RenameTable(
                name: "bookings",
                newName: "Booking");

            migrationBuilder.RenameIndex(
                name: "IX_coupons_UserId",
                table: "Coupon",
                newName: "IX_Coupon_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_coupons_BookingId",
                table: "Coupon",
                newName: "IX_Coupon_BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_ID",
                table: "Booking",
                newName: "IX_Booking_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coupon",
                table: "Coupon",
                column: "CouponId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_users_ID",
                table: "Booking",
                column: "ID",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupon_Booking_BookingId",
                table: "Coupon",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupon_users_UserId",
                table: "Coupon",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
