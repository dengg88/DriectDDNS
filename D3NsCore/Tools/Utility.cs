using System.IO;

namespace D3NsCore.Tools
{
    internal static class Utility
    {
        public static byte[] ToBytes(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}