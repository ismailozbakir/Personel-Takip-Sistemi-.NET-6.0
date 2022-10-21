using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelKayit
{
    internal class PersonelTablo
    {
        public short Id { get; set; }
        public object Ad { get; set; }
        public object Soyad { get; set; }
        public object Sehir { get; set; }
        public short Maas { get; set; }
        public string Durum { get; set; }
        public object Meslek { get; set; }
    }
}
