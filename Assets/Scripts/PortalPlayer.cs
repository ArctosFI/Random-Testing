using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPlayer : MonoBehaviour
{
    [HideInInspector] public bool teleported = false;
    [HideInInspector] public int portalCount = 0;

    private void Update()
    {
        if (portalCount <= 0)
        {
            teleported = false;
        }
    }

    public void Teleport(Transform inPortal, Transform outPortal)
    {
        if (teleported) { return; }
        teleported = true;


        Vector3 teleportMovement = outPortal.position - inPortal.position;
        transform.position += teleportMovement;

        float teleportRotation = outPortal.rotation.eulerAngles.y
                                    - inPortal.rotation.eulerAngles.y;
        transform.Rotate(0f, teleportRotation - 180f, 0f);
    }
}
