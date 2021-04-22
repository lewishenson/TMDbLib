using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TMDbLib.Utilities.Serializer
{
    public interface ITMDbSerializer
    {
        [Obsolete]
        void Serialize(Stream target, object obj, Type type);
        [Obsolete]
        object Deserialize(Stream source, Type type);

        // TODO: see which required
        string Serialize(object obj);
        Task SerializeAsync(Stream target, object obj, Type type, CancellationToken cancellationToken = default);
        Task<T> DeserializeAsync<T>(Stream source, CancellationToken cancellationToken = default);
        T Deserialize<T>(string json);
        Task<object> DeserializeAsync(Stream source, Type type, CancellationToken cancellationToken = default);
    }
}