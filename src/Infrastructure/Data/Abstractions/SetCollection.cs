using System.Reflection;
using App.InvoisysTest.Infrastructure.Attributes;
using App.InvoisysTest.Infrastructure.Entities.Base;
using Strategyo.Extensions.Conversions;

namespace App.InvoisysTest.Infrastructure.Data.Abstractions;

public abstract class SetCollection<TCollection> where TCollection : BaseCollection
{
    private readonly string _filePath;
    
    protected SetCollection()
    {
        var databasePatchAttribute = typeof(TCollection).GetCustomAttribute<DatabasePathAttribute>();
        
        if(databasePatchAttribute == null)
            throw new Exception("Database patch attribute could not be found");
        
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        
        var dataFolder = Path.Combine(basePath, "Data");
        
        var exists = File.Exists(dataFolder);

        if (!exists)
        {
            Directory.CreateDirectory(dataFolder);
        }
        
        var fullPath = Path.Combine(dataFolder, databasePatchAttribute.FilePath);
        
        _filePath = fullPath;
    }
    
    public async Task<List<TCollection>> GetCollectionAsync(CancellationToken ct = default)
    {
        var content = await File.ReadAllTextAsync(_filePath, ct);

        var entities = content.FromJson<List<TCollection>>();
        
        return entities ?? [];
    }
}