using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;


namespace LibTXReSciCollCSharp {

    /// <summary>
    /// RSCItems represents the collection object, which can in turn contain a series of RSCItem objects, each of which represents an item in the collection.
    /// </summary>
    public class RSCItems
    {
        private List<RSCItem> _samples;

        /// <summary>
        /// Constructor
        /// </summary>
        public RSCItems()
        {
            _samples = new List<RSCItem>();
        }

        /// <summary>
        /// The set of samples in the collection
        /// </summary>
        public List<RSCItem> Samples
        {
            get { return _samples; }
        }

        /// <summary>
        /// Adds the given item to the collection
        /// </summary>
        /// <param name="sample">Sample to be added to the collection</param>
        public void AddItem(RSCItem sample)
        {
            if (!_samples.Contains(sample))
            {
                _samples.Add(sample);
            }
        }

        /// <summary>
        /// Removes the given item from the collection
        /// </summary>
        /// <param name="sample"></param>
        public void RemoveItem(RSCItem sample)
        {
            if (_samples.Contains(sample))
            {
                _samples.Remove(sample);
            }
        }

        /// <summary>
        /// Returns an array of strings, each of which represents a line in the output csv file for a given item.
        /// </summary>
        /// <returns>Array of csv strings, one for each item in the collection</returns>
        public List<string[]> ToRSCCsvArray()
        {
            List<string[]> ret = new List<string[]>();

            foreach (RSCItem sample in _samples)
            {
                string[] sampX = sample.ToRSCCsvRowArray();
                ret.Add(sampX);
            }

            return ret;
        }

        /// <summary>
        /// Writes the contents of the collection out to a ReSciColl-compatible item-level metadata CSV file
        /// </summary>
        /// <param name="outFilePath">The path to write the file.</param>
        public void ToRSCCsvFile(string outFilePath)
        {
            List<string[]> rows = ToRSCCsvArray();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = (field) =>
                {
                    return true;
                }
            };

            using (StreamWriter fileWriter = new StreamWriter(outFilePath, false, Encoding.UTF8))
            using (CsvWriter csvWriter = new CsvWriter(fileWriter, config))
            {
                // Handle the header line
                string[] headers = GetHeaders();
                csvWriter.WriteHeader<RSCItem>();
                csvWriter.NextRecord();

                //Write the business data lines
                csvWriter.WriteRecords(Samples);
            }
        }

        /// <summary>
        /// Returns an array of strings, one for each field in the ReSciColl CSV schema
        /// </summary>
        /// <returns>Array of strings, one for each column in the ReSciColl CSV schema</returns>
        public string[] GetHeaders()
        {
            string[] ret = {
                "localID",
                "title",
                "alternateTitle",
                "abstract",
                "coordinateLon",
                "coordinateLat",
                "publicationDate",
                "alternateGeometry",
                "onlineResource",
                "browseGraphic",
                "date",
                "verticalExtent",
                "IGSN",
                "parentIGSN",
                "relIGSN",
                "relationType",
                "largerWorkCitation"
            };

            return ret;
        }
    }
}