using System;
using System.Collections.Generic;
using System.Linq;

namespace CapDetector.Utilities
{
    public class FolderEnumerator
    {
        public int GetMaxFileName(string rootFolder)
        {
            var allFiles = System.IO.Directory.GetFiles(rootFolder, "*.jpg");
            List<int> numbers = new List<int>();
            foreach (var file in allFiles)
            {
                int number;
                var cleanName = System.IO.Path.GetFileNameWithoutExtension(file);

                if (int.TryParse(cleanName, out number))
                {
                    numbers.Add(number);
                }
            }

            return numbers.Max();
        }
    }
}
