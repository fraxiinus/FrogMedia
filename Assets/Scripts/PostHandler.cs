using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Script tied to the post Prefab, comes along with every new post.
public class PostHandler : MonoBehaviour, IPointerClickHandler
{
    public PostParameters Fields { get; set; }
    
    public GameObject NotifPrefab; // Set in unity inspector

    private Text TimerDisplay;

    void Start()
    {
        TimerDisplay = gameObject.FindChildObject("TimeLeft").GetComponent<Text>();
        gameObject.FindChildObject("Username").GetComponent<Text>().text = Fields.Username;
        gameObject.FindChildObject("Content").GetComponent<Text>().text = Fields.Content.Text;
    }

    void Update()
    {
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
            Destroy(this.gameObject);
        }
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