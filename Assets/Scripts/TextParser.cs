using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class TextParser : MonoBehaviour
{
    //MAKE TEXT FILES AS THESE NAMES

    public List<string> FrogNames;

    public List<string> SnakeNames;

    public List<Tuple<string,int>> Posts;

    public TextAsset FrogNamesText;
    public TextAsset SnakeNamesText;
    public TextAsset PostsText;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the Lists
        FrogNames = new List<string>();
        SnakeNames = new List<string>();
        Posts = new List<Tuple<string,int>>();

        ParseFrogNames();
        ParseSnakeNames();
        ParsePosts();
    }

    void ParseFrogNames()
    {
        var fullText = FrogNamesText.text;
        string[] names = fullText.Split('\n');
        FrogNames.AddRange(names);
    }

    void ParseSnakeNames()
    {
        var fullText = SnakeNamesText.text;
        string[] names = fullText.Split('\n');
        SnakeNames.AddRange(names);
    }

    void ParsePosts()
    {
        var fullText = PostsText.text;
        string[] names = fullText.Split('\n');
        for(int i = 0; i < names.Length; i+=2)
        {
            int parsedInt;
            if(!Int32.TryParse(names[i + 1].Trim(), out parsedInt)) UnityEngine.Debug.Log($"Parsing Int Error in at line {i+1}");
            Posts.Add(Tuple.Create(names[i], parsedInt));
        }
    }
}
