using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants.Errors
{
    public class OrderErrorsConstant
    {
        public static readonly string ORDERITEMSEMPTY =SharedError.FAIL + "Order should have one item inside at least!";
    }
}
