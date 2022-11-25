using System;

namespace WeeXnes.Views.KeyManager
{
    public class KeyItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Filename { get; set; }
        public KeyItem(string name, string value)
        {
            this.Name = name;
            this.Value = value;
            this.Filename = Guid.NewGuid().ToString() + ".wx";
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}