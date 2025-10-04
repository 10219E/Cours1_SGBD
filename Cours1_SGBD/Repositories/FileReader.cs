using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Cours1_SGBD.Repositories
{

    public class GetFile
    {
        public static string GetFileContent(string filepath)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // Find resource name that ends with filepath
            var resource = assembly.GetManifestResourceNames().SingleOrDefault(str => str.EndsWith(filepath));

            if (resource == null)
            {
                throw new Exception($"File '{filepath}' not found as embedded resource.");
            }

            // Get the resource stream from assembly
            using var stream = assembly.GetManifestResourceStream(resource);
            if (stream == null)
            {
                throw new Exception($"Unable to load resource stream for '{resource}'.");
            }

            // Read the stream content
            using var reader = new System.IO.StreamReader(stream);
            return reader.ReadToEnd();
        }
    }

}
