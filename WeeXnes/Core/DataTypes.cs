using System;

namespace WeeXnes.Core
{
    public class UpdateVar<T>
    {
        private T _value;

        public Action ValueChanged;

        public T Value
        {
            get => _value;

            set
            {
                _value = value;
                OnValueChanged();
            }
        }

        protected virtual void OnValueChanged() => ValueChanged?.Invoke();
    }
}