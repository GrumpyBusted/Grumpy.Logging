using System.Collections.Generic;

namespace Grumpy.Logging
{
    internal interface ITextExpression
    {
        string Evaluate(object[] objects, ref int index, ref ICollection<string> data);
    }
}