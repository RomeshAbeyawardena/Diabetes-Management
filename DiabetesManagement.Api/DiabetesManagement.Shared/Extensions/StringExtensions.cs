using System.Security.Cryptography;

namespace DiabetesManagement.Shared.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetBytes(string value)
        {
#pragma warning disable IDE0063
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    streamWriter.Write(value);
                }

                return memoryStream.ToArray();
            }
#pragma warning restore IDE0063
        }

        public static string Encrypt(this string value, string algorithm, byte[] rgbKey, byte[]? rgbIV)
        {
            using var algo = SymmetricAlgorithm.Create(algorithm);
            var encryptor = algo!.CreateEncryptor(rgbKey, rgbIV);
            using (var ms = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cryptoStream))
                    streamWriter.Write(value);

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static string Decrypt(this string value, string algorithm, byte[] rgbKey, byte[]? rgbIV)
        {
            using var algo = SymmetricAlgorithm.Create(algorithm);
            var encryptor = algo!.CreateDecryptor(rgbKey, rgbIV);
            using (var ms = new MemoryStream(Convert.FromBase64String(value)))
            {
                using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static string Hash(this string value, string algorithm, string hashSalt)
        {
            using var hashAlgorithm = HashAlgorithm.Create(algorithm);

            var saltedValue = $"!{value}-$-$-{hashSalt}%";

            return Convert.ToBase64String(hashAlgorithm!.ComputeHash(GetBytes(value)));
        }
    }
}
