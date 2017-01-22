using SQLite;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using USCISCaseTracker.Common;

namespace USCISCaseTracker.Models
{
    public class Case
    {
        public Case()
        {

        }

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get;
            set;
        }

        public string Name { get; set; }

        public string ReceiptNumber { get; set; }

        public string FormType { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Case last modified during synchronization or by user. Or for future modifications (for example renames)
        /// </summary>
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// Timestamp when the user has last opened the case.
        /// </summary>
        public DateTime LastReadDate { get; set; }

        /// <summary>
        /// Every time we check for updates, this timestamp is updated.
        /// </summary>
        public DateTime LastSyncedDate { get; set; }

        public async Task Update()
        {
            var httpClient = new HttpClient();
            var uri = new Uri(string.Format(Constants.GetCaseStatusUrl, ReceiptNumber));
            string html = null;
            try
            {
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    html = await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException)
            {
                return;
            }

            if (html != null)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                var appointmentNode = htmlDocument.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"].Value.Contains("appointment-sec"));
                var contentNode = appointmentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"].Value == "rows text-center");

                var isModified = false;

                var newStatus = contentNode.Descendants("h1").FirstOrDefault().InnerText;
                if (Status != null)
                {
                    if (!Status.Equals(newStatus))
                    {
                        Status = newStatus;
                        isModified = true;
                    }
                }
                else
                {
                    Status = newStatus;
                    isModified = true;
                }

                var newDescription = contentNode.Descendants("p").FirstOrDefault().InnerText;
                if (Description != null)
                {
                    if (!Description.Equals(newDescription))
                    {
                        Description = newDescription;
                        isModified = true;
                    }
                }
                else
                {
                    Description = newDescription;
                    isModified = true;
                }

                if (isModified)
                {
                    LastModifiedDate = DateTime.Now;
                }

                if (string.IsNullOrEmpty(FormType) && !string.IsNullOrEmpty(newDescription))
                {
                    var pieces = newDescription.Split(',');
                    string yourForm = "your Form ";
                    if (pieces[2].Contains(yourForm))
                    {
                        var index = pieces[2].IndexOf(yourForm, StringComparison.Ordinal);
                        FormType = pieces[2].Substring(index + yourForm.Length);
                    }
                }

                LastSyncedDate = DateTime.Now;
            }
        }
    }
}
