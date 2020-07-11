using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PostGenerator : MonoBehaviour
{
    public GameObject PageContainer;
    
    public GameObject PostPrefab;

    public float PostLifetime;

    private List<GameObject> tabs;

    private List<List<PostContainer>> tabContents;
    public List<PostContainer> AllowedPosts;
    public List<PostContainer> DeletedPosts;

    private TextParser loader;

    // Start is called before the first frame update
    void Start()
    {
        loader = gameObject.GetComponent<TextParser>();
        
        tabs = PageContainer.GetComponentsInChildren<Transform>()
            .Select(x => x.gameObject)
            .Where(x => x.name.StartsWith("TabPage"))
            .ToList();
        
        tabContents = new List<List<PostContainer>>();
        tabContents.Add(new List<PostContainer>());
        tabContents.Add(new List<PostContainer>());
        tabContents.Add(new List<PostContainer>());
        tabContents.Add(new List<PostContainer>());

        AllowedPosts = new List<PostContainer>();
        DeletedPosts = new List<PostContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        DebugSpawnPost();
        AgePosts(0);
        AgePosts(1);
        AgePosts(2);
        AgePosts(3);
    }

    void AgePosts(int index)
    {
        var posts = tabContents[index];
        var removed = new List<PostContainer>();
        foreach (var post in posts)
        {
            post.Life -= Time.deltaTime;
            post.TimerDisplay.text = string.Format("{0:F1}", post.Life);
            
            if (post.Life < 0)
            {
                post.Active = false;

                // score the post here
                AllowedPosts.Add(post);

                removed.Add(post);
                Destroy(post.LinkedGameObject);
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
            var post = new PostContainer()
            {
                Username = "Ribbitor",
                IsFrog = true,
                Active = true,
                Life = PostLifetime,
                Content = loader.GetRandomPost(1),
            };
            SpawnPost(0, post);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            var post = new PostContainer()
            {
                Username = "G*mer",
                IsFrog = true,
                Active = true,
                Life = PostLifetime,
                Content = loader.GetRandomPost(2),
            };
            SpawnPost(1, post);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            var post = new PostContainer()
            {
                Username = "Weeb",
                IsFrog = true,
                Active = true,
                Life = PostLifetime,
                Content = loader.GetRandomPost(3),
            };
            SpawnPost(2, post);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            var post = new PostContainer()
            {
                Username = "Memer",
                IsFrog = true,
                Active = true,
                Life = PostLifetime,
                Content = loader.GetRandomPost(4),
            };
            SpawnPost(3, post);
        }
    }

    // Returns true if successfully created post in tab.
    // Adds post to tab's contents
    bool SpawnPost(int tabIndex, PostContainer post)
    {
        if (tabContents[tabIndex].Count == 5) return false;
        
        var newPost = Instantiate(PostPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        newPost.FindChildObject("Content").GetComponent<Text>().text = post.Content.Text.ToUpper();
        newPost.FindChildObject("Username").GetComponent<Text>().text = post.Username.ToUpper();
        
        post.TimerDisplay = newPost.FindChildObject("TimeLeft").GetComponent<Text>();
        post.TimerDisplay.text = string.Format("{0:F1}", post.Life);

        newPost.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 130);
        newPost.transform.SetParent(tabs[tabIndex].transform);
        newPost.transform.localScale = new Vector3(1, 1, 1);

        post.LinkedGameObject = newPost;
        tabContents[tabIndex].Add(post);
        return true;
    }
}
