using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float NotAttractRange;
    public float AttarctForce;

    private void OnTriggerStay(Collider other)
    {
        if(other != null && (other.tag == "Block" || other.tag == "Boss"))
        {
            Vector3 dist = transform.position - other.transform.position;
            if (dist.magnitude > NotAttractRange)
            {
                other.GetComponent<Rigidbody>().AddForce(dist.normalized * AttarctForce);
            }
        }
    }
}
