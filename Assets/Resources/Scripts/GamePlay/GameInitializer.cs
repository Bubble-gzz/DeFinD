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
        Utils.KeyInitialize();
    }
    void Start()
    {
        PlayStory();
    }
    public void LoadPuzzle(string puzzleName)
    {
        if (puzzleName == "") puzzleName = Global.puzzleName;
        Global.puzzleInfo = Resources.Load<PuzzleInfo>("PuzzleData/" + puzzleName);
        Global.puzzleComplete = false;
    }
    void PlayStory(string puzzleName = "")
    {
        if (puzzleName != "") this.puzzleName = puzzleName;
        else puzzleName = this.puzzleName;
        if (puzzleName == "1-1") StartCoroutine(P1_1());
    }
    IEnumerator P1_1()
    {
        DialogBox dialog = DialogBox.CreateDialogBox(new Vector2(0, 350), new Vector2(0.5f, 1));
        yield return StartCoroutine(dialog.ShowMessage("Press [Space] or [DownArrow] to pick the card...", false));
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)) break;
            yield return null;
        }
        yield return StartCoroutine(dialog.ShowMessage("Good Job!!"));
        yield return StartCoroutine(dialog.ShowMessage("Now press [Enter] to perform the magic!", false));
        
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return)) break;
            yield return null;
        }
        yield return StartCoroutine(dialog.ShowMessage("Great! We have the desired candy now."));
        yield break;
    }
}
