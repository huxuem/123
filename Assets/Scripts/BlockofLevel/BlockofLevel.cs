using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//逆天写法，实现三层列表的序列化
[System.Serializable]
public class Content
{
    public string content;
}

//中间类
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
        //要加的时候需要先小后大
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

        //把用上面那个逆天方法序列化的三重列表翻译过来
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
