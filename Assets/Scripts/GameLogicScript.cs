using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject NextDayPrefab; // Set in unity editor
    public Canvas OverlayDestination; // set in unity editor
    public GameObject DayCounterGO; // set in unity editor
    public GameObject RuleList; // set in unity editor
    public GameObject RuleTextPrefab; // set in unity editor
    public GameObject RetryButton; // Set in unity editor
    public GameObject TabController; // Set in unity editor

    [SerializeField]
    private List<string> RulesTexts; //The text for each day that appears on screen. MANUALLY SET IN UNITY EDITOR.
    [SerializeField]
    private List<RulesContainer> RulesForDays; //The text to modify the messages. Each day is a different RulesContainer object, just a container for a string List
    //Manually set in the Create function bc im a code goblin
    private bool isPaused = false;
    private TextParser loader;
    private PostGenerator generator;
    private MeterHandler userMeter; // internal meter handler object
    private MeterHandler fakeMeter; // internal meter handler object
    private GameObject currentOverlay = null;
    private Text DayCountText;

    private List<GameObject> currentRules;

    // Start is called before the first frame update
    void Start()
    {
        generator = gameObject.GetComponent<PostGenerator>();
        loader = gameObject.GetComponent<TextParser>();
        userMeter = UserMeterGO.GetComponent<MeterHandler>();
        fakeMeter = FakeMeterGO.GetComponent<MeterHandler>();
        DayCountText = DayCounterGO.GetComponent<Text>();
        currentRules = new List<GameObject>();

        RetryButton.GetComponent<Button>().onClick.AddListener(delegate { RetryButtonPressed(); } );

        // The day starts at 0
        DayIndex = 0;
        DayCountText.text = (DayIndex + 1).ToString();

        //set to the first day values by default: TODO change later
        postsLeftToday = postsPerDay[0]; // do we need this? doesnt seem to do anything
        timeLeftInDay = timePerDay;
        timeSinceLastPost = timesBetweenPosts[DayIndex];

        userHappiness = 50;
        fakeRating = 50;
        
        userMeter.MaxValue = 100;    
        fakeMeter.MaxValue = 100;

        RulesForDays = new List<RulesContainer>(){
            new RulesContainer(new List<string>() {"none"}),
            new RulesContainer(new List<string>() {"#Snake","#Snakey"}),
            new RulesContainer(new List<string>() {"#Snek","#Sneaky"}),
            new RulesContainer(new List<string>() {"#DefinitelyFrog","#RealNews"}),
            new RulesContainer(new List<string>() {"Yummy","Nom"}),
            new RulesContainer(new List<string>() {"owo","FrogChamp"}),
        };


        DisplayRule(RulesTexts[DayIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentOverlay) return; // If we are displaying an overlay, wait until it's gone
        if (isPaused) return;

        float timeSinceLastFrame = Time.deltaTime;
        //Check time left in Day
        timeLeftInDay -= timeSinceLastFrame;
        if(timeLeftInDay <= 0) 
        {
            ShowNextDayOverlay(); // this will pause the game
            goNextDay();
            return; //TODO MOVE TO NEXT DAY
        }
        //Check time left between spawning posts
        timeSinceLastPost -= timeSinceLastFrame;
        if( timeSinceLastPost <= 0) 
        {
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
            ShowGameOver(true);
            fakeMeter.SetValueTo(0);
            isPaused = true;
        } 
        else userHappiness += happinessPerSecond * timeSinceLastFrame;

        if (fakeRating <= 0) fakeRating = 0;
        else if (fakeRating >= 100 && !isPaused)
        {
            isPaused = true;
            fakeMeter.SetValueTo(100);
            ShowGameOver(false);
        }

        userMeter.SetValueTo(userHappiness);
        fakeMeter.SetValueTo(fakeRating);

        ScanForPostDeletes(0);
        ScanForPostDeletes(1);
        ScanForPostDeletes(2);
        ScanForPostDeletes(3);
    }

    void goNextDay() 
    {
        generator.ClearAllPosts();
        TabController.GetComponent<TabController>().GoToHomePage();
        timeLeftInDay = timePerDay;
        DayIndex++;
        DayCountText.text = (DayIndex + 1).ToString();
        DisplayRule(RulesTexts[DayIndex]);
    }

    void ShowGameOver(bool unhappy)
    {
        if (unhappy)
        {
            GameOverScreen.FindChildObject("Wrapper").FindChildObject("GameOverReason").GetComponent<Text>().text = "THE RIBBITORS ARE UNHAPPY!!! YOU'RE FIRED!";
        }
        else
        {
            GameOverScreen.FindChildObject("Wrapper").FindChildObject("GameOverReason").GetComponent<Text>().text = "OVERWHELMINGLY F.A.K.E.! DISHONESTY RAMPANT!";
        }
        GameOverScreen.SetActive(true);
    }

    void ShowNextDayOverlay()
    {
        if (currentOverlay) return;
        var overlay = Instantiate(NextDayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        overlay.transform.SetParent(OverlayDestination.transform);
        overlay.transform.localScale = new Vector3(1, 1, 1);
        overlay.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        overlay.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        overlay.GetComponent<RectTransform>().offsetMax = new Vector2(0f, -158f);
        overlay.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 200f);
        currentOverlay = overlay;
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
            // Get a Fake post~ uwu
            PostText fakePost;
            //Is this gonna be a random or a pregen snake post?
            var randOrPregen = Random.Range(0, 100);
            
            if(randOrPregen <= 74+DayIndex && DayIndex != 0) { //TODO change back
                fakePost = loader.GetRandomPost(category);
                fakePost.FAKE += Random.Range(minFakePerDay[DayIndex], maxFakePerDay[DayIndex]);
                //Editing of text

                var randomHashtagIndex = Random.Range(1, DayIndex); //pick a random day's hashtags from the days we've passed
                string randomHashtag = " " + RulesForDays[1].rules[Random.Range(0,1)].ToUpper(); //Each day has 2 hashtags: pick one
                //Pick a random hashtag to add if we're NOT in day 1
                if(fakePost.Text.Contains("..."))
                    fakePost.Text.Insert(fakePost.Text.IndexOf("..."), randomHashtag);
                else
                    fakePost.Text += randomHashtag;
                Debug.Log("Making a fake post!");
            }
            else {
                fakePost = loader.GetRandomFakePost(minFakePerDay[DayIndex], maxFakePerDay[DayIndex]);
                // Set the category because thats the only way to knows where it is.
                fakePost.Category = category;
            }
            
            return fakePost;
        }
        else 
        {
            return loader.GetRandomPost(category);
        }
    }

    void DisplayRule(string ruleText)
    {
        var newRule = Instantiate(RuleTextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newRule.transform.parent = RuleList.transform;
        newRule.transform.localScale = new Vector3(1, 1, 1);
        newRule.GetComponent<Text>().text = ruleText.ToUpper();
        currentRules.Add(newRule);
    }

    void ClearRules()
    {
        foreach(var rule in currentRules)
        {
            Destroy(rule);
        }
    }

    public void RetryButtonPressed()
    {
        // Reset game
        userHappiness = 50f;
        fakeRating = 50f;
        DayIndex = 0;
        DayCountText.text = (DayIndex + 1).ToString();
        postsLeftToday = postsPerDay[0]; // do we need this? doesnt seem to do anything
        timeLeftInDay = timePerDay;
        timeSinceLastPost = timesBetweenPosts[DayIndex];
        isPaused = false;
        ClearRules();
        GameOverScreen.SetActive(false);
    }
}
