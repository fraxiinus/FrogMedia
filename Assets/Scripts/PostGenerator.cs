using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class PostGenerator : MonoBehaviour
{
    public GameObject PageContainer;
    
    public GameObject PostPrefab;

    public float PostLifetime;

    private List<GameObject> tabs;

    private List<List<PostParameters>> tabContents;
    public List<PostParameters> AllowedPosts;
    public List<PostParameters> DeletedPosts;

    private TextParser loader;

    // Start is called before the first frame update
    void Start()
    {
        loader = gameObject.GetComponent<TextParser>();
        
        tabs = PageContainer.GetComponentsInChildren<Transform>()
            .Select(x => x.gameObject)
            .Where(x => x.name.StartsWith("TabPage"))
            .ToList();
        
        tabContents = new List<List<PostParameters>>();
        tabContents.Add(new List<PostParameters>());
        tabContents.Add(new List<PostParameters>());
        tabContents.Add(new List<PostParameters>());
        tabContents.Add(new List<PostParameters>());

        AllowedPosts = new List<PostParameters>();
        DeletedPosts = new List<PostParameters>();
    }

    // Update is called once per frame
    void Update()
    {
        DebugSpawnPost();
        ClearPosts(0);
        ClearPosts(1);
        ClearPosts(2);
        ClearPosts(3);
    }

    void ClearPosts(int index)
    {
        var posts = tabContents[index];
        var removed = new List<PostParameters>();
        foreach (var post in posts)
        {
            if (!post.Active)
            {
                // score the post here
                if (post.Deleted)
                {
                    DeletedPosts.Add(post);
                    Debug.Log("Deleted");
                }
                else
                {
                    AllowedPosts.Add(post);
                    Debug.Log("Allowed");
                }
                removed.Add(post);
            }
        }
        foreach (var del in removed)
        {
            posts.Remove(del);
        }
    }

    // Delete me
    void DebugSpawnPost()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            var post = new PostParameters
            {
                Username = loader.GetRandomName(true),
                IsFrog = true,
                Active = true,
                Deleted = false,
                Life = PostLifetime,
                Content = loader.GetRandomPost(1),
            };
            SpawnPost(0, post);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            var post = new PostParameters()
            {
                Username = loader.GetRandomName(true),
                IsFrog = true,
                Active = true,
                Deleted = false,
                Life = PostLifetime,
                Content = loader.GetRandomPost(2),
            };
            SpawnPost(1, post);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            var post = new PostParameters()
            {
                Username = loader.GetRandomName(true),
                IsFrog = true,
                Active = true,
                Deleted = false,
                Life = PostLifetime,
                Content = loader.GetRandomPost(3),
            };
            SpawnPost(2, post);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            var post = new PostParameters()
            {
                Username = loader.GetRandomName(true),
                IsFrog = true,
                Active = true,
                Deleted = false,
                Life = PostLifetime,
                Content = loader.GetRandomPost(4),
            };
            SpawnPost(3, post);
        }
    }

    // Returns true if successfully created post in tab.
    // Adds post to tab's contents
    bool SpawnPost(int tabIndex, PostParameters post)
    {
        if (tabContents[tabIndex].Count == 5) return false;
        
        var newPost = Instantiate(PostPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        newPost.GetComponent<PostHandler>().Fields = post;

        newPost.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 130);
        newPost.transform.SetParent(tabs[tabIndex].FindChildObject("Container").transform);
        newPost.transform.localScale = new Vector3(1, 1, 1);
        
        tabContents[tabIndex].Add(newPost.GetComponent<PostHandler>().Fields);
        return true;
    }
}
