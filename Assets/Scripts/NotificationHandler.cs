using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NotificationType { FAKE, NEW, UNHAPPY }

public class NotificationHandler : MonoBehaviour
{
    // Values set when created
    public int TabIndex;
    public NotificationType Type;

    // These sprites are set in unity editor
    public Sprite FakeImage;
    public Sprite NewImage;
    public Sprite UnhappyImage;

    private SpriteRenderer spriteRenderer;

    // Hardcoded tab positions 765
    private Vector3 startPosition1 = new Vector3(200, 662, 1);
    private Vector3 endPosition1 = new Vector3(200, 765, 1);
    private Vector3 startPosition2 = new Vector3(340, 662, 1);
    private Vector3 endPosition2 = new Vector3(340, 765, 1);
    private Vector3 startPosition3 = new Vector3(480, 662, 1);
    private Vector3 endPosition3 = new Vector3(480, 765, 1);
    private Vector3 startPosition4 = new Vector3(620, 662, 1);
    private Vector3 endPosition4 = new Vector3(620, 765, 1);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        SetImage();
        SetPosition();
        StartCoroutine(Animate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Animate() 
    {
        Vector3 toPosition = endPosition1;
        switch (TabIndex)
        {
            case 0:
                toPosition = endPosition1;
                break;
            case 1:
                toPosition = endPosition2;
                break;
            case 2:
                toPosition = endPosition3;
                break;
            case 3:
                toPosition = endPosition4;
                break;
        }

        Vector3 fromPosition = transform.position;
        float duration = 2.0f;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            // Animate sprite moveing
            transform.position = Vector3.Lerp(fromPosition, toPosition, t / duration);
            // Change sprite opacity
            spriteRenderer.color = new Color(1f, 1f, 1f, 1 - (t / duration));
            yield return 0;
        }
        transform.position = toPosition;
        Destroy(this.gameObject);
    }

    void SetPosition()
    {
        switch (TabIndex)
        {
            case 0:
                transform.position = startPosition1;
                break;
            case 1:
                transform.position = startPosition2;
                break;
            case 2:
                transform.position = startPosition3;
                break;
            case 3:
                transform.position = startPosition4;
                break;
        }
    }

    // Change the sprite to the correct type
    void SetImage()
    {
        switch (Type)
        {
            case NotificationType.FAKE:
                spriteRenderer.sprite = FakeImage;
                break;
            case NotificationType.NEW:
                spriteRenderer.sprite = NewImage;
                break;
            case NotificationType.UNHAPPY:
                spriteRenderer.sprite = UnhappyImage;
                break;
        }
    }
}
