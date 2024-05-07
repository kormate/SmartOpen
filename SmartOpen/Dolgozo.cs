using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOpen
{
    internal class Dolgozo
    {
        internal string jelszo;
        private string nev;

        public string Nev
        {
            get 
            {
                return nev; 
            }
            set
            {
                nev = value;
            }
        }

        public int dolgozoID { get; set; }
        public string email { get; set; }
        public string beosztas { get; internal set; }
    }
}
