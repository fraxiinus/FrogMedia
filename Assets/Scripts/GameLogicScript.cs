using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{
    [SerializeField]
    private int DayIndex;
    [SerializeField]
    private float userHappiness;
    [SerializeField]
    private float fakeRating;
    private int postsLeftToday;
    private float timeSinceLastPost;
    public float timePerDay; //Set in the Unity editor
    private float timeLeftInDay;
    public float happinessPerSecond; //Set in the Unity editor
    public List<float> postLifetimes; //Set in the Unity editor
    public List<float> timesBetweenPosts; //Set in the Unity editor
    public List<int> postsPerDay; //Set in the Unity editor
    public List<int> fakeChancesPerDay; // set in unity editor
    public List<float> minFakePerDay; // set in unity editor
    public List<float> maxFakePerDay; // set in unity editor
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

        // The day starts at 0
        DayIndex = 0;

        //set to the first day values by default: TODO change later
        postsLeftToday = postsPerDay[0];
        timeLeftInDay = timePerDay;
        timeSinceLastPost = timesBetweenPosts[DayIndex];

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
            DayIndex++;
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
                Life = postLifetimes[DayIndex],
                Content = GetPost(rand),
            };
            generator.SpawnPost(rand, post);
            timeSinceLastPost = timesBetweenPosts[DayIndex];
            postsLeftToday--;
        }

        if(userHappiness >= 100) userHappiness = 100;
        else userHappiness += happinessPerSecond * timeSinceLastFrame;

        userMeter.SetValueTo(userHappiness);
        fakeMeter.SetValueTo(fakeRating);
    }

    PostText GetPost(int category)
    {
        // This function would need to generate a random post in the future
        var outOfHundred = Random.Range(0, 100);
        if (outOfHundred <= fakeChancesPerDay[DayIndex])
        {
            // Get a Fake post instead~
            var fakePost = loader.GetRandomFakePost(minFakePerDay[DayIndex], maxFakePerDay[DayIndex]);
            // Set the category because thats the only way to knows where it is.
            fakePost.Category = category;
            return fakePost;
        }
        else 
        {
            return loader.GetRandomPost(category);
        }
    }
}
