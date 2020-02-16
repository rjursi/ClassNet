using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookerMaker
{
    class Program
    {
        
        static void Main(string[] args)
        {

            Hooker hooker = new Hooker();


            while(true)
                hooker.SetHook();
            

            //Console.ReadKey();

        }
    }
}
