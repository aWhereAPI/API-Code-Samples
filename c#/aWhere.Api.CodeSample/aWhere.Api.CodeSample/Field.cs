using System;
using System.Linq;
using Newtonsoft.Json;

namespace aWhere.Api.CodeSample {
    public class Field {
        #region Fields
        public const string FIELDS_ENDPOINT = "/v2/fields";

        public const string NEW_FIELD_ID = "mynewfield";
        private const double NEW_FIELD_ACRES = 120;
        private const string NEW_FIELD_FARM_ID = "F-1234-14-B";
        private const double NEW_FIELD_LATITUDE = 39.8282;
        private const double NEW_FIELD_LONGITUDE = -98.5795;
        private const string NEW_FIELD_NAME = "My New Field";

        #endregion Fields

        #region Propeties

        [JsonProperty("acres", Order = 4)]
        public double Acres { get; set; }

        [JsonProperty("centerpoint", Order = 5)]
        public LatLong Centerpoint { get; set; }

        [JsonProperty("farmId", Order = 3)]
        public string FarmId { get; set; }

        [JsonProperty("id", Order = 1)]
        public string Id { get; set; }

        [JsonProperty("name", Order = 2)]
        public string Name { get; set; }

        #endregion Propeties

        #region Methods

        public static Field BuildDefaultField() {
            Field defaultField = new Field();
            defaultField.Id = NEW_FIELD_ID;
            defaultField.Name = NEW_FIELD_NAME;
            defaultField.FarmId = NEW_FIELD_FARM_ID;
            defaultField.Acres = NEW_FIELD_ACRES;
            defaultField.Centerpoint = new LatLong(NEW_FIELD_LATITUDE, NEW_FIELD_LONGITUDE);

            return defaultField;
        }

        public static string BuildFieldsUrl() {
                    string url = Program.HOST + FIELDS_ENDPOINT;
                    return url;
        }

        #endregion Methods
    }
}