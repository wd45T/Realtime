namespace Realtime.Chat.Common.TransportLayer.Utilities
{
    public interface IDataConverter
    {  
        /// <summary>
        /// Converts an bytes string to an object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="bytesString">Serialized bytes array by json</param>
        /// <remarks>
        /// Example bytes string format: "\"gq5QbGF5ZXJQdWJsaWNJZM4HXtKFq1BsYWNlTnVtYmVyCA==\""
        /// </remarks>
        /// <returns>Deserialized object by bytes string</returns>
        T BytesStringToObject<T>(string bytesString);

        /// <summary>
        /// Converts an object to an array of bytes as a json string
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="object">Some type T object</param>
        /// <remarks>
        /// Example bytes string format: "\"gq5QbGF5ZXJQdWJsaWNJZM4HXtKFq1BsYWNlTnVtYmVyCA==\""
        /// </remarks>
        /// <returns>Object serialized bytes array by json</returns>
        string ObjectToBytesString<T>(T @object);

        /// <summary>
        /// Converts an object to an array of bytes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="object">Some type T object</param>
        /// <returns>Object serialized into an array of bytes</returns>
        byte[] ObjectToBytes<T>(T @object);

        /// <summary>
        /// Converts an array of bytes to an object
        /// </summary>
        /// <typeparam name="T">Some type T object</typeparam>
        /// <param name="bytes">Object as an array of bytes</param>
        /// <returns>Object deserialized from byte array</returns>
        T BytesToObject<T>(byte[] bytes);
    }
}
