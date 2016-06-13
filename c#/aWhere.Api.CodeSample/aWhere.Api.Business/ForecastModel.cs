using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace aWhere.Api.Business {
    public class ForecastModel {

        #region Fields
        public static string FORECAST_ENDPOINT = "/forecasts/?blockSize=24";
        #endregion Fields

        #region Properties

        public IEnumerable<Forecast> forecasts { get; set; }
        #endregion Properties

    }

    public class Forecast {
        public string date { get; set; }
        public Location location { get; set; }
        public IEnumerable<ForecastsData> forecast { get; set; }
    }

    public class ForecastsData {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string conditionsCode { get; set; }
        public string conditionsText { get; set; }
        public Temperatures temperatures { get; set; }
        public Precipitation precipitation { get; set; }
        public Sky sky { get; set; }
        public Solar solar { get; set; }
        public Relativehumidity relativeHumidity { get; set; }
        public Wind wind { get; set; }
        public Dewpoint dewPoint { get; set; }
    }
}
