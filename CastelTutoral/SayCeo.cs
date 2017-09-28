using System;
using System.Collections.Generic;
using System.Text;

namespace CastelTutoral
{
    public class SayCeo
    {
        private ISay1 _say1;
        private ISay2 _say2;

        public SayCeo(ISay1 say1, ISay2 say2)
        {
            _say1 = say1;
            _say2 = say2;
        }

        public void DoSomething()
        {
            _say1.SomeSay1 = "good job;";
            
            _say2.SomeSay2 = "Fuck";

            Console.WriteLine(_say1.SomeSay1);
            Console.WriteLine(_say2.SomeSay2);
        }
    }
}
