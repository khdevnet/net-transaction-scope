using System.IO;

namespace NetTransactionScope.Library.Files
{
    public class FileStorage
    {
        public virtual void CreateFile(string currentPath, byte[] fileData)
        {
            if (!Exists(currentPath))
            {
                File.WriteAllBytes(currentPath, fileData);
            }
        }

        public virtual void Move(string from, string to)
        {
            File.Move(from, to);
        }

        public virtual bool Exists(string currentPath)
        {
            return File.Exists(currentPath);
        }

        public virtual void Delete(string path)
        {
            File.Delete(path);
        }
    }
}
