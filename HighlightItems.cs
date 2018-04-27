using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class HighlightItems : MonoBehaviour
{

    public Transform FlashlightParticle;
    List<Transform> Particles = new List<Transform>();

    void Start()
    {

    }


    void Update()
    {
        // when turn off the flashlight, destroy every particle 
        if (!transform.parent.GetComponent<FlashlightController>().isOn)
        {
            if (Particles != null && Particles.Count > 0)
            {
                for (int i = Particles.Count - 1; i >= 0; i--)
                {
                    Transform t = Particles[i];
                    Particles.Remove(t);
                    Destroy(t.gameObject);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // when the cone colliders triggers an evidence, instantiate a particle on the position of the evidence, set the rotation to face up
        if (transform.parent.GetComponent<FlashlightController>().isOn)
        {
            if (other.gameObject.CompareTag("Evidence"))
            {
                Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                Transform particle = Instantiate(FlashlightParticle, other.transform.position, Quaternion.Euler(-90, 0, 0));
                particle.gameObject.name = other.gameObject.name + "Particle";
                Particles.Add(particle);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // when the cone collider exits an evidence, destroy the particle on it
        if (other.CompareTag("Evidence"))
        {
            if (GameObject.Find(other.name + "Particle"))
            {
                GameObject particle = GameObject.Find(other.name + "Particle");
                Particles.Remove(particle.transform);
                Destroy(particle);
            }


        }
    }
}
