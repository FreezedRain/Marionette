using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]
    private Transform bodyTop;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform visualBody;
    [SerializeField]
    private VisualEffect snapEffect;

    public void Socket()
    {
        LeanTween.moveLocalY(bodyTop.gameObject, 0.34f, 0.25f).setEaseInBack();
        LeanTween.moveLocalY(head.gameObject, 0.96f, 0.25f).setEaseInBack();
        LeanTween.moveLocalY(visualBody.gameObject, -2.4f, 0.35f).setEaseInBack();
        LeanTween.delayedCall(gameObject, 0.1f, PlaySnapEffect);
    }

    public void Unsocket()
    {
        LeanTween.moveLocalY(bodyTop.gameObject, 0.62f, 0.15f).setEaseOutCubic();
        LeanTween.moveLocalY(head.gameObject, 1.12f, 0.15f).setEaseOutCubic();
        LeanTween.moveLocalY(visualBody.gameObject, -1.24f, 0.20f).setEaseOutCubic();
    }

    private void PlaySnapEffect()
    {
        snapEffect.Reinit();
        snapEffect.Play();
    }
}
