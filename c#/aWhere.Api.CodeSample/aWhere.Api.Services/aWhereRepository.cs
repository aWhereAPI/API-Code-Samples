using System;
using System.Linq;
using System.Threading.Tasks;
using aWhere.Api.Business;
using aWhere.Api.ConsoleDemo;

namespace aWhere.Api.Services {
    public class aWhereRepository {
        #region Fields

        private const string BaseUrl = "https://api.awhere.com";
        private aWhereApiConnection ApiConnection;

        #endregion Fields

        #region Constructors

        public aWhereRepository(aWhereApiConnection ApiConnection) {
            this.ApiConnection = ApiConnection;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Performs a HTTP "POST" request to Create a Field.
        /// </summary>
        /// <returns></returns>
        public async Task CreateFieldPostRequestAsync() {
            var fieldPayload = new Field();
            string uri = BaseUrl + FieldModel.FIELDS_ENDPOINT;
            fieldPayload = fieldPayload.BuildRandomField();

            var postRequest = new ApiRequest<Field>(ApiConnection);
            await postRequest.MakeApiCall("POST", uri, fieldPayload);
        }

        /// <summary>
        /// Performs a HTTP "DELETE" request to Delete a Field.
        /// API Request: DELETE /v2/fields/{fieldId}
        /// Important: This API has a lower rate limit than the rest of the API platform. This API's limit is 1 request per second.
        /// </summary>
        /// <param name="fieldId">The ID of the Field that you used when you created it.</param>
        /// <returns></returns>
        public async Task DeleteFieldByIdAsync(string fieldId) {
            var request = new ApiRequest<Field>(ApiConnection);
            string uri = BaseUrl + FieldModel.FIELDS_ENDPOINT + fieldId;

            var deletedField = await request.MakeApiCall("DELETE", uri, null);

        }

        /// <summary>
        /// Performs a HTTP "GET" request to obtain all registered Fields on an account.
        /// </summary>
        /// <returns></returns>
        public async Task GetAllFieldsAsync() {
            var request = new ApiRequest<FieldModel>(ApiConnection);
            string uri = BaseUrl + FieldModel.FIELDS_ENDPOINT;

            var fieldModel = await request.MakeApiCall("GET", uri, null);

            if (fieldModel != null) {
                Menus.PrintBlankSpace(3);
                Menus.WriteLineCentered(String.Format("There are {0} field locations in the current page of results.\n", fieldModel.fields.Count().ToString()));
                Menus.DrawCenteredTitle(String.Format("{0, -12} {1, -12}", "Field ID", "Field Name"));

                for (int i = 0; i < fieldModel.fields.Count(); i++) {
                    Menus.WriteCentered(String.Format("{0} \t {1}", fieldModel.fields.ElementAt(i).id, fieldModel.fields.ElementAt(i).name));
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Performs a HTTP "GET" request to obtain information about an individual Field.
        /// API Request: GET /v2/fields/{fieldId}
        /// </summary>
        /// <param name="fieldId">The ID of the Field that you used when you created it.</param>
        /// <returns></returns>
        public async Task GetFieldByIdAsync(string fieldId) {
            var request = new ApiRequest<Field>(ApiConnection);
            string uri = BaseUrl + FieldModel.FIELDS_ENDPOINT + fieldId;

            var singleFieldModel = await request.MakeApiCall("GET", uri, null);

            if (singleFieldModel != null) {
                Menus.PrintBlankSpace(3);
                Menus.DrawCenteredTitle(String.Format("You requested information about {0}. Here are the results:", fieldId));

                Menus.WriteLineCentered(String.Format("The Field ID is {0}, the Field Name is {1}. Acres: {2} Farm ID: {3} Location: {4}", singleFieldModel.id, singleFieldModel.name, singleFieldModel.acres, singleFieldModel.farmId, singleFieldModel.centerPoint.ToString()));
            }
        }

        /// <summary>
        /// Performs a HTTP "GET" request to get the forecast at a specific Field location.
        /// API Request: GET /v2/weather/fields/{fieldId}/forecasts
        /// </summary>
        /// <param name="id">The ID of the Field that you used when you created it.</param>
        /// <returns></returns>
        public async Task GetWeatherForecastByIdAsync(string id) {
            var forecastRequest = new ApiRequest<ForecastModel>(ApiConnection);
            string uri = BaseUrl + Weather.WEATHER_ENDPOINT + id + ForecastModel.FORECAST_ENDPOINT ;

            var forecastModel = await forecastRequest.MakeApiCall("GET", uri, null);

            if (forecastModel != null) {
                Menus.PrintBlankSpace(3);
                Menus.DrawCenteredTitle(String.Format("You requested the default Forecast end point, which returns {0} days of forecast.\n", forecastModel.forecasts.Count().ToString()));

                foreach (Forecast item in forecastModel.forecasts) {
                    foreach (ForecastsData forecast in item.forecast) {
                        Menus.WriteLineCentered(String.Format("The forecasted weather on {0} is a high temp of {1}C and a low of {2}C.",
                            item.date, forecast.temperatures.max, forecast.temperatures.min));
                        Menus.WriteLineCentered(String.Format("Conditions are {0}\n", forecast.conditionsText));
                    }
                }
            }
        }

        /// <summary>
        /// Performs a HTTP "GET" request to get the norms at a specific Field location. Returns the average of the last 10 years for either a single day or a range of days. In this demo, this returns the range for a single day.
        /// API Request: GET /v2/weather/fields/{fieldId}/norms/{month-day}
        /// </summary>
        /// <param name="id">The ID of the Field that you used when you created it.</param>
        /// <returns></returns>
        public async Task GetWeatherNormsByIdAsync(string id) {
            var normsRequest = new ApiRequest<NormsModel>(ApiConnection);
            string uri = BaseUrl + Weather.WEATHER_ENDPOINT + id + NormsModel.NORMS_ENDPOINT;

            var normsModel = await normsRequest.MakeApiCall("GET", uri, null);

            if (normsModel != null) {
                Menus.PrintBlankSpace(3);
                Menus.DrawCenteredTitle(String.Format("You requested the norms for field {0} over a ten year span (2010-2016).", id));

                Menus.WriteLineCentered(String.Format("These results are the norms on 06/01 over a ten year span. Your location was ({0},{1}).", normsModel.location.latitude, normsModel.location.longitude));

                Menus.WriteLineCentered(String.Format("The mean temp was an average of {0}C with a standard deviation of {1}",
                    normsModel.meanTemp.average,
                    normsModel.meanTemp.stdDev));

                Menus.WriteLineCentered(String.Format("The max temp was an average of {0}C with a standard deviation of {1}",
                    normsModel.maxTemp.average,
                    normsModel.maxTemp.stdDev));

                Menus.WriteLineCentered(String.Format("The min temp was an average of {0}C with a standard deviation of {1}",
                    normsModel.minTemp.average,
                    normsModel.minTemp.stdDev));

                Menus.WriteLineCentered(String.Format("The precipitation was an average of {0}mm with a standard deviation of {1}",
                    normsModel.maxTemp.average,
                    normsModel.maxTemp.stdDev));
            }
        }

        /// <summary>
        /// Performs a HTTP "GET" request to get Daily Observation data at a specific Field location. Returns the last 7 days of data for the selected field location. In this demo, this returns the default range which is 7 days.
        /// API Request: GET /v2/weather/fields/{fieldId}/observations
        /// </summary>
        /// <param name="id">The ID of the Field that you used when you created it.</param>
        /// <returns></returns>
        public async Task GetWeatherObservationsForFieldByIdAsync(string id) {
            var request = new ApiRequest<ObservationModel>(ApiConnection);
            string uri = BaseUrl + Weather.WEATHER_ENDPOINT + id + ObservationModel.OBSERVATION_ENDPOINT;

            var dailyObservations = await request.MakeApiCall("GET", uri, null);

            if (dailyObservations != null) {
                Menus.PrintBlankSpace(3);
                Menus.DrawCenteredTitle(String.Format("You requested the default Observation end point, which returns {0} days of historical observed weather.", dailyObservations.observations.Count().ToString()));

                foreach (Observation item in dailyObservations.observations) {
                    Menus.WriteLineCentered(String.Format("The weather on {0} was a low temp of {1}C and a high of {2}C with {3}mm precipitation\n", item.date, item.temperatures.min, item.temperatures.max, item.precipitation.amount));
                }
            }
        }

        #endregion Methods
    }
}