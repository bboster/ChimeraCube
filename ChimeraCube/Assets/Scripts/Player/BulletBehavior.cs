using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField]
    float damage = 1;

    [SerializeField]
    ParticleSystem particles;

    [SerializeField]
    Renderer bulletRenderer;

    [SerializeField]
    Collider col;

    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Chimera")
        {
            collision.collider.GetComponent<Health>().Damage(damage);
            particles.Play();
            Destroy(gameObject);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AreaDestroyer"))
        {
            Damage(other);
        }
        else if (other.CompareTag("Chimera"))
        {
            Damage(other);
        }
    }

    private void Damage(Collider other)
    {
        Arm arm = other.GetComponentInParent<Arm>();

        //Debug.Log("Hit " + health.name + "!");
        arm.Damage(damage);
        particles.Play();
        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        bulletRenderer.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
