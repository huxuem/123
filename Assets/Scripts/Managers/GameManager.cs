using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject Block_Dynamic;
    [SerializeField] private GameObject Block_Spawn;
    //[SerializeField] private List<List<List<int>>> BlockPos = new List<List<List<int>>>();
    [SerializeField] private BlockofLevel BlockofLevel;


    private bool isStage1 = false;


    private void Awake()
    {
        instance = this;

        //在除了第一关开始时，触发传送，并刷新TeleportManager的引用
        Debug.Log(SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name != "StartScene")
        {
            comet.Teleport(TeleportManager.instance.Pos_Pre, TeleportManager.instance.Velo_Pre, TeleportManager.instance.IsHorizontal);
            TeleportManager.instance.RefreshTeleport();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //垂直同步计数设置为0，才能锁帧，否则锁帧代码无效。
        QualitySettings.vSyncCount = 0;
        //设置游戏帧数
        Application.targetFrameRate = 120;

        //把初始化硬写在这里
        LoadBlockPrefab();
        SpawnBlockPos();
        //StartCoroutine(SpawnWallCoroutine());
    }

    private void LoadBlockPrefab()
    {
        Block_Dynamic = Resources.Load<GameObject>("Prefabs/Block1");
        Block_Spawn = Resources.Load<GameObject>("Prefabs/BlockSpawn");
    }

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

    //给其他所有生成方块的物体调用，用于随机生成道具方块
    public GameObject GetRandomBlock()
    {
        ////百分比形式
        //int Ratio = 5;
        //if(UnityEngine.Random.Range(0, 100)< Ratio)
        //{
        //    return Block_Spawn;
        //}
        //else return Block_Dynamic;
        return Block_Dynamic;
    }


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
