using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] Transform targetObject;
    [SerializeField] LayerMask wallMask;
    [SerializeField] float cutoutRadius;

    Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector2 cutoutPos = cam.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;
            
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j].SetVector("_CutoffPos", cutoutPos);
                materials[j].SetFloat("_CutoffSize", cutoutRadius);
                materials[j].SetFloat("_FalloffSize", 0.05f);
            }
        }
    }
}
