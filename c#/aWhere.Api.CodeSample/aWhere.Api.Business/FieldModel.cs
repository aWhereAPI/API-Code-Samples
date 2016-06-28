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
            
            Field demoField = new Field()
            {
                id = GenerateRandomFieldName(),
                farmId = NEW_FIELD_FARM_ID,
                acres = NEW_FIELD_ACRES,
                centerPoint = new CenterPoint(NEW_FIELD_LATITUDE, NEW_FIELD_LONGITUDE)
            };

            demoField.name = demoField.id;

            Menus.PrintEachLetterToConsole(String.Format("Field ID: {0,-20}", demoField.id));
            Menus.PrintEachLetterToConsole(String.Format("Field Name: {0,-20}", demoField.name));
            Menus.PrintEachLetterToConsole(String.Format("Field Farm ID: {0,-20}", demoField.farmId));
            Menus.PrintEachLetterToConsole(String.Format("Field Acres: {0,-20}", demoField.acres));
            Menus.PrintEachLetterToConsole(String.Format("Field Latitude: {0,-20}", demoField.centerPoint.Latitude));
            Menus.PrintEachLetterToConsole(String.Format("Field Longitude: {0,-20}", demoField.centerPoint.Longitude));
            Console.WriteLine();

            return demoField;
        }

        #endregion Methods
    }
}
