using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleEntrance : SmoothObject, ISelectable
{
    public string puzzleName;
    public void Hit()
    {
        Global.puzzleName = puzzleName;
        SceneManager.LoadScene("GamePlay");
    }

    public void SetHighlight(bool flag)
    {
        if (flag)
            targetSize = new Vector2(1.2f, 1.2f);
        else
            targetSize = new Vector2(1, 1);
    }

}
