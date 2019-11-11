using System.IO;

namespace NetTransactionScope.Library.Files
{
    public class FileStorage
    {
        public virtual void CreateFile(string currentPath, byte[] fileData)
        {
            if (!File.Exists(currentPath))
            {
                File.WriteAllBytes(currentPath, fileData);
                File.SetAttributes(currentPath, FileAttributes.Hidden);
            }
        }
    }
}
