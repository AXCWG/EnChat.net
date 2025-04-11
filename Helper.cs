using System.Reflection;

namespace EnChat;

public static class Helper
{
    public static Assembly Asm = Assembly.GetExecutingAssembly();
    public static async Task<string> ReadResourceAsync(this Assembly assembly, string name)
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
}