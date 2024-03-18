using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float sensitivity;
    [SerializeField] Transform cam;
    bool cursorVisibility = true;
    bool paused = false;

    private void Start()
    {
        ChangeCursorState();
    }

    private void Update()
    {
        if (!paused)
        {
            Vector2 kbInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 movement = new Vector3(speed * kbInput.x * Time.deltaTime, 0,
                                            speed * kbInput.y * Time.deltaTime);
            transform.Translate(movement);

            Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector2 rotation = new Vector2(sensitivity * mouseInput.x * Time.deltaTime,
                                            -sensitivity * mouseInput.y * Time.deltaTime);
            transform.Rotate(new Vector3(0, rotation.x, 0));
            cam.Rotate(rotation.y, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { ChangeCursorState(); }
    }

    void ChangeCursorState()
    {
        if (cursorVisibility)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            paused = false;
            Time.timeScale = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
            Time.timeScale = 0;
        }
    }
}
