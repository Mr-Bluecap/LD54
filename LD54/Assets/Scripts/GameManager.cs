using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    float timeInSeconds = 120f;
    
    void Start()
    {
        GenerateMap();
        GenerateRequest();
        StartLevelTimer();
    }

    async void StartLevelTimer()
    {
        await Task.Delay((int)(timeInSeconds * 1000));
        
        Debug.LogError("LEVEL OVER");
    }

    void GenerateMap()
    {
        var levelLayouts = Resources.LoadAll<LevelLayoutData>("Level Layouts").ToList();

        var randomIndex = Random.Range(0, levelLayouts.Count);

        var levelLayout = levelLayouts[randomIndex];

        NodeManager.Instance.CreateNodesFromLevelLayout(levelLayout);
    }

    void GenerateRequest()
    {
        RequestGenerator.Instance.GenerateRequest();
    }
}
