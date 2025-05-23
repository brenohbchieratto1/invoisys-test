using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using App.InvoiSysTest.Infrastructure.Attributes;
using App.InvoiSysTest.Infrastructure.Data.Helpers;
using App.InvoiSysTest.Infrastructure.Entities.Base;
using Strategyo.Extensions.Conversions;

namespace App.InvoiSysTest.Infrastructure.Data.Abstractions;

[ExcludeFromCodeCoverage]
public abstract class SetCollection<TCollection> where TCollection : BaseCollection
{
    private readonly string _filePath;
    
    protected SetCollection()
    {
        var databasePatchAttribute = typeof(TCollection).GetCustomAttribute<DatabasePathAttribute>();
    
        if (databasePatchAttribute == null)
            throw new Exception("DatabasePathAttribute could not be found");

        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var dataFolder = Path.Combine(basePath, "Data");
        
        Directory.CreateDirectory(dataFolder);

        var fullPath = Path.Combine(dataFolder, databasePatchAttribute.FilePath);
        
        if (!File.Exists(fullPath))
            File.WriteAllText(fullPath, "");

        _filePath = fullPath;
    }
    
    protected async Task<List<TCollection>> GetCollectionAsync(CancellationToken ct = default)
    {
        try
        {
            var encryptContent = await File.ReadAllTextAsync(_filePath, ct).ConfigureAwait(false);

            var decryptContent = await encryptContent.DecryptAsync().ConfigureAwait(false);

            var entities = decryptContent.FromJson<List<TCollection>>();

            return entities ?? [];
        }
        catch (Exception)
        {
            return [];
        }
    }

    protected async Task<bool> SaveChangesAsync(List<TCollection> values, CancellationToken ct = default)
    {
        try
        {
            var decryptContent = values.ToJson();
            
            var encryptContent = await decryptContent!.EncryptAsync().ConfigureAwait(false);
            
            await File.WriteAllTextAsync(_filePath, encryptContent, ct).ConfigureAwait(false);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}