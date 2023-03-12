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
        Global.gameCardPrefab = gameCardPrefab;
        LoadPuzzle(puzzleName);
    }
    void Start()
    {
        
    }
    public void LoadPuzzle(string puzzleName)
    {
        Global.puzzleName = puzzleName;
        Global.puzzleInfo = Resources.Load<PuzzleInfo>("PuzzleData/" + puzzleName);
        Global.puzzleComplete = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
