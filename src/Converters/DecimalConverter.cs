namespace Talegen.AspNetCore.App.Converters
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using System.Globalization;

    /// <summary>
    /// This class represents a custom JSON converter for decimal values.
    /// </summary>
    public class DecimalConverter : JsonConverter
    {
        /// <summary>
        /// This method is used to determine if the specified object type can be converted.
        /// </summary>
        /// <param name="objectType">Contains the object type.</param>
        /// <returns>Returns a value indicating conversion support.</returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(decimal?));
        }

        /// <summary>
        /// This method is used to read the JSON value and convert it to a decimal value.
        /// </summary>
        /// <param name="reader">Contains the reader.</param>
        /// <param name="objectType">Contains the object type.</param>
        /// <param name="existingValue">Contains the existing value.</param>
        /// <param name="serializer">Contains the JSON serializer.</param>
        /// <returns></returns>
        /// <exception cref="JsonSerializationException">Returns if an unexpected decimal type is found.</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            
            if (token.Type == JTokenType.Float || token.Type == JTokenType.Integer)
            {
                return token.ToObject<decimal>();
            }
            
            if (token.Type == JTokenType.String)
            {
                // customize this to suit your needs
                return Decimal.Parse(token.ToString(), CultureInfo.CurrentCulture);
            }
            
            if (token.Type == JTokenType.Null && objectType == typeof(decimal?))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            throw new JsonSerializationException("Unexpected token type: " + token.Type.ToString());
        }

        /// <summary>
        /// This method is used to write the JSON value to the output stream.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
