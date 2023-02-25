using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace Application.Services
{
    public class FileService 
    {
        public void SerializeObjectIntoFile(object obj, string path)
        {
            using (FileStream fs = File.Create(path))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    using (JsonTextWriter jw = new JsonTextWriter(sw))
                    {
                        jw.Formatting = Formatting.Indented;
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jw, obj);
                    }
                }
            }
        }
    }
}
