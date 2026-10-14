// Name: Joaquin Wells
// Email: jwells32@student.cnm.edu
// File: DBManager.cs
//
// NOTE: This project uses Microsoft.Data.Sqlite instead of System.Data.SqlClient
// because LocalDB (.mdf) is Windows-only. SQLite is fully cross-platform and
// fulfills the same objectives (parameterized queries, SqlDataReader
// equivalent via SqliteDataReader, CRUD operations).

using System.ComponentModel;
using Microsoft.Data.Sqlite;

namespace WellsP9
{
    /// <summary>
    /// Manages all database operations for the Flash Card application.
    /// Provides methods to create the schema, read, add, update, and remove cards.
    /// Uses parameterized queries to prevent SQL injection.
    /// </summary>
    public class DBManager
    {
        #region Fields

        /// <summary>
        /// SQLite connection string retrieved from AppSettings.
        /// </summary>
        private readonly string _connectionString;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor — reads the connection string from AppSettings and ensures
        /// the database schema exists.
        /// </summary>
        public DBManager()
        {
            _connectionString = AppSettings.ConnectionString;
            EnsureDatabase();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the Card table if it does not already exist.
        /// Called once at startup to guarantee the schema is in place.
        /// </summary>
        public void EnsureDatabase()
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = @"CREATE TABLE IF NOT EXISTS Card (
                CardID   INTEGER PRIMARY KEY AUTOINCREMENT,
                Title    TEXT    NOT NULL,
                Question TEXT    NOT NULL,
                Answer   TEXT    NOT NULL,
                NumRight INTEGER NOT NULL DEFAULT 0,
                NumWrong INTEGER NOT NULL DEFAULT 0
            );";

            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Reads all rows from the Card table using a SqliteDataReader and
        /// repopulates the provided BindingList with fresh Card objects.
        /// Equivalent to: SELECT * FROM Card
        /// </summary>
        /// <param name="cards">The BindingList to clear and repopulate.</param>
        public void GetCards(BindingList<Card> cards)
        {
            cards.Clear();

            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM Card";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            using SqliteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cards.Add(new Card(
                    reader.GetInt32(reader.GetOrdinal("CardID")),
                    reader.GetString(reader.GetOrdinal("Title")),
                    reader.GetString(reader.GetOrdinal("Question")),
                    reader.GetString(reader.GetOrdinal("Answer")),
                    reader.GetInt32(reader.GetOrdinal("NumRight")),
                    reader.GetInt32(reader.GetOrdinal("NumWrong"))
                ));
            }
        }

        /// <summary>
        /// Inserts a new card row into the database.
        /// Equivalent to: INSERT INTO Card (Title, Question, Answer, NumRight, NumWrong) VALUES(...)
        /// </summary>
        /// <param name="card">The Card object containing the values to insert.</param>
        public void AddCard(Card card)
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = "INSERT INTO Card (Title, Question, Answer, NumRight, NumWrong) " +
                         "VALUES (@Title, @Question, @Answer, @NumRight, @NumWrong);";

            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title",    card.Title);
            cmd.Parameters.AddWithValue("@Question", card.Question);
            cmd.Parameters.AddWithValue("@Answer",   card.Answer);
            cmd.Parameters.AddWithValue("@NumRight", card.NumRight);
            cmd.Parameters.AddWithValue("@NumWrong", card.NumWrong);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes a card row from the database by its CardID.
        /// Equivalent to: DELETE FROM Card WHERE CardID = @CardID
        /// </summary>
        /// <param name="card">The Card to remove (uses CardID).</param>
        public void RemoveCard(Card card)
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = "DELETE FROM Card WHERE CardID = @CardID;";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CardID", card.CardID);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates all fields of an existing card row in the database.
        /// Equivalent to: UPDATE Card SET Title=... WHERE CardID = @CardID
        /// </summary>
        /// <param name="card">The Card with updated values (CardID identifies the row).</param>
        public void Update(Card card)
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE Card SET " +
                         "Title = @Title, Question = @Question, Answer = @Answer, " +
                         "NumRight = @NumRight, NumWrong = @NumWrong " +
                         "WHERE CardID = @CardID;";

            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title",    card.Title);
            cmd.Parameters.AddWithValue("@Question", card.Question);
            cmd.Parameters.AddWithValue("@Answer",   card.Answer);
            cmd.Parameters.AddWithValue("@NumRight", card.NumRight);
            cmd.Parameters.AddWithValue("@NumWrong", card.NumWrong);
            cmd.Parameters.AddWithValue("@CardID",   card.CardID);
            cmd.ExecuteNonQuery();
        }

        #endregion
    }
}
