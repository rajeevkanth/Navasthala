using DataLayer.Models;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace Navasthala.BootStrapper
{
    public class BootStrapper
    {
        public static void ConfigureDependencies()
        {
            ObjectFactory.Initialize(x => x.AddRegistry<ControllerRegistry>());
        }
    }

    public class ControllerRegistry : Registry
    {
        public ControllerRegistry()
        {
            For<NavasthalaContext>().LifecycleIs(new SingletonLifecycle()).Use(() => new NavasthalaContext());
        }
    }
}