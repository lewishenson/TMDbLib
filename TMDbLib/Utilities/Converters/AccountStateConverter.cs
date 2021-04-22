using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Utilities.Converters
{
    internal class AccountStateConverter : JsonConverter<AccountState>
    {
        public override AccountState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            AccountState accountState = new AccountState();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == 0)
                {
                    return accountState;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "favorite":
                            accountState.Favorite = reader.GetBoolean();
                            break;

                        case "id":
                            accountState.Id = reader.GetInt32();
                            break;

                        // Sometimes the AccountState.Rated is an object with a value in it
                        // In these instances, convert it from:
                        //  "rated": { "value": 5 }
                        //  "rated": False
                        // To:
                        //  "rating": 5
                        //  "rating": null
                        case "value":
                            accountState.Rating = reader.GetDouble();
                            break;

                        case "watchlist":
                            accountState.Watchlist = reader.GetBoolean();
                            break;
                    }
                }
            }

            throw new JsonException("Invalid JSON, unable to convert to T.");
        }

        public override void Write(Utf8JsonWriter writer, AccountState value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteBoolean("Favorite", value.Favorite);
            writer.WriteNumber("Id", value.Id);

            if (value.Rating is not { })
            {
                writer.WriteNull("Rating");
            }
            else
            {
                writer.WriteStartObject("Rating");
                writer.WriteNumber("Value", value.Rating.Value);
                writer.WriteEndObject();
            }

            writer.WriteBoolean("Watchlist", value.Watchlist);

            writer.WriteEndObject();
        }
    }
}