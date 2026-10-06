using System.Text;

namespace DesignABear.Services.Helpers
{
    /// <summary>
    /// Simple reversible encoding of a Guid to a URL-safe string.
    /// Useful for obfuscating entity IDs in public-facing URLs.
    /// </summary>
    public static class GuidObfuscator
    {
        /// <summary>
        /// Encode a Guid to a compact URL-safe Base64 string.
        /// </summary>
        public static string Encode(Guid id) =>
            Convert.ToBase64String(id.ToByteArray())
                   .TrimEnd('=')
                   .Replace('+', '-')
                   .Replace('/', '_');

        /// <summary>
        /// Decode a URL-safe Base64 string back to a Guid.
        /// </summary>
        public static Guid Decode(string encoded)
        {
            if (string.IsNullOrWhiteSpace(encoded))
                throw new ArgumentException("Encoded ID cannot be null or empty.");

            try
            {
                var padded = encoded.Replace('-', '+').Replace('_', '/')
                             + (encoded.Length % 4) switch
                             {
                                 2 => "==",
                                 3 => "=",
                                 _ => ""
                             };
                var bytes = Convert.FromBase64String(padded);
                return new Guid(bytes);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid encoded ID format.", ex);
            }
        }
    }
}
