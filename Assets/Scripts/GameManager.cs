using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject BlockContainer;
    [SerializeField] private float SpawnSpeedRange;
    [SerializeField] private List<GameObject> blocks;
    [SerializeField] private GameObject block;

    [SerializeField] private GameObject Wall_1;
    [SerializeField] private GameObject Wall_2;

    private bool isStage1 = false;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(SpawnBlockCoroutine());
    }

    private void ChangeWallState()
    {
        if (isStage1)
        {
            Wall_1.SetActive(true);
            Wall_2.SetActive(false);
        }
        else
        {
            Wall_1.SetActive(false);
            Wall_2.SetActive(true);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("ChangeState");
            ChangeWallState();
            isStage1 = !isStage1;
        }
    }

    IEnumerator SpawnBlockCoroutine()
    {
        while (true)
        {
            //Debug.Log(blocks.Count);
            block = Instantiate(blocks[UnityEngine.Random.Range(0, blocks.Count)], Vector3.zero, Quaternion.identity, BlockContainer.transform);
            block.GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0).normalized * SpawnSpeedRange;
            //Debug.Log("Spawn,"+block+" "+block.GetComponent<Rigidbody>().velocity);
            yield return new WaitForSeconds(2f);
        }
    }

}
