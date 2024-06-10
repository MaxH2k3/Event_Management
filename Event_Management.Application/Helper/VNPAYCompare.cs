using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Helper
{
    public class VNPAYCompare : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null && y == null)
                return 0;
            else if (x == null)
                return -1;
            else if (y == null)
                return 1;
            else
                return string.Compare(x, y, StringComparison.Ordinal);
        }
    }
}
