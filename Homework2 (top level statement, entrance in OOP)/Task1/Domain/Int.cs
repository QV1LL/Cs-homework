using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Domain
{
    internal class Int : IntDecorator
    {
        public Int(int value) : base(value.ToString())
        {

        }

        public Int(string value) : base(value)
        {
            
        }

        public override int ToInt() => Convert.ToInt32(this.Value);
    }
}
