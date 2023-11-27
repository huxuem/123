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

    [Header("�ؿ�ש������")]
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

        //�ڳ��˵�һ�ؿ�ʼʱ���������ͣ���ˢ��TeleportManager������
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
        //��ֱͬ����������Ϊ0��������֡��������֡������Ч��
        QualitySettings.vSyncCount = 0;
        //������Ϸ֡��
        Application.targetFrameRate = 120;

        //�ѳ�ʼ��Ӳд������
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

        //ɾ��ǽ��֮��������
        for (int i = 0; i < WallParent.transform.childCount; i++)
        {
            Destroy(WallParent.transform.GetChild(i).gameObject);
        }


        Debug.Log("Count:"+BlockofLevel.BlockPos.Count);
        for (int i = 0; i < BlockofLevel.BlockPos.Count; i++)
        {
            //��������BlockPos�б�Ȼ����[2,2]��Ϊ��ʼ��ͽ���������������
            for (int j = BlockofLevel.BlockPos[i][0][0]; j <= BlockofLevel.BlockPos[i][1][0]; j++)
            {
                //����blockpos��i��Ԫ�ص�x
                for(int k = BlockofLevel.BlockPos[i][0][1]; k <= BlockofLevel.BlockPos[i][1][1]; k++)
                {
                    //Debug.Log("Pos:" + j + " "+k);
                    //����y
                    Instantiate(Block_Wall, new Vector3(j * 0.5f, k * 0.5f, 0), Quaternion.identity, WallParent.transform);
                }
            }

        }
    }

    //�������������ɷ����������ã�����������ɵ��߷���
    public GameObject GetRandomBlock()
    {
        ////�ٷֱ���ʽ
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
