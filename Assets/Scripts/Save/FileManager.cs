using System;
using System.IO;
using UnityEngine;

namespace HNC.Save
{
    public static class FileManager
    {
        public static bool WriteToFile(string fileName, string fileContents)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                File.WriteAllText(fullPath, fileContents);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool LoadFromFile(string fileName, out string result)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(fullPath))
            {
                File.WriteAllText(fullPath, "");
            }
            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                result = "";
                return false;
            }
        }

        public static bool MoveFile(string fileName, string newFileName)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);
            var newFullPath = Path.Combine(Application.persistentDataPath, newFileName);

            try
            {
                if (File.Exists(newFullPath))
                {
                    File.Delete(newFullPath);
                }

                if (!File.Exists(fullPath))
                {
                    return false;
                }

                File.Move(fullPath, newFullPath);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public static bool FileExists(string fileName) => File.Exists(Path.Combine(Application.persistentDataPath, fileName));
        public static bool FileIsEmpty(string fileName) => new FileInfo(Path.Combine(Application.persistentDataPath, fileName)).Length == 0;
    }
}