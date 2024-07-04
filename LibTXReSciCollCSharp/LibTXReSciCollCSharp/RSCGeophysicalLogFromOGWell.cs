using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTXReSciCollCSharp
{
    /// <summary>
    /// Represents a geophysical log.  This is predicated on the Bureau of Economic Geology's particular arrangement of fields, but should hopefully capture most surveys' needs.  If not, it can be extended.
    /// </summary>
    public class RSCGeophysicalLogFromOGWell : RSCSampleFromOGWell
    {
        public string? LogTypes { get; set; }
        public string? LogScales { get; set; }

        public string? LogNumber { get; set; }

        public string? LogSource { get; set; }

        public string? MudWeightViscosity {  get; set; }
        public string? MudWeightDensity { get; set; }


        public double? MaxRecordedTemperature { get; set; }
        public double? MaxRecordedTemperature2 { get; set; }
        public double? MaxRecordedTemperature3 { get; set; }

        public double? CasingSize { get; set; }

        public double? BitSize { get; set; }

        public string? CirculationTime { get; set; }   
        
        public int? FreeScanCount { get; set; }

        public int? PaidScanCount { get; set; }

        public RSCGeophysicalLogFromOGWell() {
            DataType = "Well logs";
        }

        protected override List<string> ComposeAbstractElements()
        {
            List<string> elements = base.ComposeAbstractElements();

            if (LogTypes != null)
            {
                AddAbstractElement(LogTypes, "Log Types", elements);
            }

            if (LogScales != null)
            {
                AddAbstractElement(LogScales, "Log Scales", elements);
            }

            if (LogNumber != null)
            {
                AddAbstractElement(LogNumber, "Log Number", elements);
            }

            if (LogSource != null)
            {
                AddAbstractElement(LogSource, "Log Source", elements);
            }

            if (MudWeightViscosity != null)
            {
                AddAbstractElement(MudWeightViscosity.ToString(), "Mud Weight (Viscosity)", elements);
            }

            if (MudWeightDensity != null)
            {
                AddAbstractElement(MudWeightDensity.ToString(), "Mud Weight (Density)", elements);
            }

            if (MaxRecordedTemperature != null)
            {
                AddAbstractElement(MaxRecordedTemperature.ToString(), "Max. Recorded Temperature", elements);
            }

            if (MaxRecordedTemperature2 != null)
            {
                AddAbstractElement(MaxRecordedTemperature2.ToString(), "Max. Recorded Temperature 2", elements);
            }

            if (MaxRecordedTemperature3 != null)
            {
                AddAbstractElement(MaxRecordedTemperature3.ToString(), "Max. Recorded Temperature 3", elements);
            }

            if (CasingSize != null)
            {
                AddAbstractElement(CasingSize.ToString(), "Casing Size (in.)", elements);
            }

            if (BitSize != null)
            {
                AddAbstractElement(BitSize.ToString(), "Bit Size (in.)", elements);
            }

            if (CirculationTime != null)
            {
                AddAbstractElement(CirculationTime, "Circulation Time", elements);
            }

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
