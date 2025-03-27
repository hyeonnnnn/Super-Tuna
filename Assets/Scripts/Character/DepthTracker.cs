using System;
using UnityEngine;

public class DepthTracker : MonoBehaviour
{
    private enum Layer
    {
        Thermocline = 1,
        MixedLayer = 2,
        DeepLayer = 3
    }

    [SerializeField] private int mixedLayerDepth;
    [SerializeField] private int deepLayerDepth;
    [SerializeField] private Transform tunaTransform;

    private Layer currentLayer = Layer.Thermocline;

    public static event Action<string> OnLayerChanged;

    private void Update()
    {
        UpdateCurrentLayer();
    }

    private void UpdateCurrentLayer()
    {
        float depth = tunaTransform.position.y;
        Layer newLayer = currentLayer;

        if (depth <= deepLayerDepth)
            newLayer = Layer.DeepLayer;
        else if (depth <= mixedLayerDepth)
            newLayer = Layer.MixedLayer;
        else
            newLayer = Layer.Thermocline;

        if (newLayer != currentLayer)
        {
            currentLayer = newLayer;
            string layerNameInKorean = newLayer switch
            {
                Layer.Thermocline => "수온약층",
                Layer.MixedLayer => "혼합층",
                Layer.DeepLayer => "심해층",
                _ => "알 수 없는 층"
            };
            OnLayerChanged?.Invoke(layerNameInKorean);
        }
    }
}
