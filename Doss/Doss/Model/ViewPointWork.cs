using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Mapping;
using System.IO;
using System.Xml.Serialization;
using System.Windows;
using Microsoft.Win32;

namespace Doss.Model
{
    class ViewPointWork
    {
        static public void SavePoint(MyViewPoint _ViewPoint)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(MyViewPoint));

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "File (*.xml)|*.xml";
                sfd.FileName = "Рабочая область";
                if (sfd.ShowDialog() == true)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.OpenFile(), System.Text.Encoding.Default))
                    {
                    formatter.Serialize(sw, _ViewPoint);
                    }
                }
                
        }

        static public MyViewPoint LoadPoint()
        {
            MyViewPoint result = new MyViewPoint();

            XmlSerializer formatter = new XmlSerializer(typeof(MyViewPoint));

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "File (*.xml)|*.xml";
            ofd.FileName = "Рабочая область";
            if (ofd.ShowDialog() == true)
            {
                using (StreamReader sr = new StreamReader(ofd.OpenFile(), System.Text.Encoding.Default))
                {
                    result = (MyViewPoint)formatter.Deserialize(sr);
                }
            }

            return result;

        }
    }

    
    
}
