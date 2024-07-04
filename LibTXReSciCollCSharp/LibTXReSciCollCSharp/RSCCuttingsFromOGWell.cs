using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTXReSciCollCSharp
{
    /// <summary>
    /// Represents a sampling of cuttings out of an oil or gas well
    /// </summary>
    public class RSCCuttingsFromOGWell : RSCSampleFromOGWell
    {
        public RSCCuttingsFromOGWell() {
            DataType = "Rock cuttings";
        }
    }
}
