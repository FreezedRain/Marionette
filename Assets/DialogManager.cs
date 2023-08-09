using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{

    private static DialogManager _instance;
    public static DialogManager Instance { get { return _instance; } }

    private Queue<string> sentences;
    private string npcName;

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TextMeshProUGUI tmpName;
    [SerializeField]
    private TextMeshProUGUI tmpContent;

    public bool inDialog = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        sentences = new Queue<string>();
        panel.SetActive(false);
    }

    public bool StartDialog(Dialog dialog)
    {
        if (inDialog)
        {
            return DisplayNextSentence();
        }

        inDialog = true;
        
        sentences.Clear();
        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        npcName = dialog.name;

        return DisplayNextSentence();
    }

    public bool DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return false;
        } else
        {
            string sentence = sentences.Dequeue();

            tmpName.text = npcName;
            tmpContent.text = sentence;
            panel.SetActive(true);
            return true;
        }
    }

    private void EndDialog()
    {
        panel.SetActive(false);

        inDialog = false;
    }
}
