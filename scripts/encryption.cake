using System;
using System.IO;
using System.Security.Cryptography;

private static readonly int BlockBitSize = 128;

// To be sure we get the correct IV size, set the block size
private static readonly int KeyBitSize = 256;

// AES 256 bit key encryption
// Preconfigured Password Key Derivation Parameters
private static readonly int SaltBitSize = 128;

private static readonly int Iterations = 10000;

/// <summary>
///     Encrypts the plainText input using the given Key.
///     A 128 bit random salt will be generated and prepended to the ciphertext before it is base64 encoded.
///     A 16 bit random Initialization Vector will also be generated prepended to the ciphertext before it is base64
///     encoded.
/// </summary>
/// <param name="plainText">The plain text to encrypt.</param>
/// <param name="key">The plain text encryption key.</param>
/// <returns>The salt, IV and the ciphertext, Base64 encoded.</returns>
public static string Encrypt(string plainText, string key)
{
    //User Error Checks
    if (string.IsNullOrEmpty(key))
    {
        throw new ArgumentNullException("key");
    }

    if (string.IsNullOrEmpty(plainText))
    {
        throw new ArgumentNullException("plainText");
    }

    // Derive a new Salt and IV from the Key, using a 128 bit salt and 10,000 iterations
    using (var keyDerivationFunction = new Rfc2898DeriveBytes(key, SaltBitSize / 8, Iterations))
    {
        using (var aesManaged = CreateAesImplementationInstance())
        {
            aesManaged.KeySize = KeyBitSize;
            aesManaged.BlockSize = BlockBitSize;

            // Generate random IV
            aesManaged.GenerateIV();

            // Retrieve the Salt, Key and IV
            var saltBytes = keyDerivationFunction.Salt;
            var keyBytes = keyDerivationFunction.GetBytes(KeyBitSize / 8);
            var ivBytes = aesManaged.IV;

            // Create an encryptor to perform the stream transform.
            // Create the streams used for encryption.
            using (var encryptor = aesManaged.CreateEncryptor(keyBytes, ivBytes))
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            // Send the data through the StreamWriter, through the CryptoStream, to the underlying MemoryStream
                            streamWriter.Write(plainText);
                        }
                    }

                    // Return the encrypted bytes from the memory stream in Base64 form.
                    var cipherTextBytes = memoryStream.ToArray();

                    // Resize saltBytes and append IV
                    Array.Resize(ref saltBytes, saltBytes.Length + ivBytes.Length);
                    Array.Copy(ivBytes, 0, saltBytes, SaltBitSize / 8, ivBytes.Length);

                    // Resize saltBytes with IV and append cipherText
                    Array.Resize(ref saltBytes, saltBytes.Length + cipherTextBytes.Length);
                    Array.Copy(cipherTextBytes, 0, saltBytes, SaltBitSize / 8 + ivBytes.Length,
                        cipherTextBytes.Length);

                    return Convert.ToBase64String(saltBytes);
                }
            }
        }
    }
}

private static Aes CreateAesImplementationInstance()
{
    return Aes.Create() ?? throw new Exception("AES Crypto implementation is unavailable.");
}

/// <summary>
///     Decrypts the ciphertext using the Key.
/// </summary>
/// <param name="ciphertext">The ciphertext to decrypt.</param>
/// <param name="key">The plain text encryption key.</param>
/// <returns>The decrypted text.</returns>
public static string Decrypt(string ciphertext, string key)
{
    if (string.IsNullOrEmpty(ciphertext))
    {
        throw new ArgumentNullException(nameof(ciphertext));
    }

    if (string.IsNullOrEmpty(key))
    {
        throw new ArgumentNullException(nameof(key));
    }

    // Prepare the Salt and IV arrays
    var saltBytes = new byte[SaltBitSize / 8];
    var ivBytes = new byte[BlockBitSize / 8];

    // Read all the bytes from the cipher text
    var allTheBytes = Convert.FromBase64String(ciphertext);

    // Extract the Salt, IV from our ciphertext
    Array.Copy(allTheBytes, 0, saltBytes, 0, saltBytes.Length);
    Array.Copy(allTheBytes, saltBytes.Length, ivBytes, 0, ivBytes.Length);

    // Extract the Ciphered bytes
    var ciphertextBytes = new byte[allTheBytes.Length - saltBytes.Length - ivBytes.Length];
    Array.Copy(allTheBytes, saltBytes.Length + ivBytes.Length, ciphertextBytes, 0, ciphertextBytes.Length);

    using (var keyDerivationFunction = new Rfc2898DeriveBytes(key, saltBytes, Iterations))
    {
        // Get the Key bytes
        var keyBytes = keyDerivationFunction.GetBytes(KeyBitSize / 8);

        // Create a decrytor to perform the stream transform.
        // Create the streams used for decryption.
        // The default Cipher Mode is CBC and the Padding is PKCS7 which are both good
        using (var aesManaged = CreateAesImplementationInstance())
        {
            aesManaged.KeySize = KeyBitSize;
            aesManaged.BlockSize = BlockBitSize;

            using (var decryptor = aesManaged.CreateDecryptor(keyBytes, ivBytes))
            {
                using (var memoryStream = new MemoryStream(ciphertextBytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            // Return the decrypted bytes from the decrypting stream.
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}

private static string GetHashInternal(Func<MD5, byte[]> compute)
{
    using (var md5 = MD5.Create())
    {
        var hash = compute(md5);
        return BitConverter
            .ToString(hash)
            .Replace("-", "")
            .ToLowerInvariant();
    }
}

public static string GetStringHash(string data) => 
    GetDataHash(System.Text.Encoding.Unicode.GetBytes(data));

public static string GetDataHash(byte[] data)
{
    if (data == null)
    {
        return null;
    }

    return GetHashInternal(md5 => md5.ComputeHash(data));
}

public static string Decrypt(string value)
{
    return Decrypt(value, System.Environment.GetEnvironmentVariable("MASTER_KEY"));
}

public static string Encrypt(string value)
{
    return Encrypt(value, System.Environment.GetEnvironmentVariable("MASTER_KEY"));
}

Task("Encrypt").Does(() =>
{
    var text = Console.ReadLine().Trim();
    var crypt = Encrypt(text);
    Console.WriteLine($"Encrypted: '{text}'\n{crypt}");
});

Task("Decrypt").Does(() =>
{
    Console.WriteLine("Decrypted: " + Decrypt(Console.ReadLine().Trim()));
});