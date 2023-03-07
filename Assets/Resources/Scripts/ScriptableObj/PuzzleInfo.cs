using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle", menuName = "PuzzleInfo")]
public class PuzzleInfo : ScriptableObject
{
    public List<string> rules;
    public string Init;
    public string Target;
    [Serializable]
    public class TextureTableElement{
        public char letter;
        public Texture2D texture;
    }
    public List<TextureTableElement> textureTable;
}
