using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rain4.Models
{
    public class Izpit
    {
        public string imeIzpita;
        public string ime;
        public string priimek;
        public DateTime datum;

        public string toString()
        {
            return "Na izpit " + imeIzpita + " se je prijavil " + ime + " " + 
                priimek + " dne " + datum.ToString("dd.mm.yyyy") + " ob " + datum.ToString("HH:mm");
        }
    }
}
