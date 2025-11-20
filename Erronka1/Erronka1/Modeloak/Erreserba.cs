using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erronka1.Modeloak
{
    public class Erreserba
    {
        public int Id { get; set; }
        public Mahaiak Mahaia { get; set; }
        public DateTime Data { get; set; }

        public Erreserba(int id, Mahaiak mahaia, DateTime data)
        {
            Id = id;
            Mahaia = mahaia;
            Data = data;
        }
    }
}

    
