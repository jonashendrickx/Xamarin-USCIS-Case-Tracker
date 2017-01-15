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
        private string _receiptNumber;
        private string _status;
        private string _description;

        private DateTime _modifiedDate;
        private DateTime _lastReadDate;
        private DateTime _lastSyncedDate;

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
                return _receiptNumber;
            }
            set
            {
                _receiptNumber = value;
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

        /// <summary>
        /// Case last modified during synchronization or by user. Or for future modifications (for example renames)
        /// </summary>
        public DateTime LastModifiedDate
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

        /// <summary>
        /// Timestamp when the user has last opened the case.
        /// </summary>
        public DateTime LastReadDate
        {
            get
            {
                return _lastReadDate;
            }
            set
            {
                _lastReadDate = value;
            }
        }

        /// <summary>
        /// Every time we check for updates, this timestamp is updated.
        /// </summary>
        public DateTime LastSyncedDate
        {
            get
            {
                return _lastSyncedDate;
            }
            set
            {
                _lastSyncedDate = value;
            }
        }
    }
}
