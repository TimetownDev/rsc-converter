using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rscconventer.JavaGenerator.Slimefun;

public static class RadioactiveClass
{
    public static ClassDefinition Class { get; }
    static RadioactiveClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "Radioactive")
        {
            NeedGenerate = false
        };
    }
}
