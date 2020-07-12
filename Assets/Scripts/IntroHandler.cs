using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject TitleImage; // set in editor
    public GameObject Subtitle; // set in editor
    public GameObject Body; // Set in editor
    public GameObject Credits; // Set in editor

    public float fadeDuration = 1f;

    public List<string> BodyTexts; // Set in editor
    private int textCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        TitleImage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        Subtitle.GetComponent<Text>().color = new Color(55f / 255f, 149f / 255f, 110f / 255f, 0f);
        Body.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0f);
        Credits.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0f);

        Body.GetComponent<Text>().text = BodyTexts[textCounter].ToUpper();
        textCounter++;

        StartCoroutine(AnimateFadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator AnimateFadeIn()
    {
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            TitleImage.GetComponent<Image>().color = new Color(1f, 1f, 1f, t / fadeDuration);
            Subtitle.GetComponent<Text>().color = new Color(55f / 255f, 149f / 255f, 110f / 255f, t / fadeDuration);
            Body.GetComponent<Text>().color = new Color(0f, 0f, 0f, t / fadeDuration);
            Credits.GetComponent<Text>().color = new Color(0f, 0f, 0f, t / fadeDuration);
            yield return 0;
        }
        TitleImage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        Subtitle.GetComponent<Text>().color = new Color(55f / 255f, 149f / 255f, 110f / 255f, 1f);
        Body.GetComponent<Text>().color = new Color(0f, 0f, 0f, 1f);
        Credits.GetComponent<Text>().color = new Color(0f, 0f, 0f, 1f);
    }

    public IEnumerator AnimateChangeText(string text)
    {
        for (float t = 0f; t < (fadeDuration * 0.75); t += Time.deltaTime)
        {
            Body.GetComponent<Text>().color = new Color(0f, 0f, 0f, 1f - (t / (fadeDuration * 0.75f)));
            yield return 0;
        }
        Body.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0f);
        Body.GetComponent<Text>().text = text.ToUpper();
        for (float t = 0f; t < (fadeDuration * 0.75); t += Time.deltaTime)
        {
            Body.GetComponent<Text>().color = new Color(0f, 0f, 0f, t / (fadeDuration * 0.75f));
            yield return 0;
        }
        Body.GetComponent<Text>().color = new Color(0f, 0f, 0f, 1f);
    }

    public void OnPointerClick (PointerEventData pointerEventData)
    {
        if (textCounter < BodyTexts.Count)
        {
            // go next text
            StartCoroutine(AnimateChangeText(BodyTexts[textCounter]));
            textCounter++;
        }
        else
        {
            // load the game scene
            SceneManager.LoadScene("MainScene");
        }
    }

}
