using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class Underwater : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        // Future settings
        public Material material;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
        public Color color;
        public float FogDensity = 1;
        [Range(0, 1)]
        public float alpha;
        public float refraction = 0.1f;
        public Texture normalmap;
        public Vector4 UV = new Vector4(1, 1, 0.2f, 0.1f);
    }

    public Settings settings = new Settings();

    class Pass : ScriptableRenderPass
    {
        public Settings settings;
        private RTHandle source;
        RTHandle tempTexture;

        private string profilerTag;

        public void Setup(RTHandle source)
        {
            this.source = source;
        }

        public Pass(string profilerTag)
        {
            this.profilerTag = profilerTag;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            tempTexture = RTHandles.Alloc(cameraTextureDescriptor);
            ConfigureTarget(tempTexture);
            ConfigureClear(ClearFlag.All, Color.black);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
            cmd.Clear();

            try
            {
                // Set material properties
                settings.material.SetFloat("_FogDensity", settings.FogDensity);
                settings.material.SetFloat("_alpha", settings.alpha);
                settings.material.SetColor("_color", settings.color);
                settings.material.SetTexture("_NormalMap", settings.normalmap);
                settings.material.SetFloat("_refraction", settings.refraction);
                settings.material.SetVector("_normalUV", settings.UV);

                // Blit operations
                cmd.Blit(source, tempTexture);
                cmd.Blit(tempTexture, source, settings.material, 0);

                context.ExecuteCommandBuffer(cmd);
            }
            catch
            {
                Debug.LogError("Error");
            }

            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (tempTexture != null)
            {
                RTHandles.Release(tempTexture);
                tempTexture = null;
            }
        }
    }

    Pass pass;

    public override void Create()
    {
        pass = new Pass("Underwater Effects");
        name = "Underwater Effects";
        pass.settings = settings;
        pass.renderPassEvent = settings.renderPassEvent;
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        var cameraColorTargetHandle = renderer.cameraColorTargetHandle;
        pass.Setup(cameraColorTargetHandle);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}
