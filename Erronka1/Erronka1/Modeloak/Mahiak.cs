namespace Erronka1.Modeloak
{
    public class Mahaiak
    {
        public int Id { get; set; }
        public string Zenbakia { get; set; }
        public bool Okupatuta { get; set; }

        public Mahaiak(int id, string zenbakia, bool okupatuta)
        {
            Id = id;
            Zenbakia = zenbakia;
            Okupatuta = okupatuta;
        }

        public override string ToString()
        {
            return $"{Zenbakia} ({Okupatuta})";
        }
    }
}
