using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField]
    float timeInSeconds = 120f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateNewSequence();
    }

    public void CreateNewSequence()
    {
        RoomLineManager.Instance.DestroyAllLines();
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

        var centrePosition = new Vector2((NodeManager.Instance.Width - 1f) / 2f, (NodeManager.Instance.Height - 1f) / 2f);
        Camera.main.transform.position = new Vector3(centrePosition.x, centrePosition.y, Camera.main.transform.position.z);
    }

    void GenerateRequest()
    {
        RequestManager.Instance.GenerateRequest();
    }
}
