using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace aWhere.Api.CodeSample {
    internal class Fields {
        #region Fields
        public const string FIELDS_ENDPOINT = "/v2/fields";

        public const string NEW_FIELD_ID = "mynewfield";
        private const double NEW_FIELD_ACRES = 120;
        private const string NEW_FIELD_FARM_ID = "F-1234-14-B";
        private const double NEW_FIELD_LATITUDE = 39.8282;
        private const double NEW_FIELD_LONGITUDE = -98.5795;
        private const string NEW_FIELD_NAME = "My New Field";
        #endregion Fields
        

#region Methods

public static string BuildCreateFieldPayload() {
    string payload = "{ \"id\":" + "\"" + NEW_FIELD_ID + "\"" + "," +
                       "\"name\":" + "\"" + NEW_FIELD_NAME + "\"" + "," +
                       "\"farmId\":" + "\"" + NEW_FIELD_FARM_ID + "\"" + "," +
                       "\"acres\":" + NEW_FIELD_ACRES + "," +
                       "\"centerpoint\":{" +
                       "\"latitude\":" + NEW_FIELD_LATITUDE + "," +
                       "\"longitude\":" + NEW_FIELD_LONGITUDE +
                       "}}";

    JsonConvert.SerializeObject(payload);
    return payload;
}

public static string BuildFieldsUrl() {
            string url = Program.HOST + FIELDS_ENDPOINT;
            return url;
}

#endregion Methods
    }
}