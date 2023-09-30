using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Layout Data", menuName = "Levels/Level Layout")]
public class LevelLayoutData : ScriptableObject
{
    [TextArea(1, 10)]
    public string LevelLayout;

    List<string> nodeList;
    
    int levelWidth;
    int levelHeight;

    public int LevelWidth => levelWidth;
    public int LevelHeight => levelHeight;

    void OnEnable()
    {
        NodeData();
    }
    
    void NodeData()
    {
        var inversedLevelLayout = LevelLayout;
        var splitInversedLevelLayout = inversedLevelLayout.Split("/");

        var cleanSplitInversedLevelLayout = new List<string>();
        
        foreach (var levelRow in splitInversedLevelLayout)
        {
            if (levelRow.Equals(string.Empty))
            {
                continue;
            }
            
            cleanSplitInversedLevelLayout.Add(levelRow);
        }
        
        var orderedLevelLayout = cleanSplitInversedLevelLayout.ToArray().Reverse();

        nodeList = new List<string>();

        var levelLayout = orderedLevelLayout as string[] ?? orderedLevelLayout.ToArray();
        levelHeight = levelLayout.Length;

        foreach (var levelRow in levelLayout)
        {
            var levelRowAsNodes = levelRow.Split(",");
            levelWidth = levelRowAsNodes.Length;

            foreach (var levelNode in levelRowAsNodes)
            {
                if (levelNode.Equals(string.Empty))
                {
                    continue;
                }
                
                var cleanLevelNode = levelNode.Replace("\n", "");
                nodeList.Add(cleanLevelNode);
            }
        }
    }

    public bool IsNodeIndexSpawnable(int nodeIndex)
    {
        return nodeList[nodeIndex] == "O";
    }
}