/*MIT License

Copyright (c) 2025 Julian-Rolfes

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/


using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Aes = System.Security.Cryptography.Aes;

// AES_Sharper class provides methods for AES encryption and decryption
class AES_Sharper
{
    // Constructor (does nothing)
    public AES_Sharper()
    {
    }

    // Encrypts the input string using the provided key
    public string Encode(string input, string key, byte[] saltBytes)
    {
        // Validate input and key
        if (input == "" || input.Length <= 0)
            throw new ArgumentNullException("input");
        if (key == "" || key.Length <= 0)
            throw new ArgumentNullException("key");

        byte[] temp;

        // Hash the key using SHA256 to obtain a fixed-length key for AES
        using var sha = SHA256.Create();
        byte[] keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
        using var kdf = new Rfc2898DeriveBytes(keyBytes, saltBytes, 100_000, HashAlgorithmName.SHA256);
        keyBytes = kdf.GetBytes(32); // 256-bit key

        // Create AES instance
        using (Aes AES = Aes.Create())
        {
            // Create encryptor with the hashed key and auto-generated IV
            ICryptoTransform encryptor = AES.CreateEncryptor(keyBytes, AES.IV);

            // Create memory stream for encryption output
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // Create crypto stream using the encryptor
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    // Use stream writer to write input into the crypto stream
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(input);
                    }
                    // Get encrypted bytes from memory stream
                    temp = msEncrypt.ToArray();

                    // Combine IV and encrypted data into one array (IV is needed for decryption)
                    byte[] combined = new byte[AES.IV.Length + temp.Length];
                    Buffer.BlockCopy(AES.IV, 0, combined, 0, AES.IV.Length);
                    Buffer.BlockCopy(temp, 0, combined, AES.IV.Length, temp.Length);

                    // Convert combined array to Base64 string for output
                    string output = Convert.ToBase64String(combined);
                    return output;
                }
            }
        }
    }

    // Decrypts the input string using the provided key
    public string Decode(string input, string key, byte[] saltBytes)
    {
        // Validate input and key
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input));
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        // Hash the key using SHA256 for AES
        using var sha = SHA256.Create();
        byte[] keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
        using var kdf = new Rfc2898DeriveBytes(keyBytes, saltBytes, 100_000, HashAlgorithmName.SHA256);
        keyBytes = kdf.GetBytes(32); // 256-bit key

        // Convert input from Base64 string to byte array
        byte[] combinedBytes = Convert.FromBase64String(input);

        // Validate that combinedBytes contains enough bytes for IV
        if (combinedBytes.Length < 16)
            throw new ArgumentException("Invalid input: too short for IV");

        // Extract IV (first 16 bytes) and encrypted data (remaining bytes)
        byte[] ivBytes = new byte[16];
        byte[] cipherBytes = new byte[combinedBytes.Length - 16];

        Buffer.BlockCopy(combinedBytes, 0, ivBytes, 0, 16);
        Buffer.BlockCopy(combinedBytes, 16, cipherBytes, 0, cipherBytes.Length);

        // Create AES instance for decryption
        using var aes = Aes.Create();
        ICryptoTransform decryptor = aes.CreateDecryptor(keyBytes, ivBytes);

        // Create memory stream from encrypted bytes
        using var msDecrypt = new MemoryStream(cipherBytes);

        // Create crypto stream for decryption
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

        // Use stream reader to read decrypted text
        using var srDecrypt = new StreamReader(csDecrypt);

        return srDecrypt.ReadToEnd();
    }
}
