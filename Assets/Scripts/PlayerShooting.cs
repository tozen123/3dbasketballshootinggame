using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(PlayerMovement))]
public class PlayerShooting : MonoBehaviour
{

    [Header("References")]

    [SerializeField] private Transform Ball;
    [SerializeField] private Transform PosDribble;
    [SerializeField] private Transform PosOverHead;
    [SerializeField] private Transform Target;


    [Header("UI References")]
    [SerializeField] private Button chargeShootBallButton;
    [SerializeField] private Button launchBallButton;

    [Header("Script References")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Shooting Attributes")]
    [SerializeField] private float timeRange;

    [SerializeField] private bool IsBallInHands = true;
    [SerializeField] private bool IsBallFlying = false;
    [SerializeField] private float T = 0;
    [SerializeField] private bool isCharging = false;


    [SerializeField] private float ballTravelSpeed;

    [Header("Shooting Mechanics Attributes")]
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private float sliderSpeed = 2.0f;
    [SerializeField] private bool isIncreasing = true;


    [SerializeField] private float correctShotRangeMin = 0.45f;
    [SerializeField] private float correctShotRangeMax = 0.75f;




    [Header("Dribbling Attributes")]
    [SerializeField] private float dribblingSpeed = 5f;
    [SerializeField] private float dribblingHeight = 2f;

    [Header("Animator Reference")]
    public Animator animator;

    [Header("Mode Controller")]
    [SerializeField] private bool isArcade = false;
    [SerializeField] private bool isPlay = false;

    // privates
    private float errorRange = 6.0f;
    private float errorRangeX;
    private CameraSystem cameraSystem;


    [Header("Spot References")]
    [SerializeField] private List<Transform> spots; 
    private Transform currentSpot; 
    private float spotRange = 2.0f;


    [Header("Audio References")]
    [SerializeField] private float dribbleSoundThreshold = 0.05f;  
    private float previousBounceValue = 0;
    [SerializeField] private AudioSource audioSource;  
    [SerializeField] private AudioClip[] dribbleSounds;


    [SerializeField] private bool MenuChar = false;

    void Start()
    {
        cameraSystem = Camera.main.GetComponent<CameraSystem>();
        audioSource = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<AudioSource>();
        launchBallButton.gameObject.SetActive(false);
        chargeSlider.gameObject.SetActive(false);
        chargeShootBallButton.onClick.AddListener(StartCharging);
        launchBallButton.onClick.AddListener(Shoot);

        if(spots.Count > 0)
        {
            SetNextSpot();

        }
    }
    void SetNextSpot()
    {
        if (spots.Count > 0)
        {

            foreach (Transform spot in spots)
            {
                spot.GetChild(0).GetComponent<Renderer>().enabled = false; // Hide the spot
            }

            currentSpot = spots[Random.Range(0, spots.Count)];
            currentSpot.GetChild(0).GetComponent<Renderer>().enabled = true;  // Show the active spot

        }
        
    }
    bool IsInCorrectSpot()
    {
        if (Vector3.Distance(transform.position, currentSpot.position) <= spotRange)
        {
            return true;
        }
        else
        {
            Debug.Log("Player not in the correct spot");
            return false;
        }
    }

    void Update()
    {
        // condition if the ball is not on the player hand, shoot button is disabled
        //if (!Ball & !IsInCorrectSpot())
        //{
        //    IsBallInHands = false;
        //    chargeShootBallButton.interactable = false;
        //}
        //else
        //{
        //    chargeShootBallButton.interactable = true;
        //}
        if (!Ball)
        {
            IsBallInHands = false;
        }
        if (spots.Count > 0)
        {
            
            if (!IsInCorrectSpot())
            {
                chargeShootBallButton.interactable = false;  // Disable button if player is not in the correct spot
            }
            else
            {
                chargeShootBallButton.interactable = true;  // Enable button if player is in the correct spot
            }

        }
       

        // condition if the ball is in the players hand, it will bounce and if charging it will be on the players over head
        if (IsBallInHands)
        {
            if (isCharging)
            {
                Ball.position = PosOverHead.position;
                LookAtTarget(Target.transform);
            }
            else
            {
                float bounceValue = Mathf.Abs(Mathf.Sin(Time.time * dribblingSpeed));

                Ball.position = PosDribble.position + new Vector3(0, dribblingHeight, 0) * bounceValue;
                playerMovement.haveBall = true;


                if (previousBounceValue > bounceValue && bounceValue < dribbleSoundThreshold)
                {

                    if (!MenuChar)
                    {
                        PlayRandomDribbleSound();

                    }
                }

                previousBounceValue = bounceValue;
            }
        }
        else
        {
            playerMovement.haveBall = false;
        }

        // condition if the ball is threw and in the air, it would run the logic that it will goes towards
        // the ring in a parabolic way.

        if (IsBallFlying)
        {
            Debug.Log("Slider Value: " + chargeSlider.value);

            Ball.GetComponent<BallController>().isStateFlying = true;

            

            T += Time.deltaTime;
            //float duration = 0.86f;
            float duration = ballTravelSpeed;
            float t01 = T / duration;

            Vector3 A = PosOverHead.position;
            Vector3 B;

            // slider mechanics for accuracy of the shot
            // also determines if the ball is going to the net accurately or not
            if (chargeSlider.value > correctShotRangeMin && chargeSlider.value < correctShotRangeMax)
            {
                B = Target.position;
            }
            else
            {
               // error shot
                B = Target.position + new Vector3(errorRangeX, 0, -3.0f);

                if (isArcade)
                {
                    PlayerPointingSystem.Instance.ResetPoint();
                }
                if (isPlay)
                {
                    PlayerPointingSystem.Instance.ResetStreak();
                }
            }


            // make the ball travel in arc position 
            Debug.Log("Slider Value: " + B);
            Vector3 pos = Vector3.Lerp(A, B, t01); // add lerp to the ball overhead position to the target destination which is the net
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * Mathf.PI);  // this line adds arc path to the ball to travel
            Ball.position = pos + arc; // move the ball

            // if the ball completed its travelling using the interpolation variable t01, set the ball into not flying and etc.
            if (t01 >= timeRange)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
                Ball.GetComponent<BallController>().isStateFlying = false;
                Ball.GetComponent<BallController>().OnPlayer = false;
                Ball = null;
                
                cameraSystem.SetTarget(transform);

               
            }
            Debug.Log(t01);
        }

        // condition for the slider charge
        if (isCharging)
        {
            Charging();
        }
    }
    // dribble sounds using randomizer to add dynamics
    private void PlayRandomDribbleSound()
    {
        if (dribbleSounds.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, dribbleSounds.Length);
            audioSource.clip = dribbleSounds[randomIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No dribble sounds assigned or AudioSource missing.");
        }
    }
    // Slider Mechanics
    void Charging()
    {
        if (isIncreasing)
        {
            chargeSlider.value += sliderSpeed * Time.deltaTime;
            if (chargeSlider.value >= 1.0f)
            {
                chargeSlider.value = 1.0f;
                isIncreasing = false;
            }
        }
        else
        {
            chargeSlider.value -= sliderSpeed * Time.deltaTime;
            if (chargeSlider.value <= 0.0f)
            {
                chargeSlider.value = 0.0f;
                isIncreasing = true;
            }
        }
    }

    // when the player is charging his shot
    private void StartCharging()
    {
        if (isCharging)
        {
            return;
           
        }

        if (!IsBallInHands)
        {
            return;
        }
        animator.SetBool("ShootCharge", true);

        chargeShootBallButton.gameObject.SetActive(false);

        isCharging = true;
        chargeSlider.value = 0;

        chargeSlider.gameObject.SetActive(true);

        launchBallButton.gameObject.SetActive(true);

        // make the player face the net
        LookAtTarget(Target.transform);

        playerMovement.canMove = false;
    }

    void LookAtTarget(Transform target)
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = rotation;
            }
        }
    }

    private void Shoot()
    {
        if (!IsBallInHands)
        {
            return; 
        }
        cameraSystem.SetTarget(Ball);

        animator.SetBool("ShootCharge", false);
        chargeShootBallButton.gameObject.SetActive(true);
        launchBallButton.gameObject.SetActive(false);

        if (!isCharging)
            return;

        isCharging = false;
        chargeSlider.gameObject.SetActive(false);

        IsBallInHands = false;

        errorRangeX = Random.Range(-errorRange, errorRange);
        //errorRangeZ = Random.Range(-errorRange, errorRange);

        IsBallInHands = false;
        IsBallFlying = true;

        T = 0;
        playerMovement.canMove = true;

        SetNextSpot();

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            if (!Ball)
            {
                Ball = other.gameObject.transform;
                IsBallInHands = true;

                other.gameObject.GetComponent<BallController>().OnPlayer = true;
            }
        }
    }
}
