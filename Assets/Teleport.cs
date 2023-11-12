using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string targetSceneName; // 下一个场景的名称
    public bool IsHorizontal;
    private Scene targetScene; // 保存目标场景的引用

    private void Start()
    {
        //Scene curScene = SceneManager.GetActiveScene();
        //// 提前加载目标场景
        //SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
        //// 获取目标场景的引用
        //targetScene = SceneManager.GetSceneByName(targetSceneName);
        //// 禁用目标场景，确保不会显示在当前场景中
        //SceneManager.SetActiveScene(curScene);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter!");
        if (other.CompareTag("Comet"))
        {
            //暂存当前场景comet的pos和velocity，传给下一关的comet
            //Vector3 pos = TeleportManager.instance.comet.transform.position;
            //Vector3 velocity = TeleportManager.instance.comet.CurVelocity;
            // 切换到目标场景
            Debug.Log(targetSceneName);
            SceneManager.LoadScene(targetSceneName);

            ////重新获取场景里的几个引用，从新的gamemanager里拿进来
            //TeleportManager.instance.RefreshTeleport();

            ////把comet传送到屏幕另一边
            //TeleportManager.instance.comet.Teleport(pos,velocity,IsHorizontal);
        }
    }

}
