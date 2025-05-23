using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource audioSource;
    private Rigidbody rb;
    public float footstepThreshold; // rigidbody에서 움직이고 있는지 값을 받아올 수 있음
    public float footstepRate;
    private float footstepTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(rb.velocity.y) < 0.05f) // 땅에 붙어 있을때
        {
            if(rb.velocity.magnitude > footstepThreshold) // 움직이고 있을때
            {
                if(Time.time - footstepTime > footstepRate)
                {
                    footstepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
                }
            }
        }
    }
}
