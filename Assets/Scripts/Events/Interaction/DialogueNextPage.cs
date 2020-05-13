using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogueNextPage : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    void Start()
    {
        dialogueText = GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(dialogueText);
    }

    void OnEnable()
    {
        StartCoroutine(FlipPages());
    }

    IEnumerator FlipPages()
    {
        dialogueText.ForceMeshUpdate();
        var maxPage = dialogueText.textInfo.pageCount;
        for (int page = 1; page <= maxPage; page++)
        {
            dialogueText.pageToDisplay = page;
            yield return new WaitForSeconds(2.5f);
        }
        dialogueText.SetText("");
        panel.SetActive(false);
    }
}
