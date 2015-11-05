using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aWhere.Api.CodeSample {
    public class Weather {
        public const string dateFormat = "yyyy-MM-dd";
        public const string observedStartDate = "2015-07-24";
        public const string observedEndDate = "2015-07-31";

        public static string forecastStartDate = DateTime.Today.ToString(dateFormat);
        public static string forecastEndDate = (DateTime.Today.AddDays(3)).ToString(dateFormat);

        public const string monthDayStart = "07-24";
        public const string monthDayEnd = "07-31";

        public static int threeYearStartYear = DateTime.Today.Year - 4;
        public static int tenYearStartYear = DateTime.Today.Year - 11;
        public static int endYear = DateTime.Today.Year - 1;

        public static string BuildObservationsUrl() {
            string url = Program.HOST + "/v2/weather/fields/" + Fields.NEW_FIELD_ID + "/observations/" + observedStartDate + "," + observedEndDate;
            return url;
        }

        public static string BuildForecastsUrl() {
            string url = Program.HOST + "/v2/weather/fields/" + Fields.NEW_FIELD_ID + "/forecasts/" + forecastStartDate + "," + forecastEndDate;

            return url;
        }

        public static string BuildThreeYearNormsUrl() {
            string url = Program.HOST + "/v2/weather/fields/" + Fields.NEW_FIELD_ID + "/norms/" + monthDayStart + "," + monthDayEnd + "/years/" + threeYearStartYear + "," + endYear;
            return url;
        }

        public static string BuildTenYearNormsUrl() {
            string url = Program.HOST + "/v2/weather/fields/" + Fields.NEW_FIELD_ID + "/norms/" + monthDayStart + "," + monthDayEnd + "/years/" + tenYearStartYear + "," + endYear;
            return url;
        }


    }
}
