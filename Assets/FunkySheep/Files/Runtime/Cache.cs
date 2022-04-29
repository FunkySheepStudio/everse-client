using System.IO;
using UnityEngine;

namespace FunkySheep.Files
{
    public static class Cache
    {
        public static byte[] Get(string id)
        {
            string filePath = Application.persistentDataPath + "/" + id;
            byte[] fileData = null;
            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
            }

            return fileData;
        }

        public static void Set(byte[] data, string id)
        {
            File.WriteAllBytes(Application.persistentDataPath + "/" + id, data);
        }

        public static void Remove(string id)
        {
            string filePath = Application.persistentDataPath + "/" + id;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static void RemoveAll()
        {
            foreach (string file in Directory.GetFiles(Application.persistentDataPath))
            {
                File.Delete(file);
            }
        }
    }
}