using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_acc : Hitable, Iinteractable
{
    public override void OnHit(Vector3 curSpeed, int SpeedLevel, float CometScale, Vector3 normal, out Vector3 SpeedOutput)
    {
        AudioManager.instance.HitAccAudio();
        //����Ч���Ͳ�����CoemtScale�仯��
        SpeedOutput = curSpeed + curSpeed.normalized;
        Destroy(gameObject);
    }

    //public void OnEnlarge(float Ratio)
    //{
    //    transform.localScale = transform.localScale * Ratio;
    //}

    //public void OnDiminish(float Ratio)
    //{
    //    transform.localScale = transform.localScale / Ratio;
    //}

}
