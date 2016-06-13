using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace aWhere.Api.Business {
    public class Weather {
        #region Fields

        public const string WEATHER_ENDPOINT = "/v2/weather/fields/";

        #endregion Fields

        #region Methods

        public string GenerateRandomString() {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove the period
            return path;
        }

        #endregion Methods

    }

    public class Location {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string fieldId { get; set; }
    }

    public class Temperatures {
        public float max { get; set; }
        public float min { get; set; }
        public string units { get; set; }
    }

    public class Precipitation {
        public float amount { get; set; }
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Solar {
        public float amount { get; set; }
        public string units { get; set; }
    }

    public class Relativehumidity {
        public float? max { get; set; }
        public float? min { get; set; }
    }

    public class Wind {
        public float morningMax { get; set; }
        public float dayMax { get; set; }
        public float average { get; set; }
        public string units { get; set; }
    }

    public class Dewpoint {
        public float amount { get; set; }
        public string units { get; set; }
    }

    public class Sky {
        public double cloudCover { get; set; }
        public double sunshine { get; set; }
    }

    public class Meantemp {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Maxtemp {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Mintemp {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Minhumidity {
        public float average { get; set; }
        public float stdDev { get; set; }
    }

    public class Maxhumidity {
        public float average { get; set; }
        public float stdDev { get; set; }
    }

    public class Dailymaxwind {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Averagewind {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }
}
