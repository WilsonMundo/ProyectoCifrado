namespace Cifrado.Models
{
    public class Encriptado
    {
        public long IdEncriptado { get; set; }
        public string NormalizedEmail { get; set; }=null!;
        public string TextEncript {  get; set; }=null!;
        public string KeyEncript { get; set; }=null!;
        public DateTime btFecha { get; set; }

    }
}
