using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeXnes.Keys
{
    public class KeyItem
    {
        public string name { get; set; }
        public string value { get; set; }
        public KeyItem(string _name, string _value)
        {
            this.name = _name;
            this.value = _value;
        }
    }
}
