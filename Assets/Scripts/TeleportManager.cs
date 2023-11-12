using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager instance;

    [HideInInspector]public Transform CometTransform;
    [HideInInspector]public Vector3 velocity;

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
        TelePort = GameManager.instance.TelePort;
        BossLeft = GameManager.instance.BossLeft;
        comet = GameManager.instance.comet;
        Debug.Log("curComet:" + comet.GetInstanceID());
    }


}
