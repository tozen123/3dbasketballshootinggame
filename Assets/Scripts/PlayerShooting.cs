using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float sliderSpeed = 0.85f;
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

    // Private variables
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

    bool canShoot = false;


    
    void Start()
    {
        cameraSystem = Camera.main.GetComponent<CameraSystem>();
        audioSource = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<AudioSource>();

        chargeSlider.gameObject.SetActive(false);


        if (SceneManager.GetActiveScene() != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case "PlayLevel1":
                    sliderSpeed = 0.85f;
                    break;
                case "PlayLevel2":
                    sliderSpeed = 1.25f;
                    break;
                case "PlayLevel3":
                    sliderSpeed = 1.75f;
                    break;
                case "PlayLevel4":
                    sliderSpeed = 2.10f;
                    break;
                case "PlayLevel5":
                    sliderSpeed = 2.25f;
                    break;
                default:
                    break;
            }
        }

        if (spots.Count > 0)
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
                spot.GetChild(0).GetComponent<Renderer>().enabled = false;
            }

            currentSpot = spots[Random.Range(0, spots.Count)];
            currentSpot.GetChild(0).GetComponent<Renderer>().enabled = true;
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
     

        if (!Ball)
        {
            IsBallInHands = false;
        }

        if (isArcade)
        {
            if (IsBallInHands)
            {
                chargeShootBallButton.interactable = true;
                canShoot = true;


            }
        }

        if (spots.Count > 0)
        {
            if (!IsInCorrectSpot())
            {
                chargeShootBallButton.interactable = false;
                canShoot = false;
            }
            else
            {
                if (IsBallInHands)
                {
                    chargeShootBallButton.interactable = true;
                    canShoot = true;


                }
            }
        }

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

        if (IsBallFlying)
        {
            Ball.GetComponent<BallController>().isStateFlying = true;

            T += Time.deltaTime;
            float duration = ballTravelSpeed;
            float t01 = T / duration;

            Vector3 A = PosOverHead.position;
            Vector3 B = (chargeSlider.value > correctShotRangeMin && chargeSlider.value < correctShotRangeMax)
                        ? Target.position
                        : Target.position + new Vector3(errorRangeX, 0, -3.0f);



            if (chargeSlider.value > correctShotRangeMin && chargeSlider.value < correctShotRangeMax)
            {
                B = Target.position;
            }
            else
            {
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


            Vector3 pos = Vector3.Lerp(A, B, t01);
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * Mathf.PI);
            Ball.position = pos + arc;

            if (t01 >= timeRange)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
                Ball.GetComponent<BallController>().isStateFlying = false;
                Ball.GetComponent<BallController>().OnPlayer = false;
                Ball = null;

                cameraSystem.SetTarget(transform);
            }
        }

        if (isCharging)
        {
            Charging();
        }
    }

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

    public void StartCharging()
    {
        if (!canShoot) return;
        if (isCharging || !IsBallInHands) return;

        animator.SetBool("ShootCharge", true);
        isCharging = true;
        chargeSlider.value = 0;
        chargeSlider.gameObject.SetActive(true);
        LookAtTarget(Target.transform);
        playerMovement.canMove = false;
    }

    public void Shoot()
    {
        if (!IsBallInHands || !isCharging) return;

        cameraSystem.SetTarget(Ball);
        animator.SetBool("ShootCharge", false);
        isCharging = false;
        chargeSlider.gameObject.SetActive(false);
        IsBallInHands = false;
        errorRangeX = Random.Range(-errorRange, errorRange);
        IsBallFlying = true;
        T = 0;
        playerMovement.canMove = true;

        SetNextSpot();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball" && !Ball)
        {
            Ball = other.gameObject.transform;
            IsBallInHands = true;
            other.gameObject.GetComponent<BallController>().OnPlayer = true;
        }
    }

  
}
