using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimeWebApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dubbing",
                columns: table => new
                {
                    DubbingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FriendlyUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dubbing", x => x.DubbingId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FriendlyUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "MpaaRates",
                columns: table => new
                {
                    MpaaRateId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FriendlyUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MpaaRates", x => x.MpaaRateId);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FriendlyUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Studios",
                columns: table => new
                {
                    StudioId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FriendlyUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studios", x => x.StudioId);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    TypeAnimeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FriendlyUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.TypeAnimeId);
                });

            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleEn = table.Column<string>(type: "text", nullable: true),
                    TitleRu = table.Column<string>(type: "text", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Rate = table.Column<double>(type: "double precision", nullable: true),
                    CountEpisode = table.Column<int>(type: "integer", nullable: true),
                    Planned = table.Column<int>(type: "integer", nullable: true),
                    Completed = table.Column<int>(type: "integer", nullable: true),
                    Watching = table.Column<int>(type: "integer", nullable: true),
                    Dropped = table.Column<int>(type: "integer", nullable: true),
                    OnHold = table.Column<int>(type: "integer", nullable: true),
                    Href = table.Column<string>(type: "text", nullable: true),
                    NextEpisode = table.Column<string>(type: "text", nullable: true),
                    IdFromAnimeGo = table.Column<int>(type: "integer", nullable: true),
                    Duration = table.Column<string>(type: "text", nullable: true),
                    TypeId = table.Column<int>(type: "integer", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: true),
                    MpaaRateId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.AnimeId);
                    table.ForeignKey(
                        name: "FK_Animes_MpaaRates_MpaaRateId",
                        column: x => x.MpaaRateId,
                        principalTable: "MpaaRates",
                        principalColumn: "MpaaRateId");
                    table.ForeignKey(
                        name: "FK_Animes_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "StatusId");
                    table.ForeignKey(
                        name: "FK_Animes_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeAnimeId");
                });

            migrationBuilder.CreateTable(
                name: "AnimeDubbing",
                columns: table => new
                {
                    AnimesAnimeId = table.Column<int>(type: "integer", nullable: false),
                    DubbingId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeDubbing", x => new { x.AnimesAnimeId, x.DubbingId });
                    table.ForeignKey(
                        name: "FK_AnimeDubbing_Animes_AnimesAnimeId",
                        column: x => x.AnimesAnimeId,
                        principalTable: "Animes",
                        principalColumn: "AnimeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeDubbing_Dubbing_DubbingId",
                        column: x => x.DubbingId,
                        principalTable: "Dubbing",
                        principalColumn: "DubbingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeGenre",
                columns: table => new
                {
                    AnimesAnimeId = table.Column<int>(type: "integer", nullable: false),
                    GenresGenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeGenre", x => new { x.AnimesAnimeId, x.GenresGenreId });
                    table.ForeignKey(
                        name: "FK_AnimeGenre_Animes_AnimesAnimeId",
                        column: x => x.AnimesAnimeId,
                        principalTable: "Animes",
                        principalColumn: "AnimeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeGenre_Genres_GenresGenreId",
                        column: x => x.GenresGenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeStudio",
                columns: table => new
                {
                    AnimesAnimeId = table.Column<int>(type: "integer", nullable: false),
                    StudiosStudioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeStudio", x => new { x.AnimesAnimeId, x.StudiosStudioId });
                    table.ForeignKey(
                        name: "FK_AnimeStudio_Animes_AnimesAnimeId",
                        column: x => x.AnimesAnimeId,
                        principalTable: "Animes",
                        principalColumn: "AnimeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeStudio_Studios_StudiosStudioId",
                        column: x => x.StudiosStudioId,
                        principalTable: "Studios",
                        principalColumn: "StudioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeDubbing_DubbingId",
                table: "AnimeDubbing",
                column: "DubbingId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeGenre_GenresGenreId",
                table: "AnimeGenre",
                column: "GenresGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_IdFromAnimeGo",
                table: "Animes",
                column: "IdFromAnimeGo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animes_MpaaRateId",
                table: "Animes",
                column: "MpaaRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_StatusId",
                table: "Animes",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_TypeId",
                table: "Animes",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeStudio_StudiosStudioId",
                table: "AnimeStudio",
                column: "StudiosStudioId");

            migrationBuilder.CreateIndex(
                name: "IX_Dubbing_Title",
                table: "Dubbing",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Title",
                table: "Genres",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MpaaRates_Title",
                table: "MpaaRates",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Title",
                table: "Statuses",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Studios_Title",
                table: "Studios",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Types_Title",
                table: "Types",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeDubbing");

            migrationBuilder.DropTable(
                name: "AnimeGenre");

            migrationBuilder.DropTable(
                name: "AnimeStudio");

            migrationBuilder.DropTable(
                name: "Dubbing");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Animes");

            migrationBuilder.DropTable(
                name: "Studios");

            migrationBuilder.DropTable(
                name: "MpaaRates");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
