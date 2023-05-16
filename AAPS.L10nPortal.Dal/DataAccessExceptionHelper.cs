using AAPS.L10nPortal.Dal.Exceptions;
using System.Data.SqlClient;

namespace AAPS.L10nPortal.Dal
{
    public static class DataAccessExceptionHelper
    {
        /// <summary> Reflection version </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static Exception Create(SqlException sqlException)
        {
            Exception exception;
            try
            {
                var dalExceptionType = typeof(UndefinedException);
                var exceptionClassName = $"{dalExceptionType.Namespace}.{sqlException.Message}";
                exception = Activator.CreateInstance(dalExceptionType.Assembly.GetName().Name, exceptionClassName).Unwrap() as Exception;
            }
            catch
            {
                exception = new UndefinedException("Undefined SQL Exception", sqlException);
            }

            return exception;
        }
    }
}
