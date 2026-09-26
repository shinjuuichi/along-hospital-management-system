using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FeedbackSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Rating = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FeedbackTypeEnum = table.Column<int>(type: "int", nullable: false),
                    FeedbackReplyStatus = table.Column<int>(type: "int", nullable: false),
                    FeedbackStatus = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FeedbackId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackReport_Feedback_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackRespond",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeedbackStatus = table.Column<int>(type: "int", nullable: false),
                    FeedbackId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackRespond", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackRespond_Feedback_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Feedback",
                columns: new[] { "Id", "Content", "CreatedBy", "CreationDate", "DeletionDate", "FeedbackReplyStatus", "FeedbackStatus", "FeedbackTypeEnum", "IsDeleted", "MedicineId", "ModificationDate", "ModifiedBy", "Rating" },
                values: new object[,]
                {
                    { 1, "Great medicine, really helped with my symptoms and had no side effects.", 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 1, false, 1, null, null, 5m },
                    { 2, "Felt better after a few days of use. Works as described.", 39, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 4m },
                    { 3, "The medicine was okay, but caused mild dizziness after taking it.", 40, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 2, false, 1, null, null, 3m },
                    { 4, "Easy to swallow and reduced my fever quickly.", 41, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 1, false, 1, null, null, 5m },
                    { 5, "Relieved my headache well, but the tablet tasted a little bitter.", 42, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 4m },
                    { 6, "Worked for my muscle pain and I did not notice stomach discomfort.", 43, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 1, false, 1, null, null, 5m },
                    { 7, "Packaging was neat and the medicine arrived with a long expiry date.", 44, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 4m },
                    { 8, "Helped lower my fever the same evening and I slept much better.", 45, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 1, false, 1, null, null, 5m },
                    { 9, "The effect wore off sooner than I expected, but it still helped.", 46, new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 1, null, null, 3m },
                    { 10, "Reliable pain relief and easy to keep at home for common use.", 47, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 1, false, 1, null, null, 5m },
                    { 11, "It reduced body aches, though I needed to take another dose later.", 48, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 1, null, null, 3m },
                    { 12, "Very useful for family use and the instructions were easy to follow.", 49, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 1, false, 1, null, null, 5m },
                    { 13, "The medicine worked, but delivery took longer than expected.", 50, new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 4m },
                    { 14, "Good quality product and the fever relief was stable for several hours.", 51, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 1, false, 1, null, null, 5m },
                    { 15, "My headache improved, but the result was only moderate for me.", 52, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 1, null, null, 3m },
                    { 16, "I used it after vaccination and the discomfort improved quickly.", 53, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 1, false, 1, null, null, 5m },
                    { 17, "Did not notice much improvement after the first dose, maybe needs more time.", 54, new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 3, false, 1, null, null, 2m },
                    { 18, "Pain relief was reliable and the medicine did not upset my stomach.", 55, new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 1, false, 1, null, null, 5m },
                    { 19, "The tablet size was fine, but I hoped it would act a bit faster.", 56, new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 4m },
                    { 20, "Helpful product overall, but the instructions could be clearer for first-time users.", 57, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, 2, false, 1, null, null, 3m },
                    { 21, "Worked well for toothache and the relief lasted through the night.", 58, new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 5m },
                    { 22, "The result was decent, though I still needed rest to recover fully.", 59, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 4m },
                    { 23, "A practical medicine to keep in the cabinet for sudden fever.", 60, new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 5m },
                    { 24, "Helped my sore throat discomfort a little, but not as much as expected.", 61, new DateTime(2025, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 1, null, null, 3m },
                    { 25, "Easy to use, affordable, and the quality felt trustworthy.", 62, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 5m },
                    { 26, "The medicine worked for me, although I felt slightly sleepy afterward.", 63, new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 1, null, null, 3m },
                    { 27, "A dependable choice for common pain and fever in my family.", 3, new DateTime(2025, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 5m },
                    { 28, "Took effect after about an hour and eased my headache enough to work.", 39, new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 4m },
                    { 29, "No major complaints, but the effect felt average compared with other brands.", 40, new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 1, null, null, 3m },
                    { 30, "Very satisfied with the outcome and would purchase it again.", 41, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 1, null, null, 5m },
                    { 31, "Vitamin tablets had a pleasant taste and were easy to take every morning.", 3, new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 32, "I felt less tired after using it regularly for a week.", 39, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 4m },
                    { 33, "The tablets were a bit large, but the overall quality felt good.", 40, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 4m },
                    { 34, "I did not notice much improvement after two uses, maybe it needs more time.", 41, new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 3, false, 3, null, null, 2m },
                    { 35, "Good supplement to keep at home during weather changes.", 42, new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 36, "The flavor was acceptable and the product looked genuine.", 43, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 4m },
                    { 37, "Helped me recover faster from a mild cold when combined with rest.", 44, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 38, "I expected more energy, but the effect was only moderate.", 45, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 3, null, null, 3m },
                    { 39, "Easy to add to my daily routine and did not upset my stomach.", 46, new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 40, "The package arrived in good condition and the seal was intact.", 47, new DateTime(2025, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 4m },
                    { 41, "Useful product, though the price is a little higher than similar options.", 48, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 4m },
                    { 42, "I felt more comfortable during flu season after taking it daily.", 49, new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 43, "The result was acceptable, but I hoped the tablets would dissolve faster.", 50, new DateTime(2025, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 3, null, null, 3m },
                    { 44, "Taste was fine and there was no strange aftertaste.", 51, new DateTime(2025, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 45, "My throat irritation improved slightly after a few days.", 52, new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 4m },
                    { 46, "The product looked clean and trustworthy from packaging to tablet quality.", 53, new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 47, "It worked for me, but not as noticeably as I expected.", 54, new DateTime(2025, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 3, null, null, 3m },
                    { 48, "A convenient vitamin choice for busy mornings.", 55, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 49, "The tablets were easy to swallow and did not cause nausea.", 56, new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 50, "I used it during recovery and felt my appetite improve a little.", 57, new DateTime(2025, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 4m },
                    { 51, "No major issues, but the effect felt gradual rather than immediate.", 58, new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 3, null, null, 3m },
                    { 52, "My family also used it and everyone found it easy to take.", 59, new DateTime(2025, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 53, "The quality seems stable and the brand feels reliable.", 60, new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 54, "I had mild stomach warmth once, but it went away quickly.", 61, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 3, null, null, 3m },
                    { 55, "Helpful as a daily supplement, especially when I do not eat enough fruit.", 62, new DateTime(2025, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 56, "The bottle was easy to open and store in my bag.", 63, new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 4m },
                    { 57, "I bought it for immune support and I am generally satisfied.", 3, new DateTime(2025, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 4m },
                    { 58, "The benefit was there, but the pace felt slower than expected.", 39, new DateTime(2025, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 3, null, null, 3m },
                    { 59, "Clean product, good taste, and no unpleasant side effects.", 40, new DateTime(2025, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 60, "Overall a good supplement and I would consider buying it again.", 41, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 3, null, null, 5m },
                    { 61, "I used it for dry eyes and the improvement was noticeable after two days.", 3, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 62, "The eye drops were gentle and did not sting like some other brands.", 39, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 63, "My eyes felt less irritated, but I still needed to use the drops several times a day.", 40, new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 64, "The bottle was easy to use, though the drop size felt a little large.", 41, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 65, "Helped with screen-related dryness during office hours.", 42, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 66, "Relief came quickly and there was no burning sensation.", 43, new DateTime(2025, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 67, "The formula felt smooth, but I hoped the moisture would last longer.", 44, new DateTime(2025, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 68, "Good product for daily use when my eyes feel tired.", 45, new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 69, "The cooling feeling was comfortable and not too strong.", 46, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 70, "Easy to carry in my bag and use while traveling.", 47, new DateTime(2025, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 71, "My eye discomfort improved, though not completely on the first day.", 48, new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 72, "No irritation after use and the quality felt very stable.", 49, new DateTime(2025, 3, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 73, "Helpful after long hours with contact lenses.", 50, new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 74, "The bottle design is practical and easy to control.", 51, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 75, "My eyes stopped feeling gritty after regular use.", 52, new DateTime(2025, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 76, "A good choice for mild dryness, though severe cases may need more support.", 53, new DateTime(2025, 3, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 77, "The drops were clean and easy to apply even for first-time users.", 54, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 78, "I noticed clearer vision comfort while reading for long periods.", 55, new DateTime(2025, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 79, "The effect was decent, but I needed to reapply by afternoon.", 56, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 5, null, null, 3m },
                    { 80, "Very comfortable product and suitable for office workers.", 57, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 81, "No strong smell, no sting, and the bottle stayed leak-free.", 58, new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 82, "Worked well in air-conditioned rooms where my eyes usually dry out.", 59, new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 83, "The relief was moderate but still worth using.", 60, new DateTime(2025, 3, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 84, "My eye redness reduced a little after a few applications.", 61, new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 85, "Very handy for travel and did not take much space.", 62, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 86, "I liked the gentle formula and would buy it again.", 63, new DateTime(2025, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 87, "Good daily support for dry eye symptoms after computer work.", 3, new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 88, "The product quality was fine, but the cap was a little tight to open.", 39, new DateTime(2025, 3, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 4m },
                    { 89, "A reliable option when my eyes feel strained and tired.", 40, new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 90, "Overall very satisfied because it eased discomfort without irritation.", 41, new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 5, null, null, 5m },
                    { 91, "The powder dissolved quickly in water and was convenient during stomach illness.", 3, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 92, "The taste was hard to get used to, but it helped me recover from dehydration.", 39, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 4m },
                    { 93, "Very useful to keep at home. My child recovered well after using it as directed.", 40, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 94, "Packaging was secure and the product matched the description on the page.", 41, new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 4m },
                    { 95, "Worked well after stomach upset and helped me feel steadier.", 42, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 96, "The salty taste was strong, but the result was worth it.", 43, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 4m },
                    { 97, "Easy to prepare and useful for emergencies at home.", 44, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 98, "I noticed improvement after the first glass and felt less weak.", 45, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 99, "Helpful product overall, though the instructions could be clearer for first-time users.", 46, new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 8, null, null, 3m },
                    { 100, "Good to keep in the cabinet for children and adults alike.", 47, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 101, "The sachet was easy to tear and mix with water.", 48, new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 102, "I recovered from diarrhea more comfortably after using it.", 49, new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 103, "The flavor is not great, but the hydration support is effective.", 50, new DateTime(2025, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 4m },
                    { 104, "Convenient travel item and useful during hot days.", 51, new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 105, "My child accepted the taste better than I expected.", 52, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 4m },
                    { 106, "Worked reliably and the quality felt consistent.", 53, new DateTime(2025, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 107, "I would like a milder taste, but the product itself is helpful.", 54, new DateTime(2025, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 8, null, null, 3m },
                    { 108, "Very practical medicine to keep ready for dehydration risks.", 55, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 109, "The instructions were simple enough and preparation took less than a minute.", 56, new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 110, "It helped after vomiting, although I needed to sip it slowly.", 57, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 4m },
                    { 111, "Affordable, effective, and easy to store at home.", 58, new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 112, "The taste was slightly unpleasant, but I still finished the dose.", 59, new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 8, null, null, 3m },
                    { 113, "Useful during fever and stomach illness when hydration is important.", 60, new DateTime(2025, 4, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 114, "The product quality seemed good and the sachets were well sealed.", 61, new DateTime(2025, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 115, "My energy improved after rehydrating with this product.", 62, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 116, "It worked as expected, though I would prefer smaller sachets for children.", 63, new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 4m },
                    { 117, "A dependable product and easy to recommend for home use.", 3, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 118, "The result was good, but the flavor may not suit everyone.", 39, new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 2, false, 8, null, null, 3m },
                    { 119, "I keep this product in the house because it has been useful more than once.", 40, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m },
                    { 120, "Overall satisfied because it helped quickly during dehydration.", 41, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, 1, false, 8, null, null, 5m }
                });

            migrationBuilder.InsertData(
                table: "FeedbackReport",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "FeedbackId", "IsDeleted", "ModificationDate", "ModifiedBy", "Reason", "Status" },
                values: new object[,]
                {
                    { 1, 39, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, "Content appears overly promotional and should be reviewed by moderation.", 0 },
                    { 2, 40, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, "Feedback mentions product performance but lacks enough detail, needs moderation review.", 1 },
                    { 3, 41, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, false, null, null, "Feedback describes side effects and was reviewed as acceptable health-related sharing.", 2 },
                    { 4, 42, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 7, false, null, null, "Report requests moderation to verify whether the content is too generic.", 0 },
                    { 5, 43, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 11, false, null, null, "The reviewer expressed moderate dissatisfaction, but the wording remains within policy.", 2 },
                    { 6, 44, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 13, false, null, null, "This feedback mixes medicine quality and delivery experience, needs moderator decision.", 1 },
                    { 7, 45, new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, 17, false, null, null, "Negative experience was reported and checked by moderation for accuracy and wording.", 0 },
                    { 8, 46, new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, 20, false, null, null, "Constructive criticism is valid and does not violate policy after review.", 2 }
                });

            migrationBuilder.InsertData(
                table: "FeedbackRespond",
                columns: new[] { "Id", "Content", "CreatedBy", "CreationDate", "DeletionDate", "FeedbackId", "FeedbackStatus", "IsDeleted", "ModificationDate", "ModifiedBy" },
                values: new object[,]
                {
                    { 1, "Thank you for your positive feedback! We're glad it helped.", 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 0, false, null, null },
                    { 2, "We appreciate your honesty. Please consult your doctor if dizziness continues.", 12, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 0, false, null, null },
                    { 3, "Thank you for sharing. We are happy the medicine reduced your fever quickly.", 11, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 0, false, null, null },
                    { 4, "We appreciate your review and are glad the eye drops felt comfortable to use.", 12, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 6, 0, false, null, null },
                    { 5, "Thank you for your feedback. We are glad the medicine supported your recovery.", 11, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 8, 0, false, null, null },
                    { 6, "We appreciate the review. Our team is happy to know the eye drops were gentle for you.", 12, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, 10, 0, false, null, null },
                    { 7, "Thank you for keeping this product at home and using it as directed.", 11, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 12, 0, false, null, null },
                    { 8, "We are glad the vitamin tablets were easy to include in your daily routine.", 12, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 14, 0, false, null, null },
                    { 9, "Thank you for your feedback. We are pleased the powder was convenient during illness.", 11, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, 16, 0, false, null, null },
                    { 10, "We appreciate your detailed review and are glad the pain relief was reliable.", 12, new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, 18, 0, false, null, null },
                    { 11, "Thank you for the suggestion. We will review the product instructions for clarity.", 11, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 20, 0, false, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackReport_FeedbackId",
                table: "FeedbackReport",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackRespond_FeedbackId",
                table: "FeedbackRespond",
                column: "FeedbackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackReport");

            migrationBuilder.DropTable(
                name: "FeedbackRespond");

            migrationBuilder.DropTable(
                name: "Feedback");
        }
    }
}
