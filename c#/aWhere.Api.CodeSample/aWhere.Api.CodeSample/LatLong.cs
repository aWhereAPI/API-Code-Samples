using System;
using System.Linq;
using Newtonsoft.Json;

namespace aWhere.Api.CodeSample {
    public class LatLong {
        #region Constructors

        public LatLong() {
        }

        public LatLong(double latitude, double longitude) {
            Latitude = latitude;
            Longitude = longitude;
        }

        #endregion Constructors

        #region Properties

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        #endregion Properties
    }
}