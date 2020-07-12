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

    public AudioClip UnhappySound;
    public AudioClip FakeSound;
    public AudioClip NewSound;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    // Hardcoded tab positions 765
    private Vector3 startPosition1 = new Vector3(200, 662, 90);
    private Vector3 endPosition1 = new Vector3(200, 765, 90);
    private Vector3 startPosition2 = new Vector3(340, 662, 90);
    private Vector3 endPosition2 = new Vector3(340, 765, 90);
    private Vector3 startPosition3 = new Vector3(480, 662, 90);
    private Vector3 endPosition3 = new Vector3(480, 765, 90);
    private Vector3 startPosition4 = new Vector3(620, 662, 90);
    private Vector3 endPosition4 = new Vector3(620, 765, 90);

    private float xOffset;
    private float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
        xOffset = Random.Range(-5f, 5f);
        yOffset = Random.Range(-5f, 5f);
        SetImage();
        SetPosition();
        StartCoroutine(Animate());
        PlaySound();
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
                toPosition = new Vector3(endPosition1.x + xOffset, endPosition1.y + yOffset, endPosition1.z);
                break;
            case 1:
                toPosition = new Vector3(endPosition2.x + xOffset, endPosition2.y + yOffset, endPosition2.z);
                break;
            case 2:
                toPosition = new Vector3(endPosition3.x + xOffset, endPosition3.y + yOffset, endPosition3.z);
                break;
            case 3:
                toPosition = new Vector3(endPosition4.x + xOffset, endPosition4.y + yOffset, endPosition4.z);
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

    void PlaySound()
    {
        switch (Type)
        {
            case NotificationType.FAKE:
                audioSource.clip = FakeSound;
                audioSource.PlayDelayed(0.05f);
                break;
            case NotificationType.NEW:
                audioSource.clip = NewSound;
                audioSource.PlayDelayed(0.05f);
                break;
            case NotificationType.UNHAPPY:
                audioSource.clip = UnhappySound;
                audioSource.PlayDelayed(0.05f);
                break;
        }
    }

    void SetPosition()
    {
        switch (TabIndex)
        {
            case 0:
                transform.position = new Vector3(startPosition1.x + xOffset, startPosition1.y + yOffset, startPosition1.z);
                break;
            case 1:
                transform.position = new Vector3(startPosition2.x + xOffset, startPosition2.y + yOffset, startPosition2.z);
                break;
            case 2:
                transform.position = new Vector3(startPosition3.x + xOffset, startPosition3.y + yOffset, startPosition3.z);
                break;
            case 3:
                transform.position = new Vector3(startPosition4.x + xOffset, startPosition4.y + yOffset, startPosition4.z);
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
