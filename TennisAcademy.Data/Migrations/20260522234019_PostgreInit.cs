using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TennisAcademyApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class PostgreInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastWheelSpinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OperationType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    OperationTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Bag Identifier")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false, comment: "Bag Brand in English"),
                    BrandBg = table.Column<string>(type: "text", nullable: false, comment: "Bag Brand in Bulgarian"),
                    Model = table.Column<string>(type: "text", nullable: false, comment: "Bag Model in English"),
                    ModelBg = table.Column<string>(type: "text", nullable: false, comment: "Bag Model in Bulgarian"),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "Bag Price"),
                    Quantity = table.Column<int>(type: "integer", nullable: false, comment: "Available in stock"),
                    ImageUrl = table.Column<string>(type: "text", nullable: false, comment: "Bag Image"),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bags", x => x.Id);
                },
                comment: "Bags Shop");

            migrationBuilder.CreateTable(
                name: "Balls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Ball Identifier")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false, comment: "Ball Brand in English"),
                    BrandBg = table.Column<string>(type: "text", nullable: false, comment: "Ball Brand in Bulgarian"),
                    Model = table.Column<string>(type: "text", nullable: false, comment: "Ball Model in English"),
                    ModelBg = table.Column<string>(type: "text", nullable: false, comment: "Ball Model in Bulgarian"),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "Ball Price"),
                    Quantity = table.Column<int>(type: "integer", nullable: false, comment: "Available in stock"),
                    ImageUrl = table.Column<string>(type: "text", nullable: false, comment: "Ball Image"),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balls", x => x.Id);
                },
                comment: "Balls Shop");

            migrationBuilder.CreateTable(
                name: "Rackets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Racket Identifier")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false, comment: "Racket Brand in English"),
                    BrandBg = table.Column<string>(type: "text", nullable: false, comment: "Racket Brand in Bulgarian"),
                    Model = table.Column<string>(type: "text", nullable: false, comment: "Racket Model in English"),
                    ModelBg = table.Column<string>(type: "text", nullable: false, comment: "Racket Model in Bulgarian"),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "Racket Price"),
                    Quantity = table.Column<int>(type: "integer", nullable: false, comment: "Available in stock"),
                    ImageUrl = table.Column<string>(type: "text", nullable: false, comment: "Racket Image"),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rackets", x => x.Id);
                },
                comment: "Rackets Shop");

            migrationBuilder.CreateTable(
                name: "Surfaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Surface Identifier")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Surface Name in English"),
                    NameBg = table.Column<string>(type: "text", nullable: false, comment: "Surface Name in Bulgarian"),
                    ImageUrl = table.Column<string>(type: "text", nullable: false, comment: "Image of the surface"),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surfaces", x => x.Id);
                },
                comment: "Tennis Academy Surfaces");

            migrationBuilder.CreateTable(
                name: "TournamentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NameBg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Training Type Identifier")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Training Type Name in English"),
                    NameBg = table.Column<string>(type: "text", nullable: false, comment: "Training Type Name in Bulgarian"),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                },
                comment: "Tennis Academy Trainings");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    CoachId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameBg = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DescriptionBg = table.Column<string>(type: "text", nullable: false),
                    Nationality = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NationalityBg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true, comment: "Identity User Identifier linked to this coach"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.CoachId);
                    table.ForeignKey(
                        name: "FK_Coaches_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BagCart",
                columns: table => new
                {
                    BagId = table.Column<int>(type: "integer", nullable: false, comment: "Foreign Key of Bag"),
                    UserId = table.Column<string>(type: "text", nullable: false, comment: "Foreign Key of IdentityUser"),
                    IsGift = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, comment: "Quantity of Bags in Cart"),
                    IsOrdered = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagCart", x => new { x.BagId, x.UserId, x.IsGift });
                    table.ForeignKey(
                        name: "FK_BagCart_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BagCart_Bags_BagId",
                        column: x => x.BagId,
                        principalTable: "Bags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BallCart",
                columns: table => new
                {
                    BallId = table.Column<int>(type: "integer", nullable: false, comment: "Foreign Key of Ball"),
                    UserId = table.Column<string>(type: "text", nullable: false, comment: "Foreign Key of IdentityUser"),
                    IsGift = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, comment: "Quantity of Balls in Cart"),
                    IsOrdered = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BallCart", x => new { x.BallId, x.UserId, x.IsGift });
                    table.ForeignKey(
                        name: "FK_BallCart_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BallCart_Balls_BallId",
                        column: x => x.BallId,
                        principalTable: "Balls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Ball Cart");

            migrationBuilder.CreateTable(
                name: "RacketCart",
                columns: table => new
                {
                    RacketId = table.Column<int>(type: "integer", nullable: false, comment: "Foreign Key of Racket"),
                    UserId = table.Column<string>(type: "text", nullable: false, comment: "Foreign Key of IdentityUser"),
                    IsGift = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, comment: "Quantity of Rackets in Cart"),
                    IsOrdered = table.Column<bool>(type: "boolean", nullable: false),
                    BallId = table.Column<int>(type: "integer", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacketCart", x => new { x.RacketId, x.UserId, x.IsGift });
                    table.ForeignKey(
                        name: "FK_RacketCart_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RacketCart_Balls_BallId",
                        column: x => x.BallId,
                        principalTable: "Balls",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RacketCart_Rackets_RacketId",
                        column: x => x.RacketId,
                        principalTable: "Rackets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Racket Cart");

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TitleBg = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DescriptionBg = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EntryFee = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MaxParticipants = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_TournamentCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TournamentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Reservation Identifier")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Note = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true, comment: "Player Notes in English"),
                    NoteBg = table.Column<string>(type: "text", nullable: true, comment: "Player Notes in Bulgarian"),
                    SurfaceId = table.Column<int>(type: "integer", nullable: false, comment: "Choosing a surface"),
                    CoachId = table.Column<int>(type: "integer", nullable: false, comment: "Choosing a coach"),
                    TrainingTypeId = table.Column<int>(type: "integer", nullable: false, comment: "Choosing a training type"),
                    PlayerId = table.Column<string>(type: "text", nullable: false, comment: "Player Identifer"),
                    Duration = table.Column<int>(type: "integer", nullable: false, comment: "Duration of the session"),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date Select"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "CoachId");
                    table.ForeignKey(
                        name: "FK_Reservations_Surfaces_SurfaceId",
                        column: x => x.SurfaceId,
                        principalTable: "Surfaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Trainings_TrainingTypeId",
                        column: x => x.TrainingTypeId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Player Reservations");

            migrationBuilder.CreateTable(
                name: "UserFavourites",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false, comment: "Foreign Key which references to IdentityUser"),
                    CoachId = table.Column<int>(type: "integer", nullable: false, comment: "Foreign Key which references to Coach"),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavourites", x => new { x.UserId, x.CoachId });
                    table.ForeignKey(
                        name: "FK_UserFavourites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFavourites_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "CoachId");
                },
                comment: "Users Favourite Coach");

            migrationBuilder.CreateTable(
                name: "TournamentsUsers",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    EnrolledOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentsUsers", x => new { x.TournamentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TournamentsUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentsUsers_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Bags",
                columns: new[] { "Id", "Brand", "BrandBg", "ImageUrl", "Model", "ModelBg", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, "Wilson", "Уилсън", "https://cdn.media.amplience.net/i/sportinglife/25918789_0/Team-3-Pack-Tennis-Bag?$default$&fmt=auto&w=540&h=540", "Team 3-Pack", "Тийм 3-Пак", 59.99m, 10 },
                    { 2, "Head", "Хед", "https://media.strefatenisa.com.pl/public/media/20/c1/2b/1721072068/head-tour-team-6r-combi-black-mixed-1.jpg?ts=1745860751", "Tour Team 6R", "Тур Тийм 6Р", 89.99m, 7 },
                    { 3, "Babolat", "Баболат", "https://m.media-amazon.com/images/I/61vGrieRbCL._UF1000,1000_QL80_.jpg", "Pure Drive RHx6", "Пюр Драйв Ер Ха х6", 99.99m, 5 },
                    { 4, "Yonex", "Йонекс", "https://www.midwestracquetsports.com/images/xl/BAG92429BK.jpg?v=1", "Pro Series 9-Pack", "Про Сериес 9-Пак", 129.99m, 4 }
                });

            migrationBuilder.InsertData(
                table: "Balls",
                columns: new[] { "Id", "Brand", "BrandBg", "ImageUrl", "Model", "ModelBg", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, "Wilson", "Уилсън", "https://m.media-amazon.com/images/I/715MEN61aPL._UF1000,1000_QL80_.jpg", "US Open Extra Duty", "Ю Ес Оупън Екстра Дюти", 12.99m, 50 },
                    { 2, "Head", "Хед", "https://cdn.sportdepot.bg/files/catalog/detail/570823_01.jpg", "Tour XT", "Тур Екс Те", 11.49m, 35 },
                    { 3, "Dunlop", "Дънлоп", "https://m.media-amazon.com/images/I/618MvroxyXL._UF1000,1000_QL80_.jpg", "ATP Championship", "Ей Ти Пи Чемпиъншип", 10.99m, 40 }
                });

            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "CoachId", "Age", "Description", "DescriptionBg", "ImageUrl", "IsDeleted", "Name", "NameBg", "Nationality", "NationalityBg", "UserId" },
                values: new object[,]
                {
                    { 1, 38, "One of the greatest tennis players of all time, known for his clay court dominance.", "Един от най-великите тенисисти на всички времена, известен с доминацията си на клей кортове.", "https://imageio.forbes.com/specials-images/imageserve/5ece8a5c938ec500060aae37/0x0.jpg?format=jpg&crop=2462,2460,x503,y156,safe&height=416&width=416&fit=bounds", false, "Rafael Nadal", "Рафаел Надал", "Spanish", "Испанец", null },
                    { 2, 43, "Swiss tennis legend with unmatched elegance and 20 Grand Slam titles.", "Швейцарска тенис легенда с ненадмината елегантност и 20 титли от Големия шлем.", "https://a.espncdn.com/combiner/i?img=/i/headshots/tennis/players/full/425.png", false, "Roger Federer", "Роджър Федерер", "Swiss", "Швейцарец", null },
                    { 3, 37, "Serbian champion, known for his resilience and complete game.", "Сръбски шампион, известен със своята издръжливост и комплексна игра.", "https://a.espncdn.com/i/headshots/tennis/players/full/296.png", false, "Novak Djokovic", "Новак Джокович", "Serbian", "Сърбин", null },
                    { 4, 55, "American icon who redefined tennis in the 90s with a colorful personality.", "Американска икона, която предефинира тениса през 90-те години с колоритна идентичност.", "https://www.atptour.com/-/media/alias/player-headshot/A092", false, "Andre Agassi", "Андре Агаси", "American", "Американец", null },
                    { 5, 68, "Swedish legend with ice-cold nerves and six French Open titles.", "Шведска легенда с ледени нерви и шест титли от Ролан Гарос.", "https://lavercup.com/wp-content/uploads/2022/12/figure-borg-2.png", false, "Björn Borg", "Бьорн Борг", "Swedish", "Швед", null }
                });

            migrationBuilder.InsertData(
                table: "Rackets",
                columns: new[] { "Id", "Brand", "BrandBg", "ImageUrl", "Model", "ModelBg", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, "Wilson", "Уилсън", "https://cdncloudcart.com/28710/products/images/134337/tenis-raketa-wilson-pro-staff-rf-97-v13-0-tns-fr-image_6358bfebb40a9_800x800.jpeg?1666760684", "Pro Staff 97", "Про Стаф 97", 349.99m, 5 },
                    { 2, "Babolat", "Баболат", "https://babolat.bg/image/cache/catalog/tennis/2024/rackets/101474/101474-Pure_Drive_98-136-1-Face_2-250x250.jpg", "Pure Drive", "Пюр Драйв", 299.99m, 8 },
                    { 3, "Head", "Хед", "https://i.sportisimo.com/products/images/1104/1104555/700x700/head-graphene-360-speed-mp_1.jpg", "Graphene 360+ Speed", "Графен 360+ Спийд", 279.99m, 10 },
                    { 4, "Yonex", "Йонекс", "https://us.yonex.com/cdn/shop/files/EZ0898_BlastBlue_5868.jpg?v=1739481973&width=1946", "Ezone 98", "Езоун 98", 319.99m, 6 },
                    { 5, "Prince", "Принс", "https://images.squarespace-cdn.com/content/v1/56e9b38c2b8dde820241b62d/1471886555425-JT9KKFKPOL4FNLAV9ZB0/r2.jpg", "Tour 100P", "Тур 100П", 259.99m, 4 },
                    { 6, "Tecnifibre", "Технифайбър", "https://www.tecnifibre.com/dw/image/v2/BHDN_PRD/on/demandware.static/-/Sites-tecnifibre-master-catalog/default/dwcf93310b/hi-res/T-FIGHT%202025/Packshots/305S/14FI305S5_04.jpg?sw=608&sh=608&sm=fit", "TFight 305", "Т-Файт 305", 289.99m, 7 }
                });

            migrationBuilder.InsertData(
                table: "Surfaces",
                columns: new[] { "Id", "ImageUrl", "Name", "NameBg" },
                values: new object[,]
                {
                    { 1, "https://www.edwardssports.co.uk/pub/media/magefan_blog/Clay_Tennis_Courts.jpg", "Clay", "Клей (Червен корт)" },
                    { 2, "https://www.tennisnerd.net/wp-content/uploads/2024/06/grass-tennis.webp", "Grass", "Трева" },
                    { 3, "https://asltenniscourts.com.au/wp-content/uploads/2021/03/AdobeStock_253105355-1024x683.jpeg", "Hard", "Твърда настилка (Хард корт)" }
                });

            migrationBuilder.InsertData(
                table: "TournamentCategories",
                columns: new[] { "Id", "IsDeleted", "Name", "NameBg" },
                values: new object[,]
                {
                    { 1, false, "Singles Men", "Сингъл Мъже" },
                    { 2, false, "Singles Women", "Сингъл Жени" },
                    { 3, false, "Juniors", "Юноши" },
                    { 4, false, "Doubles Mixed", "Смесени Двойки" },
                    { 5, false, "Veterans 45+", "Ветерани 45+" },
                    { 6, false, "Amateur League", "Аматьорска Лига" }
                });

            migrationBuilder.InsertData(
                table: "Trainings",
                columns: new[] { "Id", "Name", "NameBg" },
                values: new object[,]
                {
                    { 1, "Physical Conditioning Routine", "Физическа подготовка" },
                    { 2, "Technical Skill Development", "Развитие на технически умения" },
                    { 3, "Tactical Game Strategy", "Тактическа стратегия за игра" },
                    { 4, "Mental Toughness Training", "Психологическа устойчивост и ментална тренировка" }
                });

            migrationBuilder.InsertData(
                table: "Tournaments",
                columns: new[] { "Id", "CategoryId", "Description", "DescriptionBg", "EndDate", "EntryFee", "IsDeleted", "MaxParticipants", "StartDate", "Title", "TitleBg" },
                values: new object[,]
                {
                    { 1, 1, "Annual spring tournament open for all non-professional male players. Format: Direct elimination.", "Годишен пролетен турнир, отворен за всички непрофесионални играчи (мъже). Формат: Директна елиминация.", new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 40.00m, false, 32, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring Clay Court Open", "Пролетен отворен шампионат на клей" },
                    { 2, 2, "Special dynamic tournament for women. Beautiful trophies and sponsor prizes provided.", "Специален динамичен турнир за жени. Осигурени са красиви трофеи и награди от спонсори.", new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 35.00m, false, 16, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Academy Women's Cup", "Купа на Академията за жени" },
                    { 3, 3, "Tournament targeted at young talents up to 18 years old. Great opportunity to boost local ranking points.", "Турнир, насочен към млади таланти до 18 години. Страхотна възможност за трупане на точки за местната ранглиста.", new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 20.00m, false, 24, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Junior Summer Slams", "Младежки летен шлем" },
                    { 4, 4, "Bring your partner and fight for the grand trophy. Fun and highly competitive atmosphere.", "Доведете партньора си и се борете за голямия трофей. Забавна и силно конкурентна атмосфера.", new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 50.00m, false, 16, new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Midsummer Mixed Doubles", "Летни Смесени Двойки" },
                    { 5, 5, "Exclusively for players aged 45 and above. Hard court battles, tactical play, and great experience.", "Ексклузивно за играчи на възраст 45 и повече години. Битки на твърди кортове, тактическа игра и страхотно изживяване.", new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 45.00m, false, 32, new DateTime(2026, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Masters Veterans Tournament", "Мастърс Турнир за Ветерани" },
                    { 6, 6, "Perfect tournament for recreation players who want to try competitive tennis. Matches played after 18:00.", "Перфектен турнир за любители, които искат да се пробват в състезателния тенис. Мачовете се играят след 18:00 часа.", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 30.00m, false, 64, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Weekend Warrior Amateur League", "Лига 'Уикенд Воини' за Аматьори" },
                    { 7, 1, "Experience the thrill of playing under the lights. Evening matches on fast hard courts.", "Изживейте тръпката от играта под светлините на прожекторите. Вечерни мачове на бързи твърди кортове.", new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 55.00m, false, 32, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "August Night Hardcourt Championship", "Августовски Нощен Шампионат" },
                    { 8, 2, "Gathering the best local female players for an end-of-season showdown on clay.", "Събиране на най-добрите местни тенисистки за сблъсък в края на сезона на клей корт.", new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 35.00m, false, 16, new DateTime(2026, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Autumn Women Single Open", "Есенен Отворен Шампионат за Жени" },
                    { 9, 3, "An exciting singles tournament for juniors to celebrate the new school season. Lots of prizes.", "Вълнуващ сингъл турнир за юноши по случай новия учебен сезон. Множество награди.", new DateTime(2026, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.00m, false, 32, new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Back to School Youth Cup", "Младежка Купа 'Обратно на Училище'" },
                    { 10, 4, "The ultimate team tournament before moving to indoor courts. Group phase followed by eliminations.", "Финалният отборен турнир преди преместването в закрити кортове. Групова фаза, последвана от елиминации.", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 60.00m, false, 16, new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Golden Autumn Doubles", "Златна Есен Смесени Двойки" },
                    { 11, 1, "The first grand tournament of the winter season inside the academy’s premium heated halls.", "Първият голям турнир за зимния сезон вътре в премиум отопляемите зали на академията.", new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 50.00m, false, 32, new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Indoor Premium Cup", "Закрит Премиум Шампионат" },
                    { 12, 5, "Winter edition of our highly anticipated veteran tournament. Keep the competitive spirit alive.", "Зимно издание на нашия дългоочакван ветерански турнир. Поддържайте състезателния дух жив.", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 45.00m, false, 16, new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Legends 45+ Winter Trophy", "Зимна Трофейна Лига за Легенди 45+" },
                    { 13, 6, "Our final event of the year. All entry fees will be donated to local youth sports development.", "Последното ни събитие за годината. Всички такси за участие ще бъдат дарени за развитието на местния младежки спорт.", new DateTime(2026, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 40.00m, false, 64, new DateTime(2026, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christmas Academy Charity Slams", "Коледен Благотворителен Шлем на Академията" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BagCart_UserId",
                table: "BagCart",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BallCart_UserId",
                table: "BallCart",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_UserId",
                table: "Coaches",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RacketCart_BallId",
                table: "RacketCart",
                column: "BallId");

            migrationBuilder.CreateIndex(
                name: "IX_RacketCart_UserId",
                table: "RacketCart",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CoachId",
                table: "Reservations",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PlayerId",
                table: "Reservations",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SurfaceId",
                table: "Reservations",
                column: "SurfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TrainingTypeId",
                table: "Reservations",
                column: "TrainingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_CategoryId",
                table: "Tournaments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentsUsers_UserId",
                table: "TournamentsUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavourites_CoachId",
                table: "UserFavourites",
                column: "CoachId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BagCart");

            migrationBuilder.DropTable(
                name: "BallCart");

            migrationBuilder.DropTable(
                name: "RacketCart");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "TournamentsUsers");

            migrationBuilder.DropTable(
                name: "UserFavourites");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Bags");

            migrationBuilder.DropTable(
                name: "Balls");

            migrationBuilder.DropTable(
                name: "Rackets");

            migrationBuilder.DropTable(
                name: "Surfaces");

            migrationBuilder.DropTable(
                name: "Trainings");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "TournamentCategories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
