using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTXReSciCollCSharp
{
    /// <summary>
    /// Represents a rock core that came out of an oil and gas well.
    /// </summary>
    public class RSCRockCoreFromOGWell : RSCSampleFromOGWell
    {


        public RSCRockCoreFromOGWell() 
        {
            DataType = "Rock cores";
        }

        protected override List<string> ComposeAbstractElements()
        {
            List<string> elements = base.ComposeAbstractElements();

            if(FormationName != null)
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
