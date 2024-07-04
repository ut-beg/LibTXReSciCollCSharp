using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTXReSciCollCSharp
{
    public class RSCCorePhoto : RSCSampleFromOGWell
    {
        public RSCCorePhoto()
        {
            DataType = "Core Photos";
        }

        public int FreeScanCount { get; set; }
        public int PaidScanCount { get; set; }

        protected override List<string> ComposeAbstractElements()
        {
            List<string> elements = base.ComposeAbstractElements();

            if (FreeScanCount != null)
            {
                AddAbstractElement(FreeScanCount.ToString(), "Free Scans Available", elements);
            }

            if (PaidScanCount != null)
            {
                AddAbstractElement(PaidScanCount.ToString(), "Paid Scans Available", elements);
            }


            return elements;
        }
    }
}
