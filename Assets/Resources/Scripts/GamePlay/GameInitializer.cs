using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    public string puzzleName;
    public GameObject gameCardPrefab;
    void Awake()
    {
        Global.puzzleName = puzzleName;
        Global.puzzleInfo = Resources.Load<PuzzleInfo>("PuzzleData/" + puzzleName);
        Global.gameCardPrefab = gameCardPrefab;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
