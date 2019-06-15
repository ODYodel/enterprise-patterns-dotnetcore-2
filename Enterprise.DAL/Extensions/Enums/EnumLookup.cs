using System;
using System.Collections.Generic;
using System.Text;

namespace Enterprise.DAL.Extensions.Enums
{
    public class Lookup<T>
        where T : Enum
    {
        public Lookup()
        {
        }
        public Lookup(T value)
        {
            Id = Convert.ToInt32(value);
            Value = value;
            Name = value.ToString();
        }

        public int Id { get; set; }
        public T Value { get; set; }
        public string Name { get; set; }
    }
}
