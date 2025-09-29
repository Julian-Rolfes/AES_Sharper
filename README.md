# AES_Sharper

**MIT License** – free to use in your projects.

A simple and efficient .NET library for AES-256 encryption with SHA256 key derivation.

---

## Features

- 🔒 AES-256 encryption  
- 🔑 SHA256 key derivation  
- 🚀 Easy to use  
- 🛡️ Secure by default  
- 📦 No external dependencies  

---

## Installation

1. Copy the `AES_Sharper.cs` file into your .NET project.  
2. Include the following namespaces:

```csharp
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

Quick Start

// Create instance
var aes = new AES_Sharper();

// Encrypt
string encrypted = aes.Encode("Hello World", "myPassword123");

// Decrypt
string decrypted = aes.Decode(encrypted, "myPassword123");

Console.WriteLine(decrypted); // Output: "Hello World"

API Reference
Constructor

var aes = new AES_Sharper();

Methods
Encode(string input, string key)

Encrypts a string using the provided key.

    Parameters:

        input – The text to encrypt

        key – The encryption password

    Returns: Base64 encoded encrypted string

Decode(string input, string key)

Decrypts a string using the provided key.

    Parameters:

        input – Base64 encoded encrypted string

        key – The decryption password

    Returns: Original plaintext string

Examples

var aes = new AES_Sharper();

string secret = "Sensitive data";
string password = "StrongPassword!";

// Encrypt
string encrypted = aes.Encode(secret, password);

// Decrypt
string decrypted = aes.Decode(encrypted, password);

Console.WriteLine(decrypted); // Output: "Sensitive data"

Error Handling

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

How It Works
Encryption Process

    Derives a 256-bit key from the password using SHA256

    Generates a random IV (Initialization Vector)

    Encrypts data using AES-CBC mode

    Combines IV + ciphertext

    Returns Base64 encoded string

Decryption Process

    Decodes Base64 input

    Extracts IV from the first 16 bytes

    Derives key from the password using SHA256

    Decrypts the remaining ciphertext using AES

    Returns the original plaintext

Security Notes

    Use strong, complex passwords

    Store passwords securely (do not hardcode)

    IV is automatically generated and included in output

    AES-256 provides strong encryption

    This library does not manage password security – ensure keys are stored safely

Requirements

    .NET Framework / .NET Core

    System.Security.Cryptography namespace

License

MIT License – see LICENSE
file for details.
