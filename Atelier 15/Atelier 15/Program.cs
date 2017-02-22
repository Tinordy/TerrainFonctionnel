using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AtelierXNA
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Atelier game = new Atelier())
            {
                game.Run();
            }
        }
    }
}
