using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerGlobe : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform cam;
    CustomPhysics physics;

    private void Start()
    {
        physics = GetComponent<CustomPhysics>();
    }

    private void Update()
    {
        Vector2 kbInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 movement = transform.right * speed * Time.deltaTime * kbInput.x;
        movement += transform.forward * speed * Time.deltaTime * kbInput.y;
        transform.position += movement;

        //Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //transform.Rotate(transform.up * mouseInput.x * rotationSpeed);
        //cam.Rotate(cam.right * mouseInput.y * rotationSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && physics.isGrounded)
        {
            physics.AddForce(transform.up, jumpForce);
        }
    }
}
