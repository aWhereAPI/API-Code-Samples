using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aWhere.Api.Business {
    public class NormsModel {

        #region Fields

        public const string NORMS_ENDPOINT = "/norms/06-01/years/2013,2016";

        #endregion Fields

        #region Properties

        public string day { get; set; }
        public Location location { get; set; }
        public Meantemp meanTemp { get; set; }
        public Maxtemp maxTemp { get; set; }
        public Mintemp minTemp { get; set; }
        public Precipitation precipitation { get; set; }
        public Solar solar { get; set; }
        public Minhumidity minHumidity { get; set; }
        public Maxhumidity maxHumidity { get; set; }
        public Dailymaxwind dailyMaxWind { get; set; }
        public Averagewind averageWind { get; set; }

        #endregion Properties
    }


}
