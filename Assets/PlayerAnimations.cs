using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private PlayerFace face;

    [SerializeField]
    private Transform bodyTop;

    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform neck;
    [SerializeField]
    private Transform visualBody;
    [SerializeField]
    private VisualEffect snapEffect;

    [SerializeField]
    private Transform bodyBottom;

    [SerializeField]
    private Transform reference;

    [SerializeField]
    private Transform hands;

    [SerializeField]
    private Transform hand1pos;
    [SerializeField]
    private Transform hand1;
    [SerializeField]
    private Transform hand2pos;
    [SerializeField]
    private Transform hand2;

    private Vector3 pf_reference_rotation = Vector3.zero;
    private Vector3 pf_hand1_pos = Vector3.zero;
    private Vector3 pf_hand2_pos = Vector3.zero;

    private void Start()
    {
        pf_hand1_pos = hand1pos.transform.position;
        pf_hand2_pos = hand2pos.transform.position;
    }

    public void Socket()
    {
        LeanTween.moveLocalY(bodyTop.gameObject, 0.40f, 0.25f).setEaseInBack();
        LeanTween.moveLocalY(neck.gameObject, 0.65f, 0.25f).setEaseInBack();
        LeanTween.moveLocalY(visualBody.gameObject, -2.4f, 0.35f).setEaseInBack();
        LeanTween.moveLocalY(hands.gameObject, 0.1f, 0.35f).setEaseInBack();
        LeanTween.delayedCall(gameObject, 0.1f, PlaySnapEffect);
        LeanTween.rotateLocal(bodyBottom.gameObject, new Vector3(90, 0, 0), 0.2f);

        SetFace(1, 0.25f);
    }

    public void Unsocket()
    {
        LeanTween.moveLocalY(bodyTop.gameObject, 0.69f, 0.15f).setEaseOutCubic();
        LeanTween.moveLocalY(neck.gameObject, 0.487f, 0.15f).setEaseOutCubic();
        LeanTween.moveLocalY(visualBody.gameObject, -1.24f, 0.20f).setEaseOutCubic();
        LeanTween.moveLocalY(hands.gameObject, 0f, 0.35f).setEaseOutCubic();

        SetFace(0, 0.05f);
    }

    private void PlaySnapEffect()
    {
        snapEffect.Reinit();
        snapEffect.Play();
    }

    private void SetFace(int index, float delay)
    {
        LeanTween.delayedCall(gameObject, delay, () =>
        {
            face.SetFace(index);
        });
    }

    private void FixedUpdate()
    {
        //BOTTOM BODY SPIN
        if (movement.IsActive())
        {

            bodyBottom.localEulerAngles -= new Vector3(0, reference.localEulerAngles.y - pf_reference_rotation.y, 0);
            bodyBottom.localEulerAngles += Vector3.up * Time.deltaTime * movement.GetSpeed() * 300;
        }

        //HEAD ROTATION DELAY
        head.localEulerAngles -= new Vector3(0, reference.localEulerAngles.y - pf_reference_rotation.y, 0);
        head.localRotation = Quaternion.Slerp(head.localRotation, Quaternion.identity, Time.deltaTime*10);

        if (movement.GetMovementState() != PlayerMovement.MOVEMENT_STATE.SOCKETED)
        {
            //HAND 1
            hand1.position -= (hand1.transform.position - pf_hand1_pos);
            

            //HAND 2
            hand2.position -= (hand2.transform.position - pf_hand2_pos);
            
        }

        hand1.position = Vector3.Slerp(hand1.position, hand1pos.transform.position, Time.deltaTime * 20);
        hand2.position = Vector3.Slerp(hand2.position, hand2pos.transform.position, Time.deltaTime * 20);

        pf_reference_rotation = reference.localEulerAngles;
        pf_hand1_pos = hand1.transform.position;
        pf_hand2_pos = hand2.transform.position;
    }
}
