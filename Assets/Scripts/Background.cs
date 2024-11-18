using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;

    private float offset;
    private Material backgroundMaterial;

    private void Start() 
    {
        backgroundMaterial= GetComponent<Renderer>().material;
    }
    private void FixedUpdate() 
    {
        offset += (scrollSpeed * Time.fixedDeltaTime) / 10f;
        backgroundMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    } 
}
