using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle", menuName = "PuzzleInfo")]
public class PuzzleInfo : ScriptableObject
{
    public List<string> rules;
    public string Init;
    [Serializable]
    public class TextureTableElement{
        public char letter;
        public Texture2D texture;
    }
    [Serializable]
    public class TargetElement{
        public string s;
        public int count;
    }
    public List<TargetElement> Targets;
    public List<TextureTableElement> textureTable;
}
