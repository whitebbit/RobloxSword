using System.IO;
using System.IO.Compression;

namespace GBGamesPlugin
{
    public static class ArchivingBuild
    {
        public static void Archiving(string pathToBuiltProject)
        {
            var number = "";

            if (!File.Exists(pathToBuiltProject + ".zip"))
            {
                Do();
            }
            else
            {
                for (var i = 1; i < 100; i++)
                {
                    if (File.Exists(pathToBuiltProject + "_" + i + ".zip")) continue;
                    number = "_" + i;
                    Do();
                    break;
                }
            }

            void Do()
            {
                ZipFile.CreateFromDirectory(pathToBuiltProject, pathToBuiltProject + number + ".zip");
            }
        }
    }
}