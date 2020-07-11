using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PostGenerator : MonoBehaviour
{
    public GameObject PageContainer;

    public List<GameObject> tabs;

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
        
    }
}
