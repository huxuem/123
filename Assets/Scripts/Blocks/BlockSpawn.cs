using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawn : Hitable
{
    [SerializeField] private GameObject ItemSpawn;
    //这类方块固定受1次撞击，撞完直接露出本体
    public override void OnHit(Vector3 curVelo, int SpeedLevel, float CometScale, Vector3 normal, out Vector3 SpeedOutput)
    {
        if (CometScale <= 1)
        {
            SpeedOutput = Vector3.Reflect(curVelo, normal);
        }
        else SpeedOutput = curVelo;

        Instantiate(ItemSpawn, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
