using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace XrmVision.Extensions.Extensions
{
    public static class FetchHelper
    {
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, string fetchxmlstring,
            int pagesize = 1000)
        {
            var fetchedrecords = new EntityCollection();

            // Initialize the page number.
            var pageNumber = 1;

            // Specify the current paging cookie. For retrieving the first page, 
            // pagingCookie should be null.
            string pagingCookie = null;

            //execute query and page through results
            while (true)
            {
                // Build fetchXml string with the placeholders.
                var fetchXml = CreateXml(fetchxmlstring, pagingCookie, pageNumber, pagesize);

                var retrieved = service.RetrieveMultiple(new FetchExpression(fetchXml));

                //add retrieved records to our main entitycollection
                fetchedrecords.Entities.AddRange(retrieved.Entities);

                if (retrieved.MoreRecords)
                {
                    // Increment the page number to retrieve the next page.
                    pageNumber++;

                    // Set the paging cookie to the paging cookie returned from current results.                            
                    pagingCookie = retrieved.PagingCookie;
                }
                else
                {
                    // If no more records in the result nodes, exit the loop.
                    break;
                }
            }

            return fetchedrecords;

        }

        /// <summary>
        /// used to enable paging in the fetchxml queries - https://msdn.microsoft.com/en-us/library/gg328046.aspx
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="cookie"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string CreateXml(string xml, string cookie, int page, int count)
        {
            var stringReader = new StringReader(xml);
            var reader = new XmlTextReader(stringReader);

            // Load document
            var doc = new XmlDocument();
            doc.Load(reader);

            return CreateXml(doc, cookie, page, count);
        }

        /// <summary>
        /// used to enable paging in the fetchxml queries - https://msdn.microsoft.com/en-us/library/gg328046.aspx
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="cookie"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string CreateXml(XmlDocument doc, string cookie, int page, int count)
        {
            var attrs = doc.DocumentElement.Attributes;

            if (cookie != null)
            {
                var pagingAttr = doc.CreateAttribute("paging-cookie");
                pagingAttr.Value = cookie;
                attrs.Append(pagingAttr);
            }

            var pageAttr = doc.CreateAttribute("page");
            pageAttr.Value = Convert.ToString(page);
            attrs.Append(pageAttr);

            var countAttr = doc.CreateAttribute("count");
            countAttr.Value = Convert.ToString(count);
            attrs.Append(countAttr);

            var sb = new StringBuilder(1024);
            var stringWriter = new StringWriter(sb);

            var writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }
    }

}