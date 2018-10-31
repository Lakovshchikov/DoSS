using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doss.Model
{
    class DatePoint
    {
        public int _x { get; set; }
        public int _y { get; set; }
        public int _z { get; set; }
        public int _value { get; set; }
        public DatePoint(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        public DatePoint(int x, int y, int z, int value)
        {
            _x = x;
            _y = y;
            _z = z;
            _value = value;
        }
        public DatePoint()
        {

        }
    }
}
