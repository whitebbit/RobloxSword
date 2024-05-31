#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using System.IO;

namespace GBGamesPlugin
{
    public class PostProcessBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            var buildPatch = report.summary.outputPath + "/index.html";

            if (File.Exists(buildPatch))
            {
                File.Delete(buildPatch);
            }
        }

        [PostProcessBuild]
        public static void ModifyBuildDo(BuildTarget target, string pathToBuiltProject)
        {
            ArchivingBuild.Archiving(pathToBuiltProject);
        }
    }
}
#endif
