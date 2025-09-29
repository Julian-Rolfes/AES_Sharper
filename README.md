# AES_Sharper

MIT License – Free to use in your projects.

A simple and efficient .NET library for AES-256 encryption with SHA256 key derivation.

---

## Features

- AES-256 encryption  
- SHA256 key derivation  
- Easy to use  
- Secure by default  
- No external dependencies  

---

## Installation

Copy the `AES_Sharper.cs` file into your .NET project.  


## Quick Start

```csharp
// Create instance
var aes = new AES_Sharper();

// Encrypt
string encrypted = aes.Encode("Hello World", "myPassword123");

// Decrypt
string decrypted = aes.Decode(encrypted, "myPassword123");

Console.WriteLine(decrypted); // Output: "Hello World"
```

---

## API Reference

### Constructor

```csharp
var aes = new AES_Sharper();
```

### Methods

#### Encode(string input, string key)

Encrypts a string using the provided key.

- **Parameters:**
  - `input` – The text to encrypt
  - `key` – The encryption password
- **Returns:** Base64 encoded encrypted string

#### Decode(string input, string key)

Decrypts a string using the provided key.

- **Parameters:**
  - `input` – Base64 encoded encrypted string
  - `key` – The decryption password
- **Returns:** Original plaintext string

---

## Examples

```csharp
var aes = new AES_Sharper();

string secret = "Sensitive data";
string password = "StrongPassword!";

// Encrypt
string encrypted = aes.Encode(secret, password);

// Decrypt
string decrypted = aes.Decode(encrypted, password);

Console.WriteLine(decrypted); // Output: "Sensitive data"
```

---

## Error Handling

```csharp
try 
{
    string result = aes.Encode("text", "password");
}
catch (ArgumentNullException ex)
{
    Console.WriteLine("Input or key cannot be empty");
}
catch (Exception ex)
{
    Console.WriteLine($"Encryption failed: {ex.Message}");
}
```

---

## How It Works

### Encryption Process

1. Derives a 256-bit key from the password using SHA256
2. Generates a random IV (Initialization Vector)
3. Encrypts data using AES-CBC mode
4. Combines IV + ciphertext
5. Returns Base64 encoded string

### Decryption Process

1. Decodes Base64 input
2. Extracts IV from the first 16 bytes
3. Derives key from the password using SHA256
4. Decrypts the remaining ciphertext using AES
5. Returns the original plaintext

---

## Security Notes

- Use strong, complex passwords
- Store passwords securely (do not hardcode them)
- IV is automatically generated and included in output
- AES-256 provides strong encryption
- This library does not manage password security – ensure keys are stored safely

---

## Requirements

- .NET Framework / .NET Core
- System.Security.Cryptography namespace

---

## License

MIT License – see LICENSE for details.
