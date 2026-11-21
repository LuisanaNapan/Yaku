using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Yaku.Cameras;
using Yaku.Characters;
using Yaku.Dialogue;
using Yaku.Interaction;
using Yaku.Inventory;
using Yaku.Player;
using Yaku.Quests;
using Yaku.UI;

namespace Yaku.Editor
{
    public static class YakuProjectValidation
    {
        public const string CommunityScene = "Assets/_Yaku/Scenes/01_Community.unity";

        [MenuItem("YAKU/Verificar escena y sistemas")]
        public static void RunFromMenu() => Debug.Log(string.Join("\n", Run()));

        public static string[] Run()
        {
            Scene scene = SceneManager.GetSceneByPath(CommunityScene);
            bool opened = !scene.isLoaded;
            if (opened) scene = EditorSceneManager.OpenPreviewScene(CommunityScene);

            var checks = new List<string>();
            void Check(bool condition, string message)
            {
                if (!condition) throw new InvalidOperationException(message);
                checks.Add(message);
            }

            try
            {
                var objects = scene.GetRootGameObjects()
                    .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                    .Select(item => item.gameObject).ToArray();
                Check(scene.IsValid() && objects.Length > 0, "La escena de la comunidad se abre.");
                Check(objects.All(go => GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go) == 0),
                    "No hay referencias a scripts ausentes.");

                T[] Find<T>() where T : Component => objects.SelectMany(go => go.GetComponents<T>()).ToArray();
                var rigs = Find<PlayerRig>();
                Check(rigs.Length == 1 && rigs[0].CameraTarget != null, "La cámara tiene un objetivo único.");
                var player = rigs[0].gameObject;
                var capsule = player.GetComponent<CharacterController>();
                Check(capsule != null && capsule.enabled && capsule.height >= capsule.radius * 2,
                    "El CharacterController tiene dimensiones y colisión válidas.");
                Check(player.GetComponent<PlayerMovement>() != null && player.GetComponentInChildren<CharacterLocomotionAnimator>() != null,
                    "Movimiento y animación están conectados.");
                Check(Find<Camera>().Count(camera => camera.isActiveAndEnabled && camera.CompareTag("MainCamera")) == 1 &&
                      Find<ThirdPersonCamera>().Length == 1 && Find<AudioListener>().Count(listener => listener.isActiveAndEnabled) == 1,
                    "Hay una cámara principal, una cámara de seguimiento y un único oyente de audio.");
                Check(Find<Light>().Any(light => light.isActiveAndEnabled && light.type == LightType.Directional && light.shadows != LightShadows.None) &&
                      GraphicsSettings.currentRenderPipeline != null, "La escena usa luz direccional con sombras y un render pipeline asignado.");

                var tutorial = player.GetComponent<TutorialQuest>();
                var chapter = player.GetComponent<CommunityChapter>();
                var distribution = player.GetComponent<CommunityDistribution>();
                Check(tutorial != null && chapter != null && player.GetComponent<ChapterJournal>() != null,
                    "El tutorial y el capítulo comparten el diario de la partida.");
                Check(player.TryGetComponent<PlayerInventory>(out _) && PlayerInventory.Capacity == 6 &&
                      player.GetComponent<InventoryHud>() != null, "Inventario y HUD de seis espacios.");
                Check(References(tutorial, "quest", "briefing", "supplies", "delivery", "recipient", "dockGate"),
                    "El tutorial tiene recursos, NPC y acceso a la ribera asignados.");

                var path = new SerializedObject(chapter).FindProperty("objectives");
                Check(path != null && path.isArray && path.arraySize == 16, "El capítulo contiene 16 objetivos ordenados.");
                for (int i = 0; i < path.arraySize; i++)
                {
                    var step = path.GetArrayElementAtIndex(i).objectReferenceValue as ChapterObjective;
                    Check(step != null && !string.IsNullOrWhiteSpace(step.Instruction) &&
                          (step.HasDecision ? step.Choices.Length >= 2 && step.CorrectChoice >= 0 && step.CorrectChoice < step.Choices.Length
                              : step.Distribution != null || step.Conversation != null && step.Conversation.LineCount > 0),
                        $"Objetivo {i + 1} configurado.");
                }

                var stops = new SerializedObject(distribution).FindProperty("stops");
                Check(stops != null && stops.arraySize == 3, "Tres destinatarios de distribución.");
                for (int i = 0; i < stops.arraySize; i++)
                {
                    var stop = stops.GetArrayElementAtIndex(i);
                    var pickup = stop.FindPropertyRelative("pickup").objectReferenceValue as SupplyPickup;
                    var delivery = stop.FindPropertyRelative("delivery").objectReferenceValue as SupplyDelivery;
                    Check(stop.FindPropertyRelative("recipient").objectReferenceValue is NpcDialogue &&
                          References(pickup, "item") && References(delivery, "requiredItem"),
                        $"Recogida y destinatario {i + 1} asignados.");
                    var supply = new SerializedObject(pickup);
                    var requirement = new SerializedObject(delivery);
                    Check(supply.FindProperty("item").objectReferenceValue == requirement.FindProperty("requiredItem").objectReferenceValue &&
                          supply.FindProperty("quantity").intValue > 0 &&
                          supply.FindProperty("quantity").intValue == requirement.FindProperty("requiredQuantity").intValue,
                        $"Tipo y cantidad de entrega {i + 1} coinciden.");
                }

                Check(Find<NpcDialogue>().Length >= 3 && Find<Collider>().Any(collider => !collider.isTrigger && collider.enabled) &&
                      References(player.GetComponent<InteractionDetector>(), "interactInput"),
                    "Personajes, colisionadores sólidos e interacción disponibles.");
                Check(player.GetComponent<GameMenu>() != null && player.GetComponent<CommunityStatus>() != null &&
                      References(player.GetComponent<GameAudio>(), "menuMusic", "communityMusic", "interact", "collect", "deliver", "complete"),
                    "Menú, HUD de estado y seis clips de audio preparados.");

                return checks.ToArray();
            }
            finally
            {
                if (opened && scene.IsValid()) EditorSceneManager.ClosePreviewScene(scene);
            }
        }

        private static bool References(UnityEngine.Object owner, params string[] fields)
        {
            if (owner == null) return false;
            var serialized = new SerializedObject(owner);
            return fields.All(field => serialized.FindProperty(field)?.objectReferenceValue != null);
        }
    }
}
