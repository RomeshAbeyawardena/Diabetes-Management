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

        public static string GetCaseSignature(this string value)
        {
            var array = new byte[value.Length];

            for(var i=0; i<value.Length; i++)
            {
                var val = value[i];
                array[i] = !char.IsNumber(val) && char.IsUpper(val) ? (byte)0x02 : (byte)0x04;
            }

            return Convert.ToBase64String(array);
        }

        public static string ProcessCaseSignature(this string value, string originalString)
        {
            var array = Convert.FromBase64String(value);

            var str = string.Empty;

            for(var j=0; j < array.Length; j++)
            {
                var val = originalString[j];
                str += array[j] == 0x02 ? char.ToUpper(val) : val;
            }

            return str;
        }

        public static string Encrypt(this string value, string algorithm, byte[] rgbKey, byte[]? rgbIV, out string caseSignature)
        {
            caseSignature = value.GetCaseSignature();
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

        public static string Decrypt(this string value, string algorithm, byte[] rgbKey, byte[]? rgbIV, string caseSignature)
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
