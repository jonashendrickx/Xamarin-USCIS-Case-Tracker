using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using USCISCaseTracker.Common;
using USCISCaseTracker.Models;

namespace USCISCaseTracker.Services
{
    public class USCISService : IUSCISService
    {
        private HttpClient client;

        public USCISService()
        {
            client = new HttpClient();
        }

        public async Task<Case> GetCaseStatusAsync(string receipt_number)
        {
            var uri = new Uri(string.Format(Constants.GetCaseStatusUrl, receipt_number));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var html = await response.Content.ReadAsStringAsync();
                    return DecodeHtml(receipt_number, html);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"@\t{0}", ex.Message);
            }
            return null;
        }

        private Case DecodeHtml(string receipt_number, string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var appointmentNode = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"].Value.Contains("appointment-sec"));
            var contentNode = appointmentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"].Value == "rows text-center");

            var caseStatus = contentNode.Descendants("h1").FirstOrDefault().InnerText;
            var caseDescription = contentNode.Descendants("p").FirstOrDefault().InnerText;

            var uscisCase = new Case()
            {
                ReceiptNumber = receipt_number,
                Status = caseStatus,
                Description = caseDescription,
                LastSyncedDate = DateTime.Now
            };
            return uscisCase;
        }
    }

}
