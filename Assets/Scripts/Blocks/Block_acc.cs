using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_acc : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Comet"))
        {
            //Debug.Log("Acc");
            AudioManager.instance.HitAccAudio();
            other.transform.GetComponent<Comet>().Item_Acc();
            Destroy(gameObject);
        }
        
    }
}
