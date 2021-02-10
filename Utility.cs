using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Utility
{
    static public int[] Enum2IntArray(TetrisEnvironment.MinoKind[] enums)
    {
        int[] returnValue = new int[enums.Length];

        for (int i = 0; i < enums.Length; i++)
        {
            returnValue[i] = (int)enums[i];
        }

        return returnValue;
    }
}
