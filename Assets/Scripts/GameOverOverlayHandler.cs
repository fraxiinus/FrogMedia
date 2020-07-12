using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverOverlayHandler : MonoBehaviour
{
    private Image backgroundImage;
    // Start is called before the first frame update
    void Start()
    {
        backgroundImage = gameObject.GetComponent<Image>();
        StartCoroutine(Animate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Animate() 
    {
        float duration = 1.0f;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            // Animate sprite moveing
           // transform.position = Vector3.Lerp(fromPosition, toPosition, t / duration);
            // Change sprite opacity
            backgroundImage.color = new Color(0f, 0f, 0f, (t / duration) / 2);
            yield return 0;
        }
    }
}
