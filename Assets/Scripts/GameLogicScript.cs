using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{
    [SerializeField]
    private float userHappiness;
    [SerializeField]
    private float fakeRating;
    private int postsLeftToday;
    private float timeBetweenPosts;
    private float timeSinceLastPost;
    private float postLifetime;
    public float timePerDay; //Set in the Unity editor
    private float timeLeftInDay;
    public float happinessPerSecond; //Set in the Unity editor
    public List<float> postLifetimes; //Set in the Unity editor
    public List<float> timesBetweenPosts; //Set in the Unity editor
    public List<int> postsPerDay; //Set in the Unity editor
    public GameObject UserMeterGO; // Set in Unity Editor
    public GameObject FakeMeterGO; // Set in unity editor

    private TextParser loader;
    private PostGenerator generator;
    private MeterHandler userMeter; // internal meter handler object
    private MeterHandler fakeMeter; // internal meter handler object

    // Start is called before the first frame update
    void Start()
    {
        generator = gameObject.GetComponent<PostGenerator>();
        loader = gameObject.GetComponent<TextParser>();
        userMeter = UserMeterGO.GetComponent<MeterHandler>();
        fakeMeter = FakeMeterGO.GetComponent<MeterHandler>();

        //set to the first day values by default: TODO change later
        postsLeftToday = postsPerDay[0];
        timeBetweenPosts = timesBetweenPosts[0];
        postLifetime = postLifetimes[0];
        timeLeftInDay = timePerDay;
        timeSinceLastPost = timeBetweenPosts;


        userHappiness = 50;
        fakeRating = 50;
        
        userMeter.MaxValue = 100;    
        fakeMeter.MaxValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceLastFrame = Time.deltaTime;
        //Check time left in Day
        timeLeftInDay -= timeSinceLastFrame;
        if( timeLeftInDay <= 0) {
            return; //TODO MOVE TO NEXT DAY
        }
        //Check time left between spawning posts
        timeSinceLastPost -= timeSinceLastFrame;
        if( timeSinceLastPost <= 0) {
            //Spawn a new post
            int rand = UnityEngine.Random.Range(0, 4);
            var post = new PostParameters() //TODO load an actual post
            {
                Username = loader.GetRandomName(true),
                IsFrog = true,
                Active = true,
                Deleted = false,
                Life = postLifetime,
                Content = loader.GetRandomPost(rand),
            };
            generator.SpawnPost(rand, post);
            timeSinceLastPost = timeBetweenPosts;
            postsLeftToday--;
        }

        if(userHappiness >= 100) userHappiness = 100;
        else userHappiness += happinessPerSecond * timeSinceLastFrame;

        userMeter.SetValueTo(userHappiness);
        fakeMeter.SetValueTo(fakeRating);
    }
}
