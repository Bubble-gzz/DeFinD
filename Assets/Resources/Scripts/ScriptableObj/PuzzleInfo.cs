using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle", menuName = "PuzzleInfo")]
public class PuzzleInfo : ScriptableObject
{
    public string puzzleNickName;
    public List<string> rules;
    public string Init;
    [Serializable]
    public class TextureTableElement{
        public char letter;
        public Sprite texture;
    }
    [Serializable]
    public class TargetElement{
        public string s;
        public int count;
    }
    public List<TargetElement> Targets;
    public List<TextureTableElement> textureTable;
    public Sprite GetSprite(char ch)
    {
        foreach(var element in textureTable)
        {
            if (element.letter == ch)
                return element.texture;
        }
        return null;
    }
}
