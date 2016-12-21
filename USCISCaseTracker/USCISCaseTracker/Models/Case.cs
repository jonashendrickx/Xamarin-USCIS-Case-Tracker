using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USCISCaseTracker.Models
{
    public class Case
    {
        private string _name;
        private string _receipt_number;
        private string _status;
        private string _description;

        private DateTime _createdDate;
        private DateTime _modifiedDate;
        private DateTime _lastUpdatedDate;

        public Case()
        {

        }

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string ReceiptNumber
        {
            get
            {
                return _receipt_number;
            }
            set
            {
                _receipt_number = value;
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                return _createdDate;
            }
            set
            {
                _createdDate = value;
            }
        }

        public DateTime ModifiedDate
        {
            get
            {
                return _modifiedDate;
            }
            set
            {
                _modifiedDate = value;
            }
        }

        public DateTime LastSyncedDate
        {
            get
            {
                return _lastUpdatedDate;
            }
            set
            {
                _lastUpdatedDate = value;
            }
        }
    }
}
