using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class hao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    participantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    questionalbleRelationship = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    dateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    collectionMethod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    relationshipToCustomer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    identityNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Particip__4EE792308B9F7FFD", x => x.participantID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    roleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__CD98460A65E2B3C3", x => x.roleID);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    serviceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    serviceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    serviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    packageType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: false),
                    status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Hoạt động"),
                    extraSampleFee = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Service__4550733F3DC3F25F", x => x.serviceID);
                });

            migrationBuilder.CreateTable(
                name: "SurchargePrice",
                columns: table => new
                {
                    surchargeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sampleType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    surcharge = table.Column<decimal>(type: "money", nullable: true),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Surcharg__F9327BEAB29C560D", x => x.surchargeID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleID = table.Column<int>(type: "int", nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phoneNumber = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    dateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Hoạt động"),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__CB9A1CDF7173C51B", x => x.userID);
                    table.ForeignKey(
                        name: "FK__User__roleID__286302EC",
                        column: x => x.roleID,
                        principalTable: "Role",
                        principalColumn: "roleID");
                });

            migrationBuilder.CreateTable(
                name: "IncludeSurcharge",
                columns: table => new
                {
                    surchargeID = table.Column<int>(type: "int", nullable: false),
                    serviceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__IncludeS__1D677CD9E5ADDEC0", x => new { x.surchargeID, x.serviceID });
                    table.ForeignKey(
                        name: "FK__IncludeSu__servi__300424B4",
                        column: x => x.serviceID,
                        principalTable: "Service",
                        principalColumn: "serviceID");
                    table.ForeignKey(
                        name: "FK__IncludeSu__surch__30F848ED",
                        column: x => x.surchargeID,
                        principalTable: "SurchargePrice",
                        principalColumn: "surchargeID");
                });

            migrationBuilder.CreateTable(
                name: "BlogPost",
                columns: table => new
                {
                    postID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    createdDate = table.Column<DateOnly>(type: "date", nullable: true),
                    image = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    updatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    updateBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__BlogPost__userID__5070F446",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    bookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: false),
                    serviceID = table.Column<int>(type: "int", nullable: false),
                    numberSample = table.Column<int>(type: "int", nullable: false),
                    bookingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Chờ xác nhận"),
                    totalPrice = table.Column<decimal>(type: "numeric(38,2)", nullable: true),
                    appointmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    updateDate = table.Column<DateOnly>(type: "date", nullable: true),
                    updateBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__C6D03BED735C6CA1", x => x.bookingID);
                    table.ForeignKey(
                        name: "FK__Booking__service__35BCFE0A",
                        column: x => x.serviceID,
                        principalTable: "Service",
                        principalColumn: "serviceID");
                    table.ForeignKey(
                        name: "FK__Booking__updateB__36B12243",
                        column: x => x.updateBy,
                        principalTable: "User",
                        principalColumn: "userID");
                    table.ForeignKey(
                        name: "FK__Booking__userID__34C8D9D1",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "Consultant",
                columns: table => new
                {
                    consultantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    consultantDate = table.Column<DateOnly>(type: "date", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    confirmBy = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    createdDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Consulta__8E3CA2DFA05216C0", x => x.consultantID);
                    table.ForeignKey(
                        name: "FK__Consultan__userI__534D60F1",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    feedbackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookingID = table.Column<int>(type: "int", nullable: true),
                    comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    answers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    createDate = table.Column<DateOnly>(type: "date", nullable: true),
                    returnDate = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Hoạt động")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__2613FDC47428A57A", x => x.feedbackID);
                    table.ForeignKey(
                        name: "FK__Feedback__bookin__412EB0B6",
                        column: x => x.bookingID,
                        principalTable: "Booking",
                        principalColumn: "bookingID");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    paymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookingID = table.Column<int>(type: "int", nullable: true),
                    paymentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    amount = table.Column<decimal>(type: "money", nullable: true),
                    paymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Chờ xác nhận")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__A0D9EFA693888D36", x => x.paymentID);
                    table.ForeignKey(
                        name: "FK__Payment__booking__3B75D760",
                        column: x => x.bookingID,
                        principalTable: "Booking",
                        principalColumn: "bookingID");
                });

            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    sampleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookingID = table.Column<int>(type: "int", nullable: false),
                    userID = table.Column<int>(type: "int", nullable: true),
                    participantID = table.Column<int>(type: "int", nullable: false),
                    typeOfCollection = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sampleType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    receivedDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sample__3FD4F24B4EB5FABC", x => x.sampleID);
                    table.ForeignKey(
                        name: "FK__Sample__bookingI__4D94879B",
                        column: x => x.bookingID,
                        principalTable: "Booking",
                        principalColumn: "bookingID");
                    table.ForeignKey(
                        name: "FK__Sample__particip__4E88ABD4",
                        column: x => x.participantID,
                        principalTable: "Participant",
                        principalColumn: "participantID");
                    table.ForeignKey(
                        name: "FK__Sample__userID__4CA06362",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "TestResult",
                columns: table => new
                {
                    testResultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookingID = table.Column<int>(type: "int", nullable: false),
                    resultDate = table.Column<DateOnly>(type: "date", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    createdDate = table.Column<DateOnly>(type: "date", nullable: true),
                    resultConclution = table.Column<string>(type: "text", nullable: true),
                    resultFile = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    updatedDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TestResu__DD1FEAAD9D3F189A", x => x.testResultID);
                    table.ForeignKey(
                        name: "FK__TestResul__booki__44FF419A",
                        column: x => x.bookingID,
                        principalTable: "Booking",
                        principalColumn: "bookingID");
                });

            migrationBuilder.CreateTable(
                name: "DetailResult",
                columns: table => new
                {
                    detailResultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    testResultID = table.Column<int>(type: "int", nullable: false),
                    locusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    p1Allele1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    p1Allele2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    p2Allele1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    p2Allele2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    paternityIndex = table.Column<decimal>(type: "decimal(18,9)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DetailRe__7B658D46718D0C83", x => x.detailResultID);
                    table.ForeignKey(
                        name: "FK__DetailRes__testR__47DBAE45",
                        column: x => x.testResultID,
                        principalTable: "TestResult",
                        principalColumn: "testResultID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_userID",
                table: "BlogPost",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_serviceID",
                table: "Booking",
                column: "serviceID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_updateBy",
                table: "Booking",
                column: "updateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_userID",
                table: "Booking",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Consultant_userID",
                table: "Consultant",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_DetailResult_testResultID",
                table: "DetailResult",
                column: "testResultID");

            migrationBuilder.CreateIndex(
                name: "UQ__Feedback__C6D03BEC18723438",
                table: "Feedback",
                column: "bookingID",
                unique: true,
                filter: "[bookingID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IncludeSurcharge_serviceID",
                table: "IncludeSurcharge",
                column: "serviceID");

            migrationBuilder.CreateIndex(
                name: "UQ__Payment__C6D03BEC52329F2D",
                table: "Payment",
                column: "bookingID",
                unique: true,
                filter: "[bookingID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_bookingID",
                table: "Sample",
                column: "bookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_participantID",
                table: "Sample",
                column: "participantID");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_userID",
                table: "Sample",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "UQ__TestResu__C6D03BECEBF34560",
                table: "TestResult",
                column: "bookingID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_roleID",
                table: "User",
                column: "roleID");

            migrationBuilder.CreateIndex(
                name: "UQ__User__AB6E6164D8437F0F",
                table: "User",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPost");

            migrationBuilder.DropTable(
                name: "Consultant");

            migrationBuilder.DropTable(
                name: "DetailResult");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "IncludeSurcharge");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Sample");

            migrationBuilder.DropTable(
                name: "TestResult");

            migrationBuilder.DropTable(
                name: "SurchargePrice");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
