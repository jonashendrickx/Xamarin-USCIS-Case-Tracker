using Prism.Windows.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using USCISCaseTracker.Models;

namespace USCISCaseTracker.UWP.ViewModels.CaseViewModels
{
    public class CaseAddViewModel : ValidatableBindableBase

    {
        private string _name;
        private string _receiptNumber;

        public CaseAddViewModel()
        {

        }

        [Required]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        [RegularExpression(@"^[A-Z]{3}[0-9]{10}$", ErrorMessage = "Receipt number must be in format 'ABC0123456789'")]
        [Required]
        public string ReceiptNumber
        {
            get
            {
                return _receiptNumber;
            }
            set
            {
                SetProperty(ref _receiptNumber, value, "ReceiptNumber");
            }
        }
    }
}
