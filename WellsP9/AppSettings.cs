// Name: Joaquin Wells
// Email: jwells32@student.cnm.edu
// File: AppSettings.cs

namespace WellsP9
{
    /// <summary>
    /// Provides application-wide configuration values.
    /// Replaces App.config / ConfigurationManager for MAUI on Mac Catalyst.
    /// </summary>
    public static class AppSettings
    {
        #region Properties

        /// <summary>
        /// SQLite connection string pointing to the app's local data directory.
        /// Uses FileSystem.AppDataDirectory so the database persists across launches.
        /// </summary>
        public static string ConnectionString =>
            $"Data Source={Path.Combine(FileSystem.AppDataDirectory, "FlashCards.db")}";

        #endregion
    }
}
