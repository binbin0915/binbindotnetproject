using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Smoke.Weather.Models
{
    public class Zone
    {
        public int ID { get; set; }
        public string Name { get; set; }        
        public ObservableCollection<Area> AreaList { get; set; }
        public Zone()
        {
            this.AreaList = new ObservableCollection<Area>();
        }
    }
}
