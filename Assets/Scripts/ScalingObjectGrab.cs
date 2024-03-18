using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ScalingObjectGrab : MonoBehaviour
{
    [SerializeField] Transform playerCam;
    [SerializeField] int scaleIterations;

    [Header("Grabbable settings")]
    [SerializeField] int layer;
    [SerializeField] string grappableTag;
    int grabLayerMask;

    int wallLayerMask;

    bool objectGrapped = false;
    Transform grappedTrans;
    float grabbedAOV;

    private void Start()
    {
        grabLayerMask = 1 << layer;
        wallLayerMask = ~((1 << layer) | (1 << 3));
    }

    private void Update()
    {
        if (objectGrapped)
        {
            Ray ray = new Ray(playerCam.position, playerCam.forward);
            RaycastHit rayHit;
            Physics.Raycast(ray, out rayHit, float.PositiveInfinity, wallLayerMask);

            Vector3 grabObjectLocation = rayHit.point;
            float grabObjectScale = 2 * (grabObjectLocation - playerCam.position).magnitude
                                      * Mathf.Tan(grabbedAOV / 2f);

            for(int i = 0; i < scaleIterations; i++)
            {
                RaycastHit floorRayHit;
                Vector3 floorRayDir = Quaternion.Euler(playerCam.right
                                                        * (Mathf.Rad2Deg * grabbedAOV)
                                                        / 2f)
                                        * playerCam.forward;
                Physics.Raycast(playerCam.position,
                                floorRayDir,
                                out floorRayHit,
                                float.PositiveInfinity,
                                wallLayerMask);

                if (floorRayHit.distance < rayHit.distance
                    && floorRayHit.transform.gameObject.tag == "Ground")
                {
                    grabObjectLocation = floorRayHit.point;
                    grabObjectScale = 2 * (grabObjectLocation - playerCam.position).magnitude
                                              * Mathf.Tan(grabbedAOV / 2f);
                    grabObjectLocation += floorRayHit.normal * (grabObjectScale / 2);
                    break;
                }
                RaycastHit sphereHit;
                Physics.SphereCast(playerCam.position,
                                grabObjectScale / 2,
                                playerCam.forward,
                                out sphereHit,
                                float.PositiveInfinity,
                                wallLayerMask);

                grabObjectLocation = playerCam.position + playerCam.forward * sphereHit.distance;
                grabObjectScale = 2 * (grabObjectLocation - playerCam.position).magnitude
                                              * Mathf.Tan(grabbedAOV / 2f);
            }

            grappedTrans.position = grabObjectLocation;
            grappedTrans.localScale = Vector3.one * grabObjectScale;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!objectGrapped) { GrabObject(); }
            else { ReleaseObject(); }
        }
    }

    void GrabObject()
    {
        Ray ray = new Ray(playerCam.position, playerCam.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.PositiveInfinity, grabLayerMask);
        if(hit.transform == null) { return; }

        objectGrapped = true;

        grappedTrans = hit.transform;
        grappedTrans.GetComponent<Rigidbody>().isKinematic = true;

        grabbedAOV = 2 * Mathf.Atan(grappedTrans.localScale.x
                                     / (2 * (hit.point - playerCam.position).magnitude));
    }

    void ReleaseObject()
    {
        grappedTrans.GetComponent<Rigidbody>().isKinematic = false;
        grappedTrans = null;
        objectGrapped = false;
    }
}
