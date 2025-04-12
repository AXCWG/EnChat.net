using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace EnChat;

public static class Helper
{
    private static Assembly Asm = Assembly.GetExecutingAssembly();

    private static async Task<string> ReadResourceAsync(this Assembly assembly, string name)
    {
        // Determine path
        string resourcePath = name;
        // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
        if (!name.StartsWith(nameof(EnChat)))
        {
            resourcePath = assembly.GetManifestResourceNames()
                .Single(str => str.EndsWith(name));
        }

        using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
        using StreamReader reader = new(stream);
        return await reader.ReadToEndAsync();
    }

    public static async Task<string> ReadEmbeddedAssets(string name)
    {
        return await Asm.ReadResourceAsync(name); 
    }
    public static string ToSha256HexHashString(this string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        var hex = BitConverter.ToString(hash).Replace("-", "").ToLower();
        return hex;
    }
}