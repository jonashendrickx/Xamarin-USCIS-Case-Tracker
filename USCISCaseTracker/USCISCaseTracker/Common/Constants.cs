using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USCISCaseTracker.Common
{
    public static class Constants
    {
        public const string BaseUrl = "https://egov.uscis.gov/";
        public const string GetCaseStatusUrl = BaseUrl + "casestatus/mycasestatus.do?appReceiptNum={0}";
    }
}
