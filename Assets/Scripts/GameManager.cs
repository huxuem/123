using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject BlockContainer;
    [SerializeField] private GameObject block;

    [SerializeField] private GameObject Wall_1;
    [SerializeField] private GameObject Wall_2;
    public GameObject TelePort;
    public GameObject Arrow;
    public GameObject EndText;
    public int BossLeft;
    public Comet comet;

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
        //垂直同步计数设置为0，才能锁帧，否则锁帧代码无效。
        QualitySettings.vSyncCount = 0;
        //设置游戏帧数
        Application.targetFrameRate = 120;

        //把初始化硬写在这里
        //InitPos();
        SpawnBlockPos();
        //StartCoroutine(SpawnWallCoroutine());
    }

    

    //IEnumerator SpawnWallCoroutine()
    //{
    //    while (true)
    //    {
    //        SpawnBlockPos();
    //        yield return new WaitForSeconds(SpawnCD);
    //    }
    //}

    private void SpawnBlockPos()
    {
        BlockofLevel.InitBlock();

        //删除墙，之后再生成
        for (int i = 0; i < WallParent.transform.childCount; i++)
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

    


    public void CheckTeleportAppear()
    {
        BossLeft -= 1;
        if (BossLeft == 0)
        {
            Debug.Log("NextLevel");
            TelePort.SetActive(false);
            Arrow.SetActive(true);
        }
    }

    public void SetEndText(bool flag)
    {
        EndText.SetActive(flag);
    }


}
