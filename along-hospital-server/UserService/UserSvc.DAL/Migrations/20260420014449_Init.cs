using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Address", "CreatedBy", "CreationDate", "DateOfBirth", "DeletionDate", "Gender", "Image", "IsDeleted", "ModificationDate", "ModifiedBy", "Name", "Role" },
                values: new object[,]
                {
                    { 2, "111 Dong Khoi, quan 1, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1983, 3, 15), null, 1, null, false, null, null, "Manager User", 1 },
                    { 3, "222 Ly Thuong Kiet, quan 10, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1992, 8, 15), null, 1, null, false, null, null, "Patient User", 2 },
                    { 4, "333 Nguyen Van Cu, quan 5, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1980, 3, 10), null, 1, "Staff/2799d78a52ba482aabb2fff1552a11b4.webp", false, null, null, "Doctor User", 3 },
                    { 5, "444 An Duong Vuong, quan 5, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1990, 5, 20), null, 2, null, false, null, null, "Nurse User", 4 },
                    { 6, "555 Le Van Sy, quan 3, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1988, 11, 25), null, 2, null, false, null, null, "HR User", 5 },
                    { 7, "666 Cach Mang Thang 8, quan 10, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1987, 7, 12), null, 1, null, false, null, null, "Pharmacist User", 6 },
                    { 8, "777 Nguyen Thi Minh Khai, quan 1, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1991, 4, 18), null, 2, null, false, null, null, "Accountant User", 7 },
                    { 9, "888 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1993, 9, 5), null, 1, null, false, null, null, "Marketer User", 8 },
                    { 11, "100 Hoang Van Thu, quan Tan Binh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1995, 6, 14), null, 2, null, false, null, null, "Receptionist User", 10 },
                    { 12, "200 Truong Chinh, quan Tan Phu, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1994, 10, 22), null, 1, null, false, null, null, "Hotline Agent User", 11 },
                    { 13, "300 Luy Ban Bich, quan Tan Phu, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1996, 12, 8), null, 1, null, false, null, null, "Inventory Clerk User", 12 },
                    { 14, "150 Nguyen Du, quan 1, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1978, 1, 1), null, 1, "Staff/e775bca93b33420da05be0ff2260bf55.webp", false, null, null, "Nguyen Quoc Bao", 3 },
                    { 15, "151 Nguyen Du, quan 2, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1979, 2, 2), null, 2, "Staff/93f6e4c206bf4dd68bba2451c45f6f10.webp", false, null, null, "Tran Minh Quan", 3 },
                    { 16, "152 Nguyen Du, quan 3, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1980, 3, 3), null, 1, "Staff/511d363b7db842c19a5f302a208b65e2.webp", false, null, null, "Le Hoang Phuc", 3 },
                    { 17, "153 Nguyen Du, quan 4, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1981, 4, 4), null, 2, "Staff/4aac037d513a4a3cb8e46f51bbd5a9f5.webp", false, null, null, "Pham Duc Thinh", 3 },
                    { 18, "154 Nguyen Du, quan 5, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1982, 5, 5), null, 1, "Staff/700ecf6913ab4417959e6bfababf22e0.webp", false, null, null, "Vo Gia Huy", 3 },
                    { 19, "155 Nguyen Du, quan 6, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1983, 6, 6), null, 2, "Staff/a7b1670d959f43059676e25b2a07eabf.webp", false, null, null, "Dang Tuan Kiet", 3 },
                    { 20, "156 Nguyen Du, quan 1, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1984, 7, 7), null, 1, "Staff/9ef3ae9834be48ab93407983fcb17711.webp", false, null, null, "Bui Thanh Nam", 3 },
                    { 21, "157 Nguyen Du, quan 2, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1985, 8, 8), null, 2, "Staff/3af8d6407b7446638ab1d3f015f49290.webp", false, null, null, "Doan Quang Hiep", 3 },
                    { 22, "158 Nguyen Du, quan 3, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1986, 9, 9), null, 1, "Staff/9168d20b24264cd88731852aa898501f.webp", false, null, null, "Hoang Anh Khoa", 3 },
                    { 39, "210 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1989, 4, 1), null, 1, null, false, null, null, "Nguyen Hoang Minh", 2 },
                    { 40, "211 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1990, 5, 3), null, 2, null, false, null, null, "Tran Gia Han", 2 },
                    { 41, "212 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1991, 6, 5), null, 1, null, false, null, null, "Le Duc Anh", 2 },
                    { 42, "213 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1992, 7, 7), null, 2, null, false, null, null, "Pham Thu Trang", 2 },
                    { 43, "214 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1993, 8, 9), null, 1, null, false, null, null, "Vo Quynh Nhu", 2 },
                    { 44, "215 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1994, 9, 11), null, 2, null, false, null, null, "Dang Thanh Tung", 2 },
                    { 45, "216 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1995, 10, 13), null, 1, null, false, null, null, "Bui Ngoc Mai", 2 },
                    { 46, "217 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1996, 11, 15), null, 2, null, false, null, null, "Do Gia Bao", 2 },
                    { 47, "218 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1997, 12, 17), null, 1, null, false, null, null, "Hoang Tu Linh", 2 },
                    { 48, "219 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1998, 1, 19), null, 2, null, false, null, null, "Phan Minh Khang", 2 },
                    { 49, "220 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1999, 2, 21), null, 1, null, false, null, null, "Huynh Bao Chau", 2 },
                    { 50, "221 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2000, 3, 23), null, 2, null, false, null, null, "Ngo Thanh Truc", 2 },
                    { 51, "222 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1989, 4, 25), null, 1, null, false, null, null, "Truong Gia Huy", 2 },
                    { 52, "223 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1990, 5, 27), null, 2, null, false, null, null, "Duong Nhat Vy", 2 },
                    { 53, "224 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1991, 6, 2), null, 1, null, false, null, null, "Ly Minh Thu", 2 },
                    { 54, "225 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1992, 7, 4), null, 2, null, false, null, null, "Mai Quoc Khanh", 2 },
                    { 55, "226 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1993, 8, 6), null, 1, null, false, null, null, "Cao Bao Ngoc", 2 },
                    { 56, "227 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1994, 9, 8), null, 2, null, false, null, null, "Dinh Thanh Ha", 2 },
                    { 57, "228 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1995, 10, 10), null, 1, null, false, null, null, "Chu Ngoc Han", 2 },
                    { 58, "229 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1996, 11, 12), null, 2, null, false, null, null, "Lam Minh Quan", 2 },
                    { 59, "230 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1997, 12, 14), null, 1, null, false, null, null, "Ta Bao Tran", 2 },
                    { 60, "231 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1998, 1, 16), null, 2, null, false, null, null, "Ton That Nhu Y", 2 },
                    { 61, "232 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1999, 2, 18), null, 1, null, false, null, null, "Kieu Thanh Nhan", 2 },
                    { 62, "233 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2000, 3, 20), null, 2, null, false, null, null, "Quach Bao Han", 2 },
                    { 63, "234 Dien Bien Phu, quan Binh Thanh, thanh pho HCM", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1989, 4, 22), null, 1, null, false, null, null, "Thai Anh Thu", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
