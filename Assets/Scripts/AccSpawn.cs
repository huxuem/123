using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccSpawn : MonoBehaviour
{
    public Block_acc acc;
    private Block_acc block;
    [SerializeField] private GameObject Container;
    [SerializeField] private float SpawnCD = 10f;
    [SerializeField] private float SpawnSpeedRange = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBlockCoroutine());
    }

    IEnumerator SpawnBlockCoroutine()
    {
        while(true)
        {
            //先改成专门加加速块的
            //block = Instantiate(blocks[UnityEngine.Random.Range(0,blocks.Count)], Vector3.zero, Quaternion.identity, BlockContainer.transform);
            block = Instantiate(acc, transform.position, Quaternion.identity, Container.transform);

            block.GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0).normalized * SpawnSpeedRange;
            //Debug.Log("Spawn,"+block+" "+block.GetComponent<Rigidbody>().velocity);
            yield return new WaitForSeconds(SpawnCD);
        }
    }
}
