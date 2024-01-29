using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants.Errors
{
    public class CustomerErrorsContant
    {
        public static readonly string CUSTOMERNOTFOUND = SharedError.FAIL + "Customer Not Found!";
        public static readonly string CUSROMEREXIST = SharedError.FAIL + "This customer already exist!";

    }
}
