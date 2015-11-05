using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace aWhere.Api.CodeSample {
    class Fields {
        public const string FIELDS_ENDPOINT = "/v2/fields";

        public const string NEW_FIELD_ID = "mynewfield";
        private const double NEW_FIELD_LATITUDE 	= 39.8282;
private const double NEW_FIELD_LONGITUDE 	= -98.5795;
private const string NEW_FIELD_NAME 		= "My New Field";
        private const double NEW_FIELD_ACRES 		= 120;

private const string NEW_FIELD_FARM_ID 		= "F-1234-14-B";

        public static string BuildFieldsUrl() {
            string url = Program.HOST + FIELDS_ENDPOINT;
            return url;
        }

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
    }
}
