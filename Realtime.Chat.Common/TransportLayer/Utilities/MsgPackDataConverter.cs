using MessagePack;
using Newtonsoft.Json;

namespace Realtime.Chat.Common.TransportLayer.Utilities
{
    public class MsgPackDataConverter : IDataConverter
    {
        public T BytesStringToObject<T>(string bytesString)
        {
            var bytes = JsonConvert.DeserializeObject<byte[]>(bytesString);

            var @object = MessagePackSerializer.Deserialize<T>(bytes);

            return @object;
        }

        public string ObjectToBytesString<T>(T @object)
        {
            var bytes = MessagePackSerializer.Serialize(@object);

            var bytesString = JsonConvert.SerializeObject(bytes);

            return bytesString;
        }

        public T BytesToObject<T>(byte[] bytes)
        {
            var @object = MessagePackSerializer.Deserialize<T>(bytes);

            return @object;
        }

        public byte[] ObjectToBytes<T>(T @object)
        {
            var bytes = MessagePackSerializer.Serialize(@object);

            return bytes;
        }
    }
}
