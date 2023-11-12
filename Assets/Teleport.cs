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
            //�ݴ浱ǰ����comet��pos��velocity��������һ�ص�comet
            //Vector3 pos = TeleportManager.instance.comet.transform.position;
            //Vector3 velocity = TeleportManager.instance.comet.CurVelocity;
            // �л���Ŀ�곡��
            Debug.Log(targetSceneName);
            SceneManager.LoadScene(targetSceneName);

            ////���»�ȡ������ļ������ã����µ�gamemanager���ý���
            //TeleportManager.instance.RefreshTeleport();

            ////��comet���͵���Ļ��һ��
            //TeleportManager.instance.comet.Teleport(pos,velocity,IsHorizontal);
        }
    }

}
