using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erronka1.Modeloak
{
    public class Produktua
    {
        public int Id { get; set; }
        public string Izena { get; set; }
        public decimal Prezioa { get; set; }
        public int Stocka { get; set; }

        public Produktua(int id, string izena, decimal prezioa, int stocka)
        {
            Id = id;
            Izena = izena;
            Prezioa = prezioa;
            Stocka = stocka;
        }

        public override string ToString()
        {
            return $"{Izena} ({Prezioa}€ - {Stocka}u)";
        }
    }
}
