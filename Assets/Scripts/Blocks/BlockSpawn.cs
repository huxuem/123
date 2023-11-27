using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawn : Hitable
{
    [SerializeField] private List<GameObject> ItemSpawn;
    //���෽��̶���1��ײ����ײ��ֱ��¶������
    public override void OnHit(Vector3 curVelo, int SpeedLevel, float CometScale, Vector3 normal, out Vector3 SpeedOutput)
    {
        if (CometScale <= 1)
        {
            SpeedOutput = Vector3.Reflect(curVelo, normal);
        }
        else SpeedOutput = curVelo;

        Instantiate(ItemSpawn[Random.Range(0,ItemSpawn.Count+1)], transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
