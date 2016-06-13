using Newtonsoft.Json;
using System.Text;
using System;

namespace aWhere.Api.Business {
    public class CenterPoint {
        
        #region Constructors

        public CenterPoint() {
        }

        public CenterPoint(double latitude, double longitude) {
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

        #region Overrides
        public override string ToString() {

            return String.Format("Lat: {0} - Long: {1}", Latitude, Longitude);
        }
        #endregion Overrides
    }
}
