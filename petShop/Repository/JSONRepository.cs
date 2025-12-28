using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class JsonRepository<T> : IRepository<T> where T : class
{
    private readonly string path;

    public JsonRepository(string filepath)
    {
        path = filepath;
        if (!File.Exists(path))
            File.WriteAllText(path, "[]");
    }

    public IEnumerable<T> GetAll()
    {
        try
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }
        catch
        {
            return new List<T>();
        }
    }

    public void Add(T item)
    {
        List<T> list = GetAll().ToList();
        list.Add(item);
        Save(list);
    }
    private void Save(List<T> list)
    {
        File.WriteAllText(path, JsonConvert.SerializeObject(list, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented));
    }
}
