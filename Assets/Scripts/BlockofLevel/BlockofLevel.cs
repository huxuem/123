using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//����д����ʵ�������б�����л�
[System.Serializable]
public class Content
{
    public string content;
}

//�м���
[System.Serializable]
public class Contents
{
    public List<Content> contents;
}

[System.Serializable]
public class ShowContents : MonoBehaviour
{
    public List<Contents> test;
}


[CreateAssetMenu(menuName = "MyScriptableObject/BlockofLevel ")]
public class BlockofLevel : ScriptableObject
{

    [SerializeField] private ShowContents showContents = new ShowContents();
    [HideInInspector]public List<List<List<int>>> BlockPos = new List<List<List<int>>>();

    private void Awake()
    {
        //Ҫ�ӵ�ʱ����Ҫ��С���
        //BlockPos.Add(new List<List<int>>
        //    {
        //        new List<int> {-6,6},
        //        new List<int> {6,6}
        //    });
        //BlockPos.Add(new List<List<int>>
        //    {
        //        new List<int> {-6,-6},
        //        new List<int> {6,-6}
        //    });

        //���������Ǹ����췽�����л��������б������
        for(int i = 0; i < showContents.test.Count; i++)
        {
            for(int j=0;j<2;j++)
            {
                for(int k = 0; k < 2; k++)
                {
                    BlockPos[i][j][k] = showContents.test[i].contents[j].content[k];
                }
            }
        }
    }
}
