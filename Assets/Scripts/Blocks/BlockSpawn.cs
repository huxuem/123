using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawn : Hitable
{
    [SerializeField] private GameObject ItemSpawn;
    //���෽��̶���1��ײ����ײ��ֱ��¶������
    public override void OnHit()
    {
        Instantiate(ItemSpawn, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
