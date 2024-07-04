using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTXReSciCollCSharp
{
    /// <summary>
    /// Represents a sample of some kind from an oil or gas well.  While not declared as an abstract class, this class is really meant to be extended.
    /// </summary>
    public class RSCSample : RSCItem
    {
        /// <summary>
        /// A description of the type of sample, such as a rock core, cuttings, thin section, etc.
        /// </summary>
        public string? SampleType { get; set; }

        public string? FormationName { get; set; }

        public string? FormationAge { get; set; }

        [Ignore]
        public string? LocationDescription { get; set; }

        [Ignore]
        public string? LocalityName { get; set; }

        /// <summary>
        /// The name of the state/province/other administrative division where the well is located.
        /// </summary>
        public string? StateName { get; set; }

        /// <summary>
        /// The name of the county/parish where the well is located.
        /// </summary>
        public string? CountyName { get; set; }

        /// <summary>
        /// The name of the country where the well is located.
        /// </summary>
        public string? CountryName { get; set; }

        /// <summary>
        /// The material the sample is made out of
        /// </summary>
        public string? Material { get; set; }

        /// <summary>
        /// The material the sample is made out of
        /// </summary>
        public string? MaterialDetail { get; set; }

        /// <summary>
        /// A description of the sample
        /// </summary>
        public string? SampleDescription { get; set; }

        protected override List<string> ComposeAbstractElements()
        {
            List<string> elements = base.ComposeAbstractElements();

            if (CountryName != null)
            {
                AddAbstractElement(CountryName, "Country Name", elements);
            }

            if (StateName != null)
            {
                AddAbstractElement(StateName, "State Name", elements);
            }

            if (CountyName != null)
            {
                AddAbstractElement(CountyName, "CountyName", elements);
            }

            if (LocalityName != null)
            {
                AddAbstractElement(LocalityName, "Locality Name", elements);
            }

            if (LocationDescription != null)
            {
                AddAbstractElement(LocationDescription, "Location Description", elements);
            }

            if (SampleType != null)
            {
                AddAbstractElement(SampleType, "Sample Type", elements);
            }

            if (FormationName != null)
            {
                AddAbstractElement(FormationName, "Formation Name", elements);
            }

            if (FormationAge != null)
            {
                AddAbstractElement(FormationAge, "Formation Age", elements);
            }

            if(CollectedBy != null)
            {
                AddAbstractElement(CollectedBy, "Collected by ", elements);
            }

            if(Material != null)
            {
                AddAbstractElement(Material, "Material", elements);
            }

            if (MaterialDetail != null)
            {
                AddAbstractElement(MaterialDetail, "Material Detail", elements);
            }

            if (SampleDescription != null)
            {
                AddAbstractElement(SampleDescription, "Sample Description", elements);
            }

            return elements;
        }
    }
}
