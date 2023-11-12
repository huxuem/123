using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(menuName = "MyScriptableObject/BlockofLevel ")]
public class BlockofLevel : ScriptableObject
{

    public List<ShowContent> blocks;
    [HideInInspector] public List<List<List<int>>> BlockPos = new List<List<List<int>>>();


    [Serializable]
    public class ShowContent
    {
        public List<content> con1;
    };
    [Serializable]
    public class content
    {
        public List<int> con2;
    }

    public void InitBlock()
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
        for(int i = 0; i < blocks.Count; i++)
        {
            // 创建一个新的二维列表用于存储当前 ShowContent 中的数据
            List<List<int>> blockData = new List<List<int>>();

            for (int j=0;j<2;j++)
            {
                // 创建一个新的一维列表用于存储当前 content 中的数据
                List<int> contentData = new List<int>();

                // 将当前 content 中的数据添加到 contentData 中
                contentData.AddRange(blocks[i].con1[j].con2);

                // 将 contentData 添加到 blockData 中
                blockData.Add(contentData);
            }
            BlockPos.Add(blockData);
        }
    }
}
