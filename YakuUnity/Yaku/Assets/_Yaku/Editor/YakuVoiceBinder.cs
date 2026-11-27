using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Yaku.Dialogue;

public static class YakuVoiceBinder
{
    private const string DialogueFolder = "Assets/_Yaku/Narrative/Dialogues/";
    private const string VoiceFolder = "Assets/_Yaku/Audio/Voices/";

    // El orden se extrajo de Correspondencia_49_lineas.csv de los guiones Eleven v4.
    private static readonly Dictionary<string, string[]> Clips = new Dictionary<string, string[]>
    {
            { "ChapterClosing.asset", new[] { "Elena_21", "Elena_22", "Elena_23", "Elena_24", "Elena_25", "Elena_26" } },
            { "DistributionPlan.asset", new[] { "Elena_17", "Elena_18", "Elena_19", "Elena_20" } },
            { "Elena.asset", new[] { "Elena_01", "Elena_02", "Elena_03" } },
            { "FoodAdvisory.asset", new[] { "Avisos_05", "Avisos_06", "Avisos_07", "Avisos_08", "Avisos_09" } },
            { "FoodBriefing.asset", new[] { "Rosa_04", "Rosa_05", "Rosa_06", "Rosa_07" } },
            { "HealthGuidance.asset", new[] { "Elena_12", "Elena_13", "Elena_14", "Elena_15", "Elena_16" } },
            { "HealthIncident.asset", new[] { "Tomas_04", "Tomas_05", "Tomas_06", "Tomas_07" } },
            { "RiverMonitoring.asset", new[] { "Avisos_01", "Avisos_02", "Avisos_03", "Avisos_04" } },
            { "Rosa.asset", new[] { "Rosa_01", "Rosa_02", "Rosa_03" } },
            { "Tomas.asset", new[] { "Tomas_01", "Tomas_02", "Tomas_03" } },
            { "WaterBriefing.asset", new[] { "Elena_04", "Elena_05", "Elena_06", "Elena_07" } },
            { "WaterReport.asset", new[] { "Elena_08", "Elena_09", "Elena_10", "Elena_11" } },
    };

    [MenuItem("YAKU/Denis/Verificar y vincular 49 voces")]
    private static void BindVoices()
    {
        var pending = new List<Tuple<DialogueData, AudioClip[]>>();
        var errors = new List<string>();
        int clipCount = 0;

        foreach (var entry in Clips)
        {
            var data = AssetDatabase.LoadAssetAtPath<DialogueData>(DialogueFolder + entry.Key);
            if (data == null)
            {
                errors.Add("Falta DialogueData: " + entry.Key);
                continue;
            }
            if (data.LineCount != entry.Value.Length)
            {
                errors.Add(entry.Key + " tiene " + data.LineCount + " subtítulos; se esperaban " + entry.Value.Length +
                    ". Instala primero los 12 .asset de narrativa actualizados.");
                continue;
            }

            var clips = new AudioClip[entry.Value.Length];
            for (int i = 0; i < entry.Value.Length; i++)
            {
                string clipName = entry.Value[i];
                string voice = clipName.Substring(0, clipName.IndexOf('_'));
                string path = VoiceFolder + voice + "/" + clipName + ".wav";
                clips[i] = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                if (clips[i] == null)
                    errors.Add("Falta audio: " + path);
                else
                    clipCount++;
            }
            pending.Add(Tuple.Create(data, clips));
        }

        if (errors.Count != 0)
        {
            Debug.LogError("YAKU: NO se ha modificado ningun DialogueData. Problemas:\n" +
                string.Join("\n", errors));
            EditorUtility.DisplayDialog("YAKU: faltan archivos", errors.Count +
                " problemas. Revisa Console. No se ha guardado ningun cambio.", "Entendido");
            return;
        }

        foreach (var item in pending)
        {
            Undo.RecordObject(item.Item1, "Vincular voces del capitulo YAKU");
            var serialized = new SerializedObject(item.Item1);
            var property = serialized.FindProperty("voiceLines");
            if (property == null)
            {
                Debug.LogError("YAKU: falta el campo voiceLines. Revisa DialogueData.cs");
                return;
            }
            property.arraySize = item.Item2.Length;
            for (int i = 0; i < item.Item2.Length; i++)
                property.GetArrayElementAtIndex(i).objectReferenceValue = item.Item2[i];
            serialized.ApplyModifiedProperties();
            EditorUtility.SetDirty(item.Item1);
        }
        AssetDatabase.SaveAssets();
        Debug.Log("YAKU: " + clipCount + " voces vinculadas a " + pending.Count + " archivos de diálogo.");
        EditorUtility.DisplayDialog("YAKU: voces vinculadas",
            clipCount + " clips asignados a " + pending.Count + " DialogueData. Ejecuta Play y prueba la misión.",
            "Perfecto");
    }
}
