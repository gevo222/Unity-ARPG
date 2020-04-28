using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class DialogueInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject panel;

    public void OnInteraction(GameObject actor, GameObject target)
    {
        if (panel.active == false) {
            var text = target.GetComponent<Dialogue>()?.Text;
            if (text != null)
            {
                panel.SetActive(true);
                dialogueText.SetText($"<b>{target.name}</b>: {text}");
            }
        }
    }
}

