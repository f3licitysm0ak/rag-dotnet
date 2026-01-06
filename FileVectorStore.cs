using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace myProject
{
    public class FileVectorStore
    {
        private string fileStorePath; //readonly?

        public FileVectorStore(string storePath)
        {
            fileStorePath = storePath;

            //if directory doesn't exist, create one
            if(!Directory.Exists(storePath))
            {
                Directory.CreateDirectory(storePath);
                
            }
        }

        public void save(Dictionary<string, float[]> vector_db)
        {
            //CHECK THIS
            string filePath = Path.Combine(fileStorePath, "embeddings.json");
            string json = JsonConvert.SerializeObject(vector_db);
            File.WriteAllText(filePath, json);
        }

        public Dictionary<string, float[]> load()
        {
            string filePath = Path.Combine(fileStorePath, "embeddings.json");

            if(!File.Exists(filePath) || new FileInfo(filePath).Length ==0)
            {
                return new Dictionary<string, float[]>();

            }

            string json = File.ReadAllText(filePath);
            Dictionary<string, float[]> result = JsonConvert.DeserializeObject<Dictionary<string, float[]>>(json);
            return result;
        }



    }
}