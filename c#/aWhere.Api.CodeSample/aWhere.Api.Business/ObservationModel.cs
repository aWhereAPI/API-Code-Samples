using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aWhere.Api.Business {
    public class ObservationModel {

        #region Fields
        public static string OBSERVATION_ENDPOINT = "/observations";
        #endregion Fields

        #region Properties
        public IEnumerable<Observation> observations { get; set; }
        #endregion Properties
    }


    #region Inner Observation Classes
    public class Observation {
        public string date { get; set; }
        public Location location { get; set; }
        public Temperatures temperatures { get; set; }
        public Precipitation precipitation { get; set; }
        public Solar solar { get; set; }
        public Relativehumidity relativeHumidity { get; set; }
        public Wind wind { get; set; }
    }
    #endregion Inner Observation Classes
}
