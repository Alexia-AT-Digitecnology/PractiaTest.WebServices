using Microsoft.Extensions.Logging;
using PractiaTest.Database.Entities;
using PractiaTest.WebServices.Services.Interfaces;

namespace PractiaTest.WebServices.Services.Implementations
{
    /// <summary>
    /// Database utility class
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        private readonly ILoggerFactory _loggerFactory;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory">The logger factory to use</param>
        public DatabaseService(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        
        /// <summary>
        /// Gets a new DbContext instance
        /// </summary>
        /// <returns>A new DbContext instance</returns>
        public ApplicationDbContext GetNewContextInstance()
        {
#if DEBUG
            return new ApplicationDbContext(_loggerFactory);
#else
            return new ApplicationDbContext();
#endif
        }
    }
}