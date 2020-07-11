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

    private TextParser loader;

    // Start is called before the first frame update
    void Start()
    {
        loader = gameObject.GetComponent<TextParser>();
        
        tabs = PageContainer.GetComponentsInChildren<Transform>()
            .Select(x => x.gameObject)
            .Where(x => x.name.StartsWith("TabPage"))
            .ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SpawnPost(0, "frog name", "I spawned this post through a script!");
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SpawnPost(1, "frog name", "I spawned this post through a script!");
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SpawnPost(2, "frog name", "I spawned this post through a script!");
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SpawnPost(3, "frog name", "I spawned this post through a script!");
        }
    }

    void SpawnPost(int tabIndex, string name, string content)
    {
        var newPost = Instantiate(PostPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        newPost.FindChildObject("Content").GetComponent<Text>().text = content.ToUpper();
        newPost.FindChildObject("Username").GetComponent<Text>().text = name.ToUpper();

        newPost.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 130);
        newPost.transform.SetParent(tabs[tabIndex].transform);
        newPost.transform.localScale = new Vector3(1, 1, 1);
    }
}
