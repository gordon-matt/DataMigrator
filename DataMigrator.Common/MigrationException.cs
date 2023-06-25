using System.Runtime.Serialization;

namespace DataMigrator.Common
{
    public class MigrationException : ApplicationException
    {
        public MigrationException()
        {
        }

        public MigrationException(string message) : base(message)
        {
        }

        public MigrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MigrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}