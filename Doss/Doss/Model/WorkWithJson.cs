using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Doss.Model
{
    class WorkWithJson
    {
        public Features Features1;
        public WorkWithJson()
        {
            var obj = JsonConvert.DeserializeObject<Features>(File.ReadAllText(@"..\..\Samples\Json\Example1.json"));
            using (StreamReader file = File.OpenText(@"..\..\Samples\Json\Example1.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Features1 = (Features)serializer.Deserialize(file, typeof(Features));
            }
        }
    }
}
