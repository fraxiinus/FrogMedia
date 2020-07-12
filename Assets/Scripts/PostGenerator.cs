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
    public GameObject NotifPrefab;

    public float PostLifetime; //set in unity

    private List<GameObject> tabs;

    public List<List<PostParameters>> tabContents;
    public List<PostParameters> AllowedPosts;
    public List<PostParameters> DeletedPosts;

    private TextParser loader;
    //private GameLogicScript gameLogic;

    // Start is called before the first frame update
    void Start()
    {
        loader = gameObject.GetComponent<TextParser>();
        //gameLogic = gameObject.GetComponent<GameLogicScript>();
        
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
        
    }

    // Delete me
    // void DebugSpawnPost()
    // {
    //     if (Input.GetKeyUp(KeyCode.Q))
    //     {
    //         var post = new PostParameters
    //         {
    //             Username = loader.GetRandomName(true),
    //             IsFrog = true,
    //             Active = true,
    //             Deleted = false,
    //             Life = PostLifetime,
    //             Content = loader.GetRandomPost(1),
    //         };
    //         SpawnNotification(0, NotificationType.NEW);
    //         SpawnPost(0, post);
    //     }
    //     if (Input.GetKeyUp(KeyCode.W))
    //     {
    //         var post = new PostParameters()
    //         {
    //             Username = loader.GetRandomName(true),
    //             IsFrog = true,
    //             Active = true,
    //             Deleted = false,
    //             Life = PostLifetime,
    //             Content = loader.GetRandomPost(2),
    //         };
    //         SpawnNotification(1, NotificationType.NEW);
    //         SpawnPost(1, post);
    //     }
    //     if (Input.GetKeyUp(KeyCode.E))
    //     {
    //         var post = new PostParameters()
    //         {
    //             Username = loader.GetRandomName(true),
    //             IsFrog = true,
    //             Active = true,
    //             Deleted = false,
    //             Life = PostLifetime,
    //             Content = loader.GetRandomPost(3),
    //         };
    //         SpawnNotification(2, NotificationType.NEW);
    //         SpawnPost(2, post);
    //     }
    //     if (Input.GetKeyUp(KeyCode.R))
    //     {
    //         var post = new PostParameters()
    //         {
    //             Username = loader.GetRandomName(true),
    //             IsFrog = true,
    //             Active = true,
    //             Deleted = false,
    //             Life = PostLifetime,
    //             Content = loader.GetRandomPost(4),
    //         };
    //         SpawnNotification(3, NotificationType.NEW);
    //         SpawnPost(3, post);
    //     }
    // }

    // Returns true if successfully created post in tab.
    // Adds post to tab's contents
    public bool SpawnPost(int tabIndex, PostParameters post)
    {
        if (tabContents[tabIndex].Count == 5) return false;
        
        var newPost = Instantiate(PostPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        newPost.GetComponent<PostHandler>().Fields = post;

        Debug.Log($"Spawn post index {tabIndex}");

        newPost.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 130);
        newPost.transform.SetParent(tabs[tabIndex].FindChildObject("Container").transform);
        newPost.transform.localScale = new Vector3(1, 1, 1);
        
        tabContents[tabIndex].Add(newPost.GetComponent<PostHandler>().Fields);

        SpawnNotification(tabIndex, NotificationType.NEW);
        return true;
    }

    public void updatePostLifetime(float time)
    {
        PostLifetime = time;
    }

    void SpawnNotification(int tabIndex, NotificationType type)
    {
        var newNotif = Instantiate(NotifPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newNotif.GetComponent<NotificationHandler>().Type = type;
        newNotif.GetComponent<NotificationHandler>().TabIndex = tabIndex;
    }
}
