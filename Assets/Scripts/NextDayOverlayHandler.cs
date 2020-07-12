using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextDayOverlayHandler : MonoBehaviour
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
        float fadeInDuration = 0.5f;
        for (float t = 0f; t < fadeInDuration; t += Time.deltaTime)
        {
            // Animate sprite moveing
           // transform.position = Vector3.Lerp(fromPosition, toPosition, t / duration);
            // Change sprite opacity
            backgroundImage.color = new Color(0f, 0f, 0f, (t / fadeInDuration) / 2);
            yield return 0;
        }

        backgroundImage.color = new Color(0f, 0f, 0f, 0.5f);

        float stayDuration = 4.0f;
        for (float t = 0f; t < stayDuration; t += Time.deltaTime)
        {
            // Animate sprite moveing
           // transform.position = Vector3.Lerp(fromPosition, toPosition, t / duration);
            // Change sprite opacity
            // backgroundImage.color = new Color(0f, 0f, 0f, (t / stayDuration) / 2);
            yield return 0;
        }

        float fadeOutDuration = 0.5f;
        for (float t = 0f; t < fadeOutDuration; t += Time.deltaTime)
        {
            // Animate sprite moveing
           // transform.position = Vector3.Lerp(fromPosition, toPosition, t / duration);
            // Change sprite opacity
            backgroundImage.color = new Color(0f, 0f, 0f, 1 - (t / fadeOutDuration));
            yield return 0;
        }

        Destroy(this.gameObject);
    }
}
