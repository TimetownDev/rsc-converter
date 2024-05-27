using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rscconventer.JavaGenerator.Slimefun;

public static class SlimefunItemStackClass
{
    public static ClassDefinition Class { get; }
    static SlimefunItemStackClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.api.items", "SlimefunItemStack")
        {
            NeedGenerate = false
        };
    }
}
