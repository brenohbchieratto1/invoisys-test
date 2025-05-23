using System.Reflection;
using App.InvoisysTest.Infrastructure.Attributes;
using App.InvoisysTest.Infrastructure.Data.Helpers;
using App.InvoisysTest.Infrastructure.Entities.Base;
using Strategyo.Extensions.Conversions;

namespace App.InvoisysTest.Infrastructure.Data.Abstractions;

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

            var decryptContent = encryptContent.Decrypt();

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
            
            var encryptContent = decryptContent?.Encrypt();
            
            await File.WriteAllTextAsync(_filePath, encryptContent, ct).ConfigureAwait(false);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}