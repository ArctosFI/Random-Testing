using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    [Header("Physics settings")]
    [SerializeField] float gravity;
    [SerializeField] Transform planet;
    float planetRadius;
    Vector3 velocity = new Vector3(0,0,0);
    public bool isGrounded { get; private set; }
    float gravitySpeed = 0;
    float terminalVelocity;

    [Header("Object settings")]
    [SerializeField] float mass;
    [SerializeField] float drag;
    [SerializeField] Transform feet;
    float objectHeight;

    private void Awake()
    {
        terminalVelocity = (mass * gravity) / drag;
        planetRadius = planet.localScale.x / 2;
        objectHeight = Vector3.Distance(transform.position, feet.position);
        gravitySpeed = gravity;
    }

    private void FixedUpdate()
    {
        Vector3 gravityDirection = (planet.position - transform.position);
        gravityDirection.Normalize();

        transform.up = -gravityDirection;

        if (isGrounded)
        {
            transform.position = -gravityDirection * (objectHeight + planetRadius);
        }
        else
        {
            velocity += gravityDirection * gravitySpeed;
            velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude, 0f, terminalVelocity);
        }

        velocity -= velocity * drag;
        transform.position += velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            isGrounded = true;
            gravitySpeed = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = false;
            gravitySpeed = gravity;
        }
    }

    public void AddForce(Vector3 direction, float force)
    {
        velocity += force * direction;
        isGrounded = false;
    }
}
