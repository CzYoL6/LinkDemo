using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using System;

public class PostprocessManager : MonoBehaviour
{
    private LensDistortion ld;
    private ChromaticAberration ca;
    public PostProcessVolume volume;
    private Bloom bl;
    void Start()
    {
        volume.profile.TryGetSettings(out ld);
        volume.profile.TryGetSettings(out ca);
        volume.profile.TryGetSettings(out bl);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * 传送特效
     */
    public void trans(Action action)
    {
        ca.intensity.Override(.8f);
        DOTween.Sequence()
             .Append(DOTween.To(x => ld.intensity.Override(x), 0, -30f, 0.5f))
             
             .Join(DOTween.To(x => ca.intensity.Override(x), 0, 0.6f, 0.5f))
             .Append(DOTween.To(x => ld.intensity.Override(x), -30f, -60f, 0.05f))
             .Join(DOTween.To(x => ca.intensity.Override(x), 0.6f, 1f, 0.05f))
             .Append(DOTween.To(x => ld.intensity.Override(x), -60f, 0, 0.05f))
             .Join(DOTween.To(x => ca.intensity.Override(x), 1f, 0f, 0.05f))
             // .OnComplete(()=>ca.intensity.Override(.8f))
             .OnComplete(()=>action.Invoke());

    }

}
