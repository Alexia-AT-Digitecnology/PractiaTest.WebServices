using PractiaTest.Database.Entities;

namespace PractiaTest.WebServices.Services.Interfaces
{
    /// <summary>
    /// Database service class
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Gets a new DbContext instance
        /// </summary>
        /// <returns>A new DbContext instance</returns>
        ApplicationDbContext GetNewContextInstance();
    }
}