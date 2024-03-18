using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform portalCamTrans;
    [SerializeField] Transform playerCamTrans;
    [SerializeField] Transform otherPortal;
    [SerializeField] MeshRenderer meshRenderer;
    RenderTexture tempTexture;
    Camera portalCam;
    Camera mainCamera;
    [SerializeField] PortalPlayer player;

    private void Awake()
    {
        tempTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    }

    private void Start()
    {
        meshRenderer.material.mainTexture = tempTexture;
        portalCam = portalCamTrans.GetComponent<Camera>();
        mainCamera = playerCamTrans.GetComponent<Camera>();
        portalCam.targetTexture = tempTexture;
    }

    private void Update()
    {
        Vector3 newPos = transform.position - playerCamTrans.position;
        newPos = Quaternion.Euler(0, otherPortal.rotation.eulerAngles.y, 0) * newPos;
        newPos.y = -newPos.y;
        portalCamTrans.localPosition = newPos;
        portalCamTrans.localRotation = Quaternion.Euler(playerCamTrans.rotation.eulerAngles
                                                    - transform.rotation.eulerAngles);
        
        UpdateCameraProjectionMatrix();
    }

    void UpdateCameraProjectionMatrix()
    {
        Plane p = new Plane(-otherPortal.forward, otherPortal.position);
        Vector4 clipPlaneWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCam.worldToCameraMatrix)) * clipPlaneWorldSpace;

        var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCam.projectionMatrix = newMatrix;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        player.Teleport(transform, otherPortal);
        player.portalCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") { return; }
        player.portalCount--;
    }
}
