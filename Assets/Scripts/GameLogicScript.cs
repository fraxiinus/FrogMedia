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
    public GameObject GameOverScreen; // Set in unity editor

    private bool isGameOver = false;
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
        if (isGameOver) return;

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
        else if (userHappiness <= 0) // END THE GAME YOU LOST
        {
            ShowGameOver();
            isGameOver = true;
        } 
        else userHappiness += happinessPerSecond * timeSinceLastFrame;

        if (fakeRating == 100 && !isGameOver)
        {
            isGameOver = true;
            ShowGameOver();
        }

        userMeter.SetValueTo(userHappiness);
        fakeMeter.SetValueTo(fakeRating);

        ScanForPostDeletes(0);
        ScanForPostDeletes(1);
        ScanForPostDeletes(2);
        ScanForPostDeletes(3);
    }

    void ShowGameOver()
    {
        GameOverScreen.SetActive(true);
    }

    void ScanForPostDeletes(int index)
    {
        var posts = generator.tabContents[index];
        var removed = new List<PostParameters>();
        foreach (var post in posts)
        {
            if (!post.Active)
            {
                CalculateFake(post);
                removed.Add(post);
            }
        }
        foreach (var del in removed)
        {
            posts.Remove(del);
        }
    }

    void CalculateFake(PostParameters post)
    {
        // Fake post spotted
        if (post.Content.FAKE > 0)
        {
            if (post.Deleted)
            {
                fakeRating -= post.Content.FAKE * 10;
            }
            else
            {
                fakeRating += post.Content.FAKE * 10;
            }
        }
        else
        {
            // You deleted a post that was not fake, people are mad
            if (post.Deleted)
            {
                userHappiness -= 20;
            }
        }
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
