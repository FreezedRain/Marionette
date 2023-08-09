using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : Interactable
{
    public Dialog dialog;

    public bool TriggerDialog()
    {
        return DialogManager.Instance.StartDialog(dialog);
    }

    public override bool Interact()
    {
        return TriggerDialog();
    }
}
