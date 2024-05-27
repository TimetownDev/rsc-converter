using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rscconventer.JavaGenerator.Slimefun;

public static class SoulboundClass
{
    public static ClassDefinition Class { get; }
    static SoulboundClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "Soulbound")
        {
            NeedGenerate = false,
            IsInterface = true
        };
    }
}
