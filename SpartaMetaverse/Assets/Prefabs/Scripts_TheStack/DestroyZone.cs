using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheStack
{
    public class DestroyZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Rubble")
            {
                Destroy(collision.gameObject);
            }
        }

    }
}
