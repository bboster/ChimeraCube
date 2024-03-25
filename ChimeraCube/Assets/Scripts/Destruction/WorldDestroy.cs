using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class WorldDestroy : MonoBehaviour
{
    [SerializeField] float destructionDelay;
    [SerializeField] private Renderer floorQue;

    [SerializeField] GameObject otherEnd;

    // Triggers when hand contacts the "end zone"
    private void OnTriggerEnter(Collider other)
    {
        // When hand contacts / its tag call this function
        if(other.gameObject.CompareTag("AreaDestroyer"))
        {
            Debug.Log("I AM WORKING");
            StartCoroutine(goodbyeFloor());
            Debug.Log("This is the end");
        }
    }

    private IEnumerator goodbyeFloor()
    {
        floorQue.material.color = Color.yellow;
        yield return new WaitForSeconds(destructionDelay);
        Destroy(gameObject);
        Destroy(otherEnd);
    }
}
