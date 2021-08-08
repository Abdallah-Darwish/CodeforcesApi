using Dapper;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace CodeforcesApi.Core.Managers
{
    public static class DatabaseManager
    {
        public const string DatabaseFileName = "codeforces.sqlite";
        public static readonly string DatabaseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, DatabaseFileName);
        public static IDbConnection GetConnection() => new SQLiteConnection("Data Source=" + DatabaseFilePath + ";Version=3;");
        private static async ValueTask CreateDatabase()
        {
            if (File.Exists(DatabaseFilePath)) { File.Delete(DatabaseFilePath); }
            SQLiteConnection.CreateFile(DatabaseFilePath);
            using var con = GetConnection();
            await con.ExecuteAsync(@"
CREATE TABLE Contest (
	Id	INTEGER NOT NULL,
	Name TEXT NOT NULL,
	PRIMARY KEY(Id)
);
CREATE TABLE Problem (
	Id	TEXT NOT NULL,
	ContestId INTEGER,
	Name	TEXT NOT NULL,
	[Index]	TEXT NOT NULL,
    Rating INT,
    PRIMARY KEY(Id),
    FOREIGN KEY(ContestId) REFERENCES Contest(Id)
);
CREATE TABLE Submission (
	Id INTEGER NOT NULL,
	AuthorHandle TEXT NOT NULL,
	Verdict	INTEGER,
	ProblemId TEXT NOT NULL,
	PRIMARY KEY(Id),
    FOREIGN KEY(ProblemId) REFERENCES Problem(Id)
);
CREATE TABLE ProblemInfo (
    ProblemId TEXT NOT NULL,
    [Index] TEXT NOT NULL,
    [Name] TEXT NOT NULL,
    Rating INTEGER,
    PRIMARY KEY(ProblemId),
    FOREIGN KEY(ProblemId) REFERENCES Problem(Id)
);
").ConfigureAwait(false);
        }
        public static async ValueTask ValidateDatabase()
        {
            bool shouldCreateDb = false;
            if (File.Exists(DatabaseFilePath) == false) { shouldCreateDb = true; }
            else
            {
                //Validate it!
                try
                {
                    using var con = GetConnection();
                    await con.QueryAsync("SELECT id FROM Contest WHERE id = 0").ConfigureAwait(false);
                    await con.QueryAsync("SELECT id FROM Problem WHERE id = 0").ConfigureAwait(false);
                    await con.QueryAsync("SELECT id FROM Submission WHERE id = 0").ConfigureAwait(false);
                    await con.QueryAsync("SELECT problemId FROM ProblemInfo WHERE problemId = 'NONE'").ConfigureAwait(false);
                }
                catch
                {
                    shouldCreateDb = true;
                }
            }
            if (shouldCreateDb) { await CreateDatabase().ConfigureAwait(false); }
        }
    }
}