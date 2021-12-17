using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankYouBankruptDatabaseImplement.Migrations
{
    public partial class Bank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillsBalance = table.Column<float>(nullable: false),
                    BillsNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FIO = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    Number = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AplicationSum = table.Column<decimal>(nullable: false),
                    AplicationDate = table.Column<DateTime>(nullable: false),
                    AplicationNumber = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Application_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardsNumder = table.Column<string>(nullable: false),
                    SecurityCode = table.Column<int>(nullable: false),
                    ServiceEndDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Card_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationDate = table.Column<DateTime>(nullable: false),
                    OperationType = table.Column<string>(nullable: true),
                    OperationNumber = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashWithdrawals",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(nullable: false),
                    AvailabilityApplication = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashWithdrawals", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_CashWithdrawals_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardsApplications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(nullable: false),
                    AplicationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardsApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardsApplications_Application_AplicationId",
                        column: x => x.AplicationId,
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardsApplications_Card_CardId",
                        column: x => x.CardId,
                        principalTable: "Card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardsOperations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationId = table.Column<int>(nullable: false),
                    CardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardsOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardsOperations_Card_CardId",
                        column: x => x.CardId,
                        principalTable: "Card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardsOperations_Operation_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoneyTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<string>(nullable: false),
                    Recipient = table.Column<string>(nullable: false),
                    SendersCard = table.Column<string>(nullable: false),
                    RecipientsCard = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    OperationType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyTransfers_Operation_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashWithdrawalBills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashWithdrawalId = table.Column<int>(nullable: false),
                    BillsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashWithdrawalBills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashWithdrawalBills_Bill_BillsId",
                        column: x => x.BillsId,
                        principalTable: "Bill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashWithdrawalBills_CashWithdrawals_CashWithdrawalId",
                        column: x => x.CashWithdrawalId,
                        principalTable: "CashWithdrawals",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoneyTransferApplications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoneyTransferId = table.Column<int>(nullable: false),
                    AplicationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransferApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyTransferApplications_Application_AplicationId",
                        column: x => x.AplicationId,
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoneyTransferApplications_MoneyTransfers_MoneyTransferId",
                        column: x => x.MoneyTransferId,
                        principalTable: "MoneyTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MoneyTransferBills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoneyTransferId = table.Column<int>(nullable: false),
                    BillsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransferBills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyTransferBills_Bill_BillsId",
                        column: x => x.BillsId,
                        principalTable: "Bill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoneyTransferBills_MoneyTransfers_MoneyTransferId",
                        column: x => x.MoneyTransferId,
                        principalTable: "MoneyTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Application_UserId",
                table: "Application",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_UserId",
                table: "Card",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CardsApplications_AplicationId",
                table: "CardsApplications",
                column: "AplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CardsApplications_CardId",
                table: "CardsApplications",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardsOperations_CardId",
                table: "CardsOperations",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardsOperations_OperationId",
                table: "CardsOperations",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CashWithdrawalBills_BillsId",
                table: "CashWithdrawalBills",
                column: "BillsId");

            migrationBuilder.CreateIndex(
                name: "IX_CashWithdrawalBills_CashWithdrawalId",
                table: "CashWithdrawalBills",
                column: "CashWithdrawalId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransferApplications_AplicationId",
                table: "MoneyTransferApplications",
                column: "AplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransferApplications_MoneyTransferId",
                table: "MoneyTransferApplications",
                column: "MoneyTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransferBills_BillsId",
                table: "MoneyTransferBills",
                column: "BillsId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransferBills_MoneyTransferId",
                table: "MoneyTransferBills",
                column: "MoneyTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransfers_OperationId",
                table: "MoneyTransfers",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_UserId",
                table: "Operation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardsApplications");

            migrationBuilder.DropTable(
                name: "CardsOperations");

            migrationBuilder.DropTable(
                name: "CashWithdrawalBills");

            migrationBuilder.DropTable(
                name: "MoneyTransferApplications");

            migrationBuilder.DropTable(
                name: "MoneyTransferBills");

            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "CashWithdrawals");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "MoneyTransfers");

            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropTable(
                name: "Operation");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
