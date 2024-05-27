using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rscconventer.JavaGenerator.GuguSlimefunLib.Items;

public static class GuguSlimefunItemClass
{
    public static ClassDefinition Class { get; }
    static GuguSlimefunItemClass()
    {
        Class = new ClassDefinition("me.ddggdd135.guguslimefunlib.items", "GuguSlimefunItem")
        {
            NeedGenerate = false
        };
    }
}
