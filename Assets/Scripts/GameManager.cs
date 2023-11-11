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
    [Header("关卡砖块数据")]
    [SerializeField] private int SpawnCD;
    [SerializeField] private GameObject WallParent;
    [SerializeField] private GameObject Block_Wall;
    //[SerializeField] private List<List<List<int>>> BlockPos = new List<List<List<int>>>();
    [SerializeField] private BlockofLevel BlockofLevel;

    private bool isStage1 = false;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //把初始化硬写在这里
        //InitPos();
        SpawnBlockPos();
        StartCoroutine(SpawnBlockCoroutine());
        //StartCoroutine(SpawnWallCoroutine());
    }

    //private void InitPos()
    //{
    //    //要加的时候需要先小后大
    //    //BlockPos.Add(new List<List<int>>
    //    //    {
    //    //        new List<int> {-6,6},
    //    //        new List<int> {6,6}
    //    //    });
    //    //BlockPos.Add(new List<List<int>>
    //    //    {
    //    //        new List<int> {-6,-6},
    //    //        new List<int> {6,-6}
    //    //    });
    //    BlockPos.Add(new List<List<int>>
    //        {
    //            new List<int> {-6,-5},
    //            new List<int> {-6,5
    //            }
    //        });
    //    BlockPos.Add(new List<List<int>>
    //        {
    //            new List<int> {6,-5},
    //            new List<int> {6,5}
    //        });
    //}

    IEnumerator SpawnWallCoroutine()
    {
        while (true)
        {
            SpawnBlockPos();
            yield return new WaitForSeconds(SpawnCD);
        }
    }

    private void SpawnBlockPos()
    {
        //删除墙，之后再生成
        for(int i = 0; i < WallParent.transform.childCount; i++)
        {
            Destroy(WallParent.transform.GetChild(i).gameObject);
        }


        Debug.Log("Count:"+BlockofLevel.BlockPos.Count);
        for (int i = 0; i < BlockofLevel.BlockPos.Count; i++)
        {
            //遍历整个BlockPos列表，然后以[2,2]作为开始点和结束点坐标进行填充
            for (int j = BlockofLevel.BlockPos[i][0][0]; j <= BlockofLevel.BlockPos[i][1][0]; j++)
            {
                //遍历blockpos第i个元素的x
                for(int k = BlockofLevel.BlockPos[i][0][1]; k <= BlockofLevel.BlockPos[i][1][1]; k++)
                {
                    //Debug.Log("Pos:" + j + " "+k);
                    //遍历y
                    Instantiate(Block_Wall, new Vector3(j * 0.5f, k * 0.5f, 0), Quaternion.identity, WallParent.transform);
                }
            }

        }
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

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Debug.Log("ChangeState");
    //        ChangeWallState();
    //        isStage1 = !isStage1;
    //    }
    //}

    IEnumerator SpawnBlockCoroutine()
    {
        while (true)
        {
            //先改成专门加加速块的
            //block = Instantiate(blocks[UnityEngine.Random.Range(0,blocks.Count)], Vector3.zero, Quaternion.identity, BlockContainer.transform);
            block = Instantiate(blocks[0], Vector3.zero, Quaternion.identity, BlockContainer.transform);

            block.GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0).normalized * SpawnSpeedRange;
            //Debug.Log("Spawn,"+block+" "+block.GetComponent<Rigidbody>().velocity);
            yield return new WaitForSeconds(10f);
        }
    }

}
