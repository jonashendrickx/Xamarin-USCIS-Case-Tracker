using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USCISCaseTracker.Models;

namespace USCISCaseTracker.Services
{
    public interface IUSCISService
    {
        Task<Case> GetCaseStatusAsync(string receipt_number);
    }
}
