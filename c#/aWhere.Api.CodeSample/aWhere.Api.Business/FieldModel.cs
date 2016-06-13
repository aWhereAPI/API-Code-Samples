using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aWhere.Api.ConsoleDemo;
using Newtonsoft.Json;

namespace aWhere.Api.Business {
    public class FieldModel {
        #region Fields

        public const string FIELDS_ENDPOINT = "/v2/fields/";

        #endregion Fields

        #region Properties

        public IEnumerable<Field> fields { get; set; }

        #endregion Properties

    }

    public class Field : Weather {

        #region Fields
        
        private const double NEW_FIELD_ACRES = 120;
        private const string NEW_FIELD_FARM_ID = "F-1234-14-B";
        private const double NEW_FIELD_LATITUDE = 39.8282;
        private const double NEW_FIELD_LONGITUDE = -98.5795;

        #endregion Fields

        #region Properties

        public string name { get; set; }
        public double? acres { get; set; }
        public CenterPoint centerPoint { get; set; }
        public string farmId { get; set; }
        public string id { get; set; }

        #endregion Properties

        #region Methods

        public Field BuildRandomField() {
            Menus.PrintEachLetterToConsole("Generating random Field.....");
            Field defaultField = new Field();
            defaultField.id = defaultField.GenerateRandomString();
            defaultField.name = defaultField.id;
            defaultField.farmId = NEW_FIELD_FARM_ID;
            defaultField.acres = NEW_FIELD_ACRES;
            defaultField.centerPoint = new CenterPoint(NEW_FIELD_LATITUDE, NEW_FIELD_LONGITUDE);
            Menus.PrintEachLetterToConsole(String.Format("Field ID: {0,-20}", defaultField.id));
            Menus.PrintEachLetterToConsole(String.Format("Field Name: {0,-20}", defaultField.name));
            Menus.PrintEachLetterToConsole(String.Format("Field Farm ID: {0,-20}", defaultField.farmId));
            Menus.PrintEachLetterToConsole(String.Format("Field Acres: {0,-20}", defaultField.acres));
            Menus.PrintEachLetterToConsole(String.Format("Field Latitude: {0,-20}", defaultField.centerPoint.Latitude));
            Menus.PrintEachLetterToConsole(String.Format("Field Longitude: {0,-20}", defaultField.centerPoint.Longitude));
            Console.WriteLine();

            return defaultField;
        }

        #endregion Methods
    }
}
