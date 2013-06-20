using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Weather.Logic
{
    public sealed class Weather
    {
        static readonly string FilePath = System.Environment.CurrentDirectory + @"\Area.txt";
        public static Models.Area GetCurrentArea()
        {
            var file = new FileInfo(FilePath);
            Models.Area result;
            if (!file.Exists)
            {
                //文件不存在就返回一个默认值,默认是成都
                result = new Models.Area();
                result.ID = 250;
                result.Name = "成都";
                result.ZoneID = 23;
                //area.Zone = new Models.Zone() { ID = 23, Name = "四川" };
                result.AreaCode = "56294";
            }
            else
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Models.Area));
                using (var stream = file.Open(FileMode.Open, FileAccess.Read))
                {
                    result = (Models.Area)ser.ReadObject(stream);                    
                }
            }
            return result;

            

            var area = new Models.Area();
            area.ID = 250;
            area.Name = "成都";
            area.ZoneID = 23;
            //area.Zone = new Models.Zone() { ID = 23, Name = "四川" };
            area.AreaCode = "56294";
            return area;
        }
        public static void SaveCurrentArea(Models.Area area)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Models.Area));
            var file = new FileInfo(FilePath);
            FileStream stream;
            if (file.Exists)
            {
                stream = file.Open(FileMode.Truncate, FileAccess.Write);
            }
            else
            {
                stream = file.Open(FileMode.Create, FileAccess.Write);
            }
            using (stream)
            {
                ser.WriteObject(stream, area);
            }
        }
    }
}
