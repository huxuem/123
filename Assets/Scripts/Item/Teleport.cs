using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string targetSceneName; // ��һ������������

    public bool IsHorizontal;
    private Scene targetScene; // ����Ŀ�곡��������

    private void Start()
    {
        //Scene curScene = SceneManager.GetActiveScene();
        //// ��ǰ����Ŀ�곡��
        //SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
        //// ��ȡĿ�곡��������
        //targetScene = SceneManager.GetSceneByName(targetSceneName);
        //// ����Ŀ�곡����ȷ��������ʾ�ڵ�ǰ������
        //SceneManager.SetActiveScene(curScene);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter!");
        if (other.CompareTag("Comet"))
        {
            //�ݴ浱ǰ����comet��pos��velocity��TeleportManager�������һ�ص�comet
            //Ҫ��
            TeleportManager.instance.Pos_Pre = TeleportManager.instance.comet.transform.position;
            TeleportManager.instance.Velo_Pre = TeleportManager.instance.comet.CurVelocity;
            TeleportManager.instance.IsHorizontal = IsHorizontal;
            // �л���Ŀ�곡��
            Debug.Log(targetSceneName);
            SceneManager.LoadScene(targetSceneName);
        }
    }

}
