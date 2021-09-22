using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET_Projekt_2.Models
{
    public abstract class ApiForm
    {
        public string APIKEY { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
