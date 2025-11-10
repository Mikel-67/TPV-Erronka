using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erronka1.Modeloak
{
    public class Userrak
    {
        public int Id { get; set; }
        public string Izena { get; set; }
        public string Pasahitza { get; set; }
        public bool Admin { get; set; }

        public Userrak(int id, string izena, string pasahitza, bool admin)
        {
            Id = id;
            Izena = izena;
            Pasahitza = pasahitza;
            Admin = admin;
        }

        public override string ToString()
        {
            return $"{Izena} ({(Admin ? "Admin" : "Erabiltzailea")})";
        }
    }
}
