using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Utilities.Serializer
{
    public class TMDbJsonSerializer : ITMDbSerializer
    {
        private readonly Encoding _encoding = new UTF8Encoding(false);
        private readonly JsonSerializerOptions _options;

        public static TMDbJsonSerializer Instance { get; } = new();

        private TMDbJsonSerializer()
        {
            // _serializer = JsonSerializer.CreateDefault();
            // _serializer.Converters.Add(new ChangeItemConverter());
            // _serializer.Converters.Add(new AccountStateConverter());
            // _serializer.Converters.Add(new KnownForConverter());
            // _serializer.Converters.Add(new SearchBaseConverter());
            // _serializer.Converters.Add(new TaggedImageConverter());
            // _serializer.Converters.Add(new TolerantEnumConverter());
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new AccountStateConverter());
        }

        public string Serialize(object obj) => 
            JsonSerializer.Serialize(obj, _options);

        public async Task SerializeAsync(Stream target, object obj, Type type, CancellationToken cancellationToken = default) =>
            await JsonSerializer.SerializeAsync(target, obj, type, _options, cancellationToken).ConfigureAwait(false);

        public async Task<T> DeserializeAsync<T>(Stream source, CancellationToken cancellationToken = default) =>
            await JsonSerializer.DeserializeAsync<T>(source, _options, cancellationToken).ConfigureAwait(false);

        public T Deserialize<T>(string json) =>
            JsonSerializer.Deserialize<T>(json, _options);

        public async Task<object> DeserializeAsync(Stream source, Type type, CancellationToken cancellationToken = default) =>
            await JsonSerializer.DeserializeAsync(source, type, _options, cancellationToken).ConfigureAwait(false);

        [Obsolete]
        public void Serialize(Stream target, object obj, Type type)
        {
            // using StreamWriter sw = new StreamWriter(target, _encoding, 4096, true);
            // using JsonTextWriter jw = new JsonTextWriter(sw);
            //
            // _serializer.Serialize(jw, obj, type);
        }

        [Obsolete]
        public object Deserialize(Stream source, Type type)
        {
            // using StreamReader sr = new StreamReader(source, _encoding, false, 4096, true);
            // using JsonTextReader jr = new JsonTextReader(sr);
            //
            // return _serializer.Deserialize(jr, type);
            return null;
        }
    }
}