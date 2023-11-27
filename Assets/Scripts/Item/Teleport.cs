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
            //暂存当前场景comet的pos和velocity到TeleportManager里，传给下一关的comet
            //要改
            TeleportManager.instance.Pos_Pre = TeleportManager.instance.comet.transform.position;
            TeleportManager.instance.Velo_Pre = TeleportManager.instance.comet.CurVelocity;
            TeleportManager.instance.IsHorizontal = IsHorizontal;
            // 切换到目标场景
            Debug.Log(targetSceneName);
            SceneManager.LoadScene(targetSceneName);
        }
    }

}
