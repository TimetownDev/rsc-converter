using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rscconventer.JavaGenerator.GuguSlimefunLib.Items;

public static class RainbowTypeClass
{
    public static ClassDefinition Class { get; }
    static RainbowTypeClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.items", "RainbowType")
        {
            NeedGenerate = false
        };
    }
}
