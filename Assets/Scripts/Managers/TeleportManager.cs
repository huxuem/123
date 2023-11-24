using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager instance;

    [HideInInspector]public Vector3 Pos_Pre;
    [HideInInspector]public Vector3 Velo_Pre;
    [HideInInspector] public bool IsHorizontal;

    [SerializeField] private GameObject TelePort;
    [SerializeField] private int BossLeft;
    public Comet comet;

    private void Awake()
    {
        instance= this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RefreshTeleport();
    }

    public void RefreshTeleport()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        TelePort = gameManager.TelePort;
        BossLeft = gameManager.BossLeft;
        comet = gameManager.comet;
        Debug.Log("curComet:" + comet.GetInstanceID());
    }


}
