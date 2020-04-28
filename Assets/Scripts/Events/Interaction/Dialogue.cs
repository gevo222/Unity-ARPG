using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour, Interactable
{
    [SerializeField, TextArea] private string text;
    public string Text => text;
}
