using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamburger : MonoBehaviour
{
    Rigidbody rb;

    [Header("JumpPower")]
    public float power;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 velocity = rb.velocity;
                velocity.y = 0f;
                rb.velocity = velocity;

                rb.AddForce(Vector3.up * power, ForceMode.Impulse);
            }
        }
    }
}
