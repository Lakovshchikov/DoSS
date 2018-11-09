using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doss.Model
{
    [Serializable]
    public class MyViewPoint
    {
        public double Scale { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public bool CadMapEnab { get; set; }
        public bool StreetMapEnab { get; set; }
        public bool SpaceMapEnab { get; set; }
        public int Wkid { get; set; }

        public MyViewPoint()
        {

        }

        public MyViewPoint(double _Scale, double _X, double _Y,int _Wkid, bool _CadMapEnab, bool _StreetMapEnab, bool _SpaceMapEnab)
        {
            Scale = _Scale;
            X = _X;
            Y = _Y;
            CadMapEnab = _CadMapEnab;
            StreetMapEnab = _StreetMapEnab;
            SpaceMapEnab = _SpaceMapEnab;
            Wkid = _Wkid;
        }
    }
}
