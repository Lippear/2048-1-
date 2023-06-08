using UnityEngine;
using System;
using System.Collections;

public class Swipeing : MonoBehaviour
{
    [SerializeField] private float timeToFreezeGame;
    [SerializeField] private float _lowImpusle;
    [SerializeField] private float _normalImpulse;
    [SerializeField] private float _highImpusle;
    [SerializeField] private GameObject _2Cube;
    [SerializeField] private GameObject _4Cube;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _secondsForMerge;


    private bool newCubeIsReady=true;
    private Vector2 inputPoint;
    private Vector2 unInputPoint;
    private float timeToUnInput;
    private bool gameIsFreezed;
    private bool letToMerge;


    public static event Action<float, bool> SwipeHappend;
    public static event Action SwipeDown;
    public static event Action SwipeUp;
    public static event Action <bool>LetMerge;

    private void OnEnable()
    {
        Cube.RequestToMerge += LetToMerge;
    }
    private void OnDisable()
    {
        Cube.RequestToMerge -= LetToMerge;
    }
    private void LetToMerge()
    {
        LetMerge?.Invoke(letToMerge);
    }

    private void Update()
    {
        if (gameIsFreezed == false) 
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    inputPoint = touch.position;
                    unInputPoint = touch.position;
                    timeToUnInput = 0f;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    unInputPoint = touch.position;
                    timeToUnInput += Time.deltaTime;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    StartCoroutine(FrezzeGame(timeToFreezeGame));
                    float deltaX = Mathf.Abs(inputPoint.x - unInputPoint.x);
                    float deltaY = Mathf.Abs(inputPoint.y - unInputPoint.y);

                    if (deltaX > deltaY)
                    {
                        if (inputPoint.x > unInputPoint.x)
                        {
                            float distance = Vector2.Distance(inputPoint, unInputPoint);
                            float speed = distance / timeToUnInput;
                            Debug.Log("X-Swipe speed: " + speed);
                            WhereAndHowPowerIsSwipe(speed, true);
                        }
                        else
                        {
                            float distance = Vector2.Distance(inputPoint, unInputPoint);
                            float speed = distance / timeToUnInput;
                            Debug.Log("X-Swipe speed: " + speed);
                            WhereAndHowPowerIsSwipe(speed, false);
                        }
                    }
                    else if (deltaY > deltaX)
                    {
                        if (inputPoint.y > unInputPoint.y)
                        {
                            if (newCubeIsReady==true) 
                            {
                                Debug.Log("Swipe down");
                                SwipeDown?.Invoke();
                                newCubeIsReady = false; 
                            }
                            
                        }
                        else
                        {
                            if (newCubeIsReady==false)
                            {
                                Debug.Log("Swipe up");
                                SpawnCube();
                            }
                        }
                    }
                }
            }
        }
    }

    private IEnumerator LetToMerge(float seconds)
    {
        letToMerge = true;
        yield return new WaitForSeconds(seconds);
        letToMerge = false;
    }

    public void WhereAndHowPowerIsSwipe(float speed, bool leftSwipe)
    {
        if (speed < 10000)
        {
            SwipeHappend?.Invoke(_lowImpusle, leftSwipe);
        }
        else if(speed > 10000 && speed < 20000)
        {
            SwipeHappend?.Invoke(_normalImpulse, leftSwipe);
        }
        else
        {
            SwipeHappend?.Invoke(_highImpusle, leftSwipe);
            StartCoroutine(LetToMerge(_secondsForMerge));
        }
    }

    private IEnumerator FrezzeGame(float time)
    {
        gameIsFreezed = true;
        yield return new WaitForSeconds(time);
        gameIsFreezed = false;
    }

    public void SpawnCube()
    {
        int spawnChanceOfCubes = UnityEngine.Random.Range(1, 5);
        if (spawnChanceOfCubes == 1)
        {
            Instantiate(_4Cube, _spawnPoint.position, _spawnPoint.rotation);
        }
        else
        {
            Instantiate(_2Cube, _spawnPoint.position, _spawnPoint.rotation);
        }

        newCubeIsReady = true;
    }
}
