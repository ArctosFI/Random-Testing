using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleRemover : MonoBehaviour
{
    Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetVector("_CutoffPos", new Vector2(-5,-5));
    }
}
