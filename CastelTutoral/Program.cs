using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
namespace CastelTutoral
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Hello World!");

            var container = new WindsorContainer();
            container.Register(Component.For<SayCeo>());
            container.Register(Component.For<ISay1>().ImplementedBy<Say1>());
            container.Register(Component.For<ISay2>().ImplementedBy<Say2>());

            var mainThing = container.Resolve<SayCeo>();
            mainThing.DoSomething();
            Console.ReadLine();
        }
    }
}
