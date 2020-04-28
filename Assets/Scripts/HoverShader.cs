using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverShader : MonoBehaviour
{
    [SerializeField] private Shader hoverShader;
    private Shader defaultShader;
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultShader = renderer.material.shader;
    }

    void OnMouseEnter()
    {
        renderer.material.shader = hoverShader;
    }

    void OnMouseExit()
    {
        renderer.material.shader = defaultShader;
    }
}
