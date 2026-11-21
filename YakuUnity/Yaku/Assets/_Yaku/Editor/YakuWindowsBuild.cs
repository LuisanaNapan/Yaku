using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Yaku.Editor
{
    public static class YakuWindowsBuild
    {
        [MenuItem("YAKU/Compilar Windows x64")]
        public static void Build()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                throw new InvalidOperationException("Sal de Play Mode antes de compilar.");
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64))
                throw new InvalidOperationException("Instala Windows Build Support para esta versión de Unity.");

            YakuProjectValidation.Run();
            var projectSettings = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset");
            var input = new SerializedObject(projectSettings[0]).FindProperty("activeInputHandler");
            if (input == null || input.intValue == 0)
                throw new InvalidOperationException("El proyecto necesita Unity Input System.");

            string folder = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Builds", "YAKU-Windows"));
            Directory.CreateDirectory(folder);
            string executable = Path.Combine(folder, "YAKU.exe");
            // Keep the shared Build Settings and any other team scenes out of this build.
            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { YakuProjectValidation.CommunityScene },
                locationPathName = executable,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.StrictMode
            });

            if (report.summary.result != BuildResult.Succeeded || !File.Exists(executable))
                throw new InvalidOperationException($"YAKU Windows · {report.summary.result} · {report.summary.totalErrors} errores.");

            File.WriteAllText(Path.Combine(folder, "LEEME.txt"),
                "YAKU: Guardianes de la Vida\r\n\r\n" +
                "Abre YAKU.exe y elige Jugar. Conserva todos los archivos de esta carpeta junto al ejecutable.\r\n" +
                "WASD: mover | Shift: correr | Espacio: saltar | Ratón: cámara | E: interactuar\r\n" +
                "I: inventario | J: diario | Escape: pausa\r\n");
            Debug.Log($"YAKU_WINDOWS_BUILD_OK · {report.summary.totalErrors} errores · {report.summary.totalWarnings} avisos · {report.summary.totalSize} bytes · {executable}");
        }
    }
}
