using UnityEngine;
using UnityEngine.UI;

public class PostContainer
{
    public bool Active { get; set; }
    public float Life { get; set; }
    public bool IsFrog { get; set; }
    public string Username { get; set; }
    public PostText Content { get; set; }
    public Text TimerDisplay { get; set; }
    public GameObject LinkedGameObject { get; set; }
}