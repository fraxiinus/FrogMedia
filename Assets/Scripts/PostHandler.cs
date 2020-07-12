using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

//Script tied to the post Prefab, comes along with every new post.
public class PostHandler : MonoBehaviour, IPointerClickHandler
{
    public PostParameters Fields { get; set; }
    
    public GameObject NotifPrefab; // Set in unity inspector

    private Text TimerDisplay;
    private Image backgroundImage;

    private bool Destroyed = false;

    void Start()
    {
        TimerDisplay = gameObject.FindChildObject("TimeLeft").GetComponent<Text>();
        gameObject.FindChildObject("Username").GetComponent<Text>().text = Fields.Username;
        gameObject.FindChildObject("Content").GetComponent<Text>().text = Fields.Content.Text;

        backgroundImage = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (Destroyed) return;
        Fields.Life -= Time.deltaTime;
        TimerDisplay.text = string.Format("{0:F1}", Fields.Life);

        if (Fields.Life < 0)
        {
            if (Fields.Content.FAKE > 0 && !Fields.Deleted)
            {
                var newNotif = Instantiate(NotifPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newNotif.GetComponent<NotificationHandler>().Type = NotificationType.FAKE;
                newNotif.GetComponent<NotificationHandler>().TabIndex = Fields.Content.Category;
            }
            else if (Fields.Content.FAKE == 0 && Fields.Deleted)
            {
                var newNotif = Instantiate(NotifPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newNotif.GetComponent<NotificationHandler>().Type = NotificationType.UNHAPPY;
                newNotif.GetComponent<NotificationHandler>().TabIndex = Fields.Content.Category;
            }

            Fields.Active = false;
            Destroyed = true;
            StartCoroutine(AnimateAndDestroy());
        }
    }

    public IEnumerator AnimateAndDestroy()
    {
        float duration = 0.6f;
        if (Fields.Deleted)
        {
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                // Animate sprite moveing
                //transform.position = Vector3.Lerp(fromPosition, toPosition, t / duration);
                // Change sprite opacity
                backgroundImage.color = new Color(1f, duration - t, duration - t);
                yield return 0;
            }
        }
        else
        {
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                // Animate sprite moveing
                //transform.position = Vector3.Lerp(fromPosition, toPosition, t / duration);
                // Change sprite opacity
                backgroundImage.color = new Color(duration - t, 1f, duration - t);
                yield return 0;
            }
        }
        
        Destroy(this.gameObject);
    }

    public void OnPointerClick (PointerEventData pointerEventData)
    {
        if(pointerEventData.button == PointerEventData.InputButton.Left)
        {
            // Banned
            Fields.Deleted = true;
            Fields.Active = false;
            Fields.Life = 0.0f;
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            // Allowed
            Fields.Deleted = false;
            Fields.Active = false;
            Fields.Life = 0.0f;
        }
    }
}