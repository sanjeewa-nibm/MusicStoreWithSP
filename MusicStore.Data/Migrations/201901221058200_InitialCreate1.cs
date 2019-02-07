using System.Data.SqlClient;

namespace MusicStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
                    Sql(@"
                                CREATE PROCEDURE GetAllGenres
                    AS
                    BEGIN
	                         -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;

                   SELECT [GenreId]
                      ,[Name]
                      ,[Description]
                  FROM [dbo].[Genres]

                END
                ");

                    Sql(@"CREATE PROCEDURE GetGenresByID
                        @GenreId INT
                        AS
                        BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;

                       SELECT [GenreId]
                          ,[Name]
                          ,[Description]
                      FROM [dbo].[Genres]
                      WHERE [GenreId] = @GenreId
                    END
                    ");

                    Sql(@"CREATE PROCEDURE GetGenresByName
                        @GenreName NVARCHAR(100) 
                        AS
                        BEGIN
	                        -- SET NOCOUNT ON added to prevent extra result sets from
	                        -- interfering with SELECT statements.
	                        SET NOCOUNT ON;

                           SELECT [GenreId]
                              ,[Name]
                              ,[Description]
                          FROM [dbo].[Genres]
                          WHERE [Name] = @GenreName
                        END
                        ");
        }

        public override void Down()
        {
        }
    }
}
