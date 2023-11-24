using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Hitable
{
    [SerializeField] private float SpeedDecRatio = 0;

    public override void OnHit(Vector3 curVelo, int SpeedLevel, float CometScale, Vector3 normal, out Vector3 SpeedOutput)
    {
        SpeedOutput = Vector3.Reflect(curVelo, normal);
        AudioManager.instance.HitWallAudio();
    }

}
