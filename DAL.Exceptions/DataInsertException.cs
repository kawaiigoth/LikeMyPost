using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class DataInsertException : Exception
    {
        public DataInsertException()
        {
        }

        public DataInsertException(string message)
        : base(message)
    {
        }

        public DataInsertException(string message, Exception inner)
        : base(message, inner)
    {
        }
    }
}
