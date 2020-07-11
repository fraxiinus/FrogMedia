using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TextParser : MonoBehaviour
{
    //MAKE TEXT FILES AS THESE NAMES
    public List<string> FrogParts;
    public List<string> SnakeParts;
    public List<string> UniqueSnakes;
    public List<string> UniqueFrogs;
    public List<PostText> Posts;

    public TextAsset UniqueNamesText;
    public TextAsset PartNamesText;
    public TextAsset Posts1;
    public TextAsset Posts2;
    public TextAsset Posts3;
    public TextAsset Posts4;

    private char columnDelimiter = '\t';
    // Start is called before the first frame update
    void Start()
    {
        //Initialize the Lists
        FrogParts = new List<string>();
        SnakeParts = new List<string>();
        UniqueSnakes = new List<string>();
        UniqueFrogs = new List<string>();
        Posts = new List<PostText>();

        LoadUniqueNames();
        LoadNameParts();
        LoadPosts(1, Posts1);
        LoadPosts(2, Posts2);
        LoadPosts(3, Posts3);
        LoadPosts(4, Posts4);
    }

    public PostText GetRandomPost(int category)
    {
        var collection = Posts.Where(x => x.Category == category);
        return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count()));
    }

    public string GetRandomName(bool isFrog)
    {
        //Is this a Unique or Assembled name?
        int type = UnityEngine.Random.Range(0,2);
        string ret = "";

        if(isFrog) {
            if(type == 0) ret = UniqueFrogs.ElementAt(UnityEngine.Random.Range(0,UniqueFrogs.Count()));
            if(type == 1) {
                ret = FrogParts.ElementAt(UnityEngine.Random.Range(0,FrogParts.Count())) + " " + FrogParts.ElementAt(UnityEngine.Random.Range(0,FrogParts.Count()));
            }
        }
        if(!isFrog) {
            if(type == 0) ret = UniqueSnakes.ElementAt(UnityEngine.Random.Range(0,UniqueSnakes.Count()));
            if(type == 1) {
                ret = SnakeParts.ElementAt(UnityEngine.Random.Range(0,FrogParts.Count())) + " " + SnakeParts.ElementAt(UnityEngine.Random.Range(0,FrogParts.Count()));
            }
        }
        return ret;
    }

    void LoadUniqueNames()
    {
        var lines = UniqueNamesText.text.Split('\n');
        int lineCounter = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(columnDelimiter);
            if (parts.Length < 2)
            {
                Debug.Log($"Invalid line {lineCounter} in PartNamesText");
                continue;
            }

            if (parts[1].Trim().Equals("frog", StringComparison.OrdinalIgnoreCase))
            {
                UniqueFrogs.Add(parts[0].ToUpper());
            }
            else if (parts[1].Trim().Equals("snake", StringComparison.OrdinalIgnoreCase))
            {
                UniqueSnakes.Add(parts[0].ToUpper());
            }
            lineCounter++;
        }
    }

    void LoadNameParts()
    {
        var lines = PartNamesText.text.Split('\n');
        int lineCounter = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(columnDelimiter);
            if (parts.Length < 2)
            {
                Debug.Log($"Invalid line {lineCounter} in PartNamesText");
                continue;
            }

            if (parts[1].Trim().Equals("frog", StringComparison.OrdinalIgnoreCase))
            {
                FrogParts.Add(parts[0].ToUpper());
            }
            else if (parts[1].Trim().Equals("snake", StringComparison.OrdinalIgnoreCase))
            {
                SnakeParts.Add(parts[0].ToUpper());
            }
            lineCounter++;
        }
    }

    void LoadPosts(int category, TextAsset source)
    {
        var lines = source.text.Split('\n');
        int lineCounter = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(columnDelimiter);
            if (parts.Length < 2)
            {
                Debug.Log($"Invalid line {lineCounter} in PartNamesText");
                continue;
            }
            int fakeScore = 0;
            Int32.TryParse(parts[1].Trim(), out fakeScore);
            Posts.Add(new PostText { Content = parts[0].ToUpper(), FAKE = fakeScore, Category = category } );
        }
    }
}
