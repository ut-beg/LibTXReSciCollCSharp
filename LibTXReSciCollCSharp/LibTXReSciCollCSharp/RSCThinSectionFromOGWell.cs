using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTXReSciCollCSharp
{
    /// <summary>
    /// Represents a thin section, most likely out of a core, from an oil or gas well.
    /// </summary>
    public class RSCThinSectionFromOGWell : RSCSampleFromOGWell
    {
        public RSCThinSectionFromOGWell()
        {
            DataType = "Thin Sections";
        }
    }
}
