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


using System.Security.Cryptography;
using System.Text;
using Aes = System.Security.Cryptography.Aes;


class AES_Sharper
{
    public AES_Sharper()
    {
        

    }
    public string Encode(string input, string key)
    {
        if (input == "" || input.Length <= 0)
            throw new ArgumentNullException("input");
        if (key == "" || key.Length <= 0)
            throw new ArgumentNullException("key");
        byte[] temp;
        using var sha = SHA256.Create();
        byte[] keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(key));


        using (Aes AES = Aes.Create())
        {
            

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = AES.CreateEncryptor(keyBytes, AES.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(input);
                    }
                    temp = msEncrypt.ToArray();
                    byte[] combined = new byte[AES.IV.Length + temp.Length];
                    Buffer.BlockCopy(AES.IV, 0, combined, 0, AES.IV.Length);
                    Buffer.BlockCopy(temp, 0, combined, AES.IV.Length, temp.Length);

                    string output = Convert.ToBase64String(combined);
                    return output;


                }
            }
        }

       
    }
    public string Decode(string input, string key)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input));
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        // Key per SHA256 auf 32 Bytes bringen (AES-256)
        using var sha = SHA256.Create();
        byte[] keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(key));

        // Input Base64-decodieren (IV + Ciphertext)
        byte[] combinedBytes = Convert.FromBase64String(input);

        if (combinedBytes.Length < 16)
            throw new ArgumentException("Ungültiger Input: zu kurz für IV");

        // IV = erste 16 Bytes, Ciphertext = Rest
        byte[] ivBytes = new byte[16];
        byte[] cipherBytes = new byte[combinedBytes.Length - 16];

        Buffer.BlockCopy(combinedBytes, 0, ivBytes, 0, 16);          // IV abschneiden
        Buffer.BlockCopy(combinedBytes, 16, cipherBytes, 0, cipherBytes.Length);

        // AES entschlüsseln
        using var aes = Aes.Create();
        ICryptoTransform decryptor = aes.CreateDecryptor(keyBytes, ivBytes);

        using var msDecrypt = new MemoryStream(cipherBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);

        return srDecrypt.ReadToEnd();
    }


}
