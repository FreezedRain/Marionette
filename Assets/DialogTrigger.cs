using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : Interactable
{
    public Dialog dialog;

    public GameObject prompt;

    public bool TriggerDialog()
    {
        return DialogManager.Instance.StartDialog(dialog);
    }

    public override bool Interact()
    {
        return TriggerDialog();
    }

    public override void OnHover()
    {
        prompt.SetActive(true);
    }

    public override void OnUnhover()
    {
        prompt.SetActive(false);
    }
}
