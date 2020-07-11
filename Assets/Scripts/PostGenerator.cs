using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PostGenerator : MonoBehaviour
{
    public GameObject PageContainer;
    
    public GameObject PostPrefab;

    private List<GameObject> tabs;

    private List<List<PostText>> tabContents;

    private TextParser loader;

    // Start is called before the first frame update
    void Start()
    {
        loader = gameObject.GetComponent<TextParser>();
        
        tabs = PageContainer.GetComponentsInChildren<Transform>()
            .Select(x => x.gameObject)
            .Where(x => x.name.StartsWith("TabPage"))
            .ToList();
        
        tabContents = new List<List<PostText>>();
        tabContents.Add(new List<PostText>());
        tabContents.Add(new List<PostText>());
        tabContents.Add(new List<PostText>());
        tabContents.Add(new List<PostText>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SpawnPost(0, "frog name", loader.GetRandomPost(1));
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SpawnPost(1, "frog name", loader.GetRandomPost(2));
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SpawnPost(2, "frog name", loader.GetRandomPost(3));
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SpawnPost(3, "frog name", loader.GetRandomPost(4));
        }
    }

    // Returns true if successfully created post in tab.
    // Adds post to tab's contents
    bool SpawnPost(int tabIndex, string name, PostText post)
    {
        if (tabContents[tabIndex].Count == 5) return false;
        tabContents[tabIndex].Add(post);

        var newPost = Instantiate(PostPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        newPost.FindChildObject("Content").GetComponent<Text>().text = post.Content.ToUpper();
        newPost.FindChildObject("Username").GetComponent<Text>().text = name.ToUpper();

        newPost.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 130);
        newPost.transform.SetParent(tabs[tabIndex].transform);
        newPost.transform.localScale = new Vector3(1, 1, 1);
        return true;
    }
}
