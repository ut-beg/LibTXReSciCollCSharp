using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTXReSciCollCSharp
{
    public class RSCGISDataset : RSCItem
    {
        public RSCGISDataset() {
            DataType = "GIS Dataset";
        }

        public string? Description
        {
            get; set;
        }

        protected override List<string> ComposeAbstractElements()
        {
            List<string> elements = base.ComposeAbstractElements();

            if (Description != null)
            {
                AddAbstractElement(Description, "Description", elements);
            }
           
            return elements;
        }
    }
}
