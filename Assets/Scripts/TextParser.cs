using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class TextParser : MonoBehaviour
{
    //MAKE TEXT FILES AS THESE NAMES

    public List<string> frog_names;

    public List<string> snake_names;

    public List<Tuple<string,int>> post_texts;


    // Start is called before the first frame update
    void Start()
    {
        //Initialize the Lists
        frog_names = new List<string>();
        snake_names = new List<string>();
        post_texts = new List<Tuple<string,int>>();


        string[] filenames = Directory.GetFiles(@".\Assets\Scripts\text\", "*.txt");
        foreach (string filename in filenames)
        {
            ParseFile(filename);
        }

        /**
        foreach(string name in frog_names)
        {
            UnityEngine.Debug.Log(name);
        }
        foreach (string name in snake_names)
        {
            UnityEngine.Debug.Log(name);
        }
        
        foreach (Tuple<string,int> post_weight in post_texts)
        {
            UnityEngine.Debug.Log(post_weight.Item1 + post_weight.Item2);
        }
        */
    }


    void ParseFile(string filename)
    {
        string fulltext = File.ReadAllText(filename);

        if(filename.EndsWith("frog_names.txt")) //Format: New line seperated names.
        {
            string[] names = fulltext.Split('\n');
            frog_names.AddRange(names);
        }
        else if (filename.EndsWith("snake_names.txt")) //Format: New line seperated names.
        {
            string[] names = fulltext.Split('\n');
            snake_names.AddRange(names);
        }
        else if (filename.EndsWith("post_texts.txt")) //Format: Post text, then a new line, then the weight (0 FAKE to 10 FAKE). New line after this.
        {
            
            string[] names = fulltext.Split('\n');
            for(int i = 0; i < names.Length; i+=2)
            {
                //UnityEngine.Debug.Log("names: " + names[i] + "/" + names[i + 1]);
                int parsedInt;
                if(!Int32.TryParse(names[i + 1].Trim(), out parsedInt)) UnityEngine.Debug.Log("Parsing Int Error in post_texts.txt");
                post_texts.Add(Tuple.Create(names[i], parsedInt));
            }
        }
    }

}
