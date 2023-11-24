using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_acc : Hitable
{
    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Comet"))
    //    {
    //        //Debug.Log("Acc");
    //        AudioManager.instance.HitAccAudio();
            //other.transform.GetComponent<Comet>().Item_Acc();
    //        Destroy(gameObject);
    //    }
        
    //}

    public override void OnHit(Vector3 curSpeed, int SpeedLevel, Vector3 normal, out Vector3 SpeedOutput)
    {
        AudioManager.instance.HitAccAudio();
        SpeedOutput = curSpeed + curSpeed.normalized;
        Destroy(gameObject);
    }

}
