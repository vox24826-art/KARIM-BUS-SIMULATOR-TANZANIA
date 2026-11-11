using UnityEngine;

namespace KarimBusSimulator
{
    public enum GraphicsTier
    {
        Low,
        Medium,
        High,
        Ultra
    }

    public class GraphicsQualityManager : MonoBehaviour
    {
        public GraphicsTier currentTier = GraphicsTier.High;
        public bool autoRecommend = true;
        public bool applyDynamicResolution = true;
        public int targetFrameRate = 60;

        private void Start()
        {
            if (autoRecommend)
            {
                currentTier = RecommendTierForDevice();
            }

            ApplyTier(currentTier);
        }

        public void ApplyTier(GraphicsTier tier)
        {
            currentTier = tier;

            switch (tier)
            {
                case GraphicsTier.Low:
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = Mathf.Min(targetFrameRate, 30);
                    QualitySettings.shadowDistance = 18f;
                    QualitySettings.shadows = ShadowQuality.HardOnly;
                    QualitySettings.masterTextureLimit = 1;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    QualitySettings.lodBias = 0.8f;
                    QualitySettings.pixelLightCount = 2;
                    QualitySettings.skinWeights = SkinWeights.OneBone;
                    QualitySettings.SetQualityLevel(0, true);
                    break;

                case GraphicsTier.Medium:
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = Mathf.Min(targetFrameRate, 45);
                    QualitySettings.shadowDistance = 40f;
                    QualitySettings.shadows = ShadowQuality.HardOnly;
                    QualitySettings.masterTextureLimit = 0;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    QualitySettings.lodBias = 1.0f;
                    QualitySettings.pixelLightCount = 4;
                    QualitySettings.skinWeights = SkinWeights.TwoBones;
                    QualitySettings.SetQualityLevel(1, true);
                    break;

                case GraphicsTier.High:
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = Mathf.Min(targetFrameRate, 60);
                    QualitySettings.shadowDistance = 70f;
                    QualitySettings.shadows = ShadowQuality.All;
                    QualitySettings.masterTextureLimit = 0;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    QualitySettings.lodBias = 1.2f;
                    QualitySettings.pixelLightCount = 6;
                    QualitySettings.skinWeights = SkinWeights.FourBones;
                    QualitySettings.SetQualityLevel(2, true);
                    break;

                case GraphicsTier.Ultra:
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = Mathf.Min(targetFrameRate, 60);
                    QualitySettings.shadowDistance = 120f;
                    QualitySettings.shadows = ShadowQuality.All;
                    QualitySettings.masterTextureLimit = 0;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    QualitySettings.lodBias = 1.4f;
                    QualitySettings.pixelLightCount = 8;
                    QualitySettings.skinWeights = SkinWeights.FourBones;
                    QualitySettings.SetQualityLevel(3, true);
                    break;
            }

            if (applyDynamicResolution)
            {
                float scale = tier == GraphicsTier.Low ? 0.88f : tier == GraphicsTier.Medium ? 0.94f : tier == GraphicsTier.High ? 0.98f : 1f;
                QualitySettings.resolutionScalingFixedDPIFactor = scale;
            }
        }

        public GraphicsTier RecommendTierForDevice()
        {
            var ramMb = SystemInfo.systemMemorySize;
            var gpuName = SystemInfo.graphicsDeviceName.ToLower();

            if (ramMb <= 2048)
            {
                return GraphicsTier.Low;
            }

            if (ramMb <= 4096 && !gpuName.Contains("adreno") && !gpuName.Contains("mali") && !gpuName.Contains("powervr"))
            {
                return GraphicsTier.Medium;
            }

            if (ramMb <= 6144 || gpuName.Contains("adreno 7") || gpuName.Contains("mali-g7") || gpuName.Contains("powervr"))
            {
                return GraphicsTier.High;
            }

            return GraphicsTier.Ultra;
        }
    }
}
