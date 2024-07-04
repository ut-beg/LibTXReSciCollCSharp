using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTXReSciCollCSharp
{
    public class RSCHandSpecimen : RSCSampleFromOGWell
    {
        public string? FormationName { get; set; }

        public string? FormationAge { get; set; }

        public RSCHandSpecimen()
        {
            DataType = "Hand Specimen";
        }

        protected override List<string> ComposeAbstractElements()
        {
            List<string> elements = base.ComposeAbstractElements();

            if (FormationName != null)
            {
                AddAbstractElement(FormationName, "Formation Name", elements);
            }

            if (FormationAge != null)
            {
                AddAbstractElement(FormationAge, "Formation Age", elements);
            }

            return elements;
        }
    }
}
