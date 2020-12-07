using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAndAway : MonoBehaviour
{
    public GameObject SmokePuff;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        Transform thisSpot = this.GetComponent<Transform>();
        Debug.Log(thisSpot);
        Instantiate(SmokePuff, thisSpot);
        Destroy(this);

    }
    void OnTriggerEnter(Collider other)
    {
        Transform thisSpot = this.GetComponent<Transform>();
        Debug.Log(thisSpot);
        Instantiate(SmokePuff, thisSpot);
        Destroy(this);
    }
}
