using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;

namespace WaterSystem
{
    public class WaterSystemFeature : ScriptableRendererFeature
    {
        #region Water Effects Pass

        class WaterFxPass : ScriptableRenderPass
        {
            private const string k_RenderWaterFXTag = "Render Water FX";
            private readonly ShaderTagId m_WaterFXShaderTag = new ShaderTagId("WaterFX");
            private readonly Color m_ClearColor = new Color(0.0f, 0.5f, 0.5f, 0.5f);
            private FilteringSettings m_FilteringSettings;

            public WaterFxPass()
            {
                m_FilteringSettings = new FilteringSettings(RenderQueueRange.transparent);
            }

            private class PassData
            {
                public RendererListHandle rendererListHandle;
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                UniversalRenderingData renderingData = frameData.Get<UniversalRenderingData>();
                UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
                UniversalLightData lightData = frameData.Get<UniversalLightData>();

                RenderTextureDescriptor desc = cameraData.cameraTargetDescriptor;
                desc.depthBufferBits = 0;
                desc.width /= 2;
                desc.height /= 2;
                desc.colorFormat = RenderTextureFormat.Default;

                TextureHandle waterFXTexture = UniversalRenderer.CreateRenderGraphTexture(
                    renderGraph, desc, "_WaterFXMap", true, FilterMode.Bilinear, TextureWrapMode.Clamp);

                using (var builder = renderGraph.AddRasterRenderPass<PassData>(k_RenderWaterFXTag, out var passData))
                {
                    var drawSettings = RenderingUtils.CreateDrawingSettings(
                        m_WaterFXShaderTag, renderingData, cameraData, lightData, SortingCriteria.CommonTransparent);

                    var param = new RendererListParams(renderingData.cullResults, drawSettings, m_FilteringSettings);
                    passData.rendererListHandle = renderGraph.CreateRendererList(param);

                    builder.UseRendererList(passData.rendererListHandle);
                    builder.SetRenderAttachment(waterFXTexture, 0, AccessFlags.Write);
                    builder.SetGlobalTextureAfterPass(waterFXTexture, Shader.PropertyToID("_WaterFXMap"));

                    builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
                    {
                        context.cmd.ClearRenderTarget(false, true, m_ClearColor);
                        context.cmd.DrawRendererList(data.rendererListHandle);
                    });
                }
            }
        }

        #endregion

        #region Caustics Pass

        class WaterCausticsPass : ScriptableRenderPass
        {
            private const string k_RenderWaterCausticsTag = "Render Water Caustics";
            public Material WaterCausticMaterial;
            private static Mesh m_mesh;

            private class PassData
            {
                public Material material;
                public Mesh mesh;
                public Matrix4x4 matrix;
                public Matrix4x4 sunMatrix;
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

                var cam = cameraData.camera;
                if (cam.cameraType == CameraType.Preview || !WaterCausticMaterial)
                    return;

                if (!m_mesh)
                    m_mesh = GenerateCausticsMesh(1000f);

                using (var builder = renderGraph.AddRasterRenderPass<PassData>(k_RenderWaterCausticsTag, out var passData))
                {
                    passData.material = WaterCausticMaterial;
                    passData.mesh = m_mesh;

                    passData.sunMatrix = RenderSettings.sun != null
                        ? RenderSettings.sun.transform.localToWorldMatrix
                        : Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-45f, 45f, 0f), Vector3.one);

                    var position = cam.transform.position;
                    position.y = 0;
                    passData.matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);

                    builder.SetRenderAttachment(resourceData.activeColorTexture, 0, AccessFlags.Write);
                    builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture, AccessFlags.Read);

                    builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
                    {
                        data.material.SetMatrix("_MainLightDir", data.sunMatrix);
                        context.cmd.DrawMesh(data.mesh, data.matrix, data.material, 0, 0);
                    });
                }
            }
        }

        #endregion

        WaterFxPass m_WaterFxPass;
        WaterCausticsPass m_CausticsPass;

        public WaterSystemSettings settings = new WaterSystemSettings();
        [HideInInspector][SerializeField] private Shader causticShader;
        [HideInInspector][SerializeField] private Texture2D causticTexture;

        private Material _causticMaterial;

        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int Size = Shader.PropertyToID("_Size");
        private static readonly int CausticTexture = Shader.PropertyToID("_CausticMap");

        public override void Create()
        {
            m_WaterFxPass = new WaterFxPass { renderPassEvent = RenderPassEvent.BeforeRenderingOpaques };
            m_CausticsPass = new WaterCausticsPass();

            causticShader = causticShader ? causticShader : Shader.Find("Hidden/BoatAttack/Caustics");
            if (causticShader == null) return;
            if (_causticMaterial)
            {
                DestroyImmediate(_causticMaterial);
            }
            _causticMaterial = CoreUtils.CreateEngineMaterial(causticShader);
            _causticMaterial.SetFloat("_BlendDistance", settings.causticBlendDistance);

            if (causticTexture == null)
            {
#if UNITY_EDITOR
                causticTexture = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.verasl.water-system/Textures/WaterSurface_single.tif");
#endif
            }
            _causticMaterial.SetTexture(CausticTexture, causticTexture);

            switch (settings.debug)
            {
                case WaterSystemSettings.DebugMode.Caustics:
                    _causticMaterial.SetFloat(SrcBlend, 1f);
                    _causticMaterial.SetFloat(DstBlend, 0f);
                    _causticMaterial.EnableKeyword("_DEBUG");
                    m_CausticsPass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
                    break;
                case WaterSystemSettings.DebugMode.WaterEffects:
                    break;
                case WaterSystemSettings.DebugMode.Disabled:
                    _causticMaterial.SetFloat(SrcBlend, 2f);
                    _causticMaterial.SetFloat(DstBlend, 0f);
                    _causticMaterial.DisableKeyword("_DEBUG");
                    m_CausticsPass.renderPassEvent = RenderPassEvent.AfterRenderingSkybox + 1;
                    break;
            }

            _causticMaterial.SetFloat(Size, settings.causticScale);
            m_CausticsPass.WaterCausticMaterial = _causticMaterial;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (m_WaterFxPass != null) renderer.EnqueuePass(m_WaterFxPass);
            if (m_CausticsPass != null && m_CausticsPass.WaterCausticMaterial != null) renderer.EnqueuePass(m_CausticsPass);
        }

        private static Mesh GenerateCausticsMesh(float size)
        {
            var m = new Mesh();
            size *= 0.5f;

            var verts = new[]
            {
                new Vector3(-size, 0f, -size),
                new Vector3(size, 0f, -size),
                new Vector3(-size, 0f, size),
                new Vector3(size, 0f, size)
            };
            m.vertices = verts;

            var tris = new[]
            {
                0, 2, 1,
                2, 3, 1
            };
            m.triangles = tris;

            return m;
        }

        [System.Serializable]
        public class WaterSystemSettings
        {
            [Header("Caustics Settings")]
            [Range(0.1f, 1f)]
            public float causticScale = 0.25f;

            public float causticBlendDistance = 3f;

            [Header("Advanced Settings")] public DebugMode debug = DebugMode.Disabled;

            public enum DebugMode
            {
                Disabled,
                WaterEffects,
                Caustics
            }
        }
    }
}