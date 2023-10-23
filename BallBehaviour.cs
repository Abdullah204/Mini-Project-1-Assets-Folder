using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class BallBehaviour : MonoBehaviour
{
    public int redScore;
    public int greenScore;
    public int blueScore;
    public int totalScore;
    public char currentForm;
    public bool invincible;
    public bool multiplier;
    public bool shield;
    static readonly char[] colors = { 'R', 'G', 'B', 'O', 'E' };
    readonly ArrayList perms = GetPerms(string.Empty);
    public Material material;
    public TMP_Text _tmPro;
    public GameObject redOrb;
    public GameObject greenOrb;
    public GameObject blueOrb;
    public GameObject obstacle;
    public Transform spawnPoint;
    public GameObject player;
    private readonly List<GameObject> obstacles = new();
    private bool first = true;
    public GameObject restartButton;
    public GameObject resumeButton;
    public GameObject homeButton;
    public AudioClip audioClip;
    public GameObject pauseButton;
    public GameObject redButton;
    public GameObject greenButton;
    public GameObject blueButton;
    public GameObject powerButton;
    public static int finalScore = 0;
    public AudioClip collect;
    public AudioClip power;
    public AudioClip collision;
    public AudioClip switchform;
    public AudioClip wrong;
    public AudioClip slow;
    private AudioSource audioClipSource;
    private AudioSource collectAudioSource;
    private AudioSource powerAudioSource;
    private AudioSource collisionAudioSource;
    private AudioSource switchformAudioSource;
    private AudioSource wrongAudioSource;
    private AudioSource slowAudioSource;
    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        redScore = 0;
        blueScore = 0;
        greenScore = 0;
        totalScore = 0;
        currentForm = 'W';
        invincible = false;
        multiplier = false;
        shield = false;
        resumeButton.SetActive(false);
        restartButton.SetActive(false);
        homeButton.SetActive(false);
        audioClipSource = CreateAudioSource(audioClip);
        collectAudioSource = CreateAudioSource(collect);
        powerAudioSource = CreateAudioSource(power);
        collisionAudioSource = CreateAudioSource(collision);
        switchformAudioSource = CreateAudioSource(switchform);
        wrongAudioSource = CreateAudioSource(wrong);
        slowAudioSource = CreateAudioSource(slow);
        audioClipSource.loop = true;
        slowAudioSource.loop = true;
        MyPlay(audioClipSource);

        InstantiateHorizontalLine(40);
        InstantiateHorizontalLine(60);
        InstantiateHorizontalLine(80);
        InvokeRepeating(nameof(InstantiateHorizontalLineCaller), 1f, 1f);
    }

    private AudioSource CreateAudioSource(AudioClip clip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        return audioSource;
    }

    private void InstantiateHorizontalLineCaller()
    {
        InstantiateHorizontalLine(80);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle") && !collision.gameObject.CompareTag("Ground"))
        {
            PlayObstacleHit();
            CollectOrb(collision.gameObject.tag[0]);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (invincible)
            {
                collision.gameObject.GetComponent<Collider>().isTrigger = false;
                collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                player.GetComponent<Collider>().isTrigger = false;
            }
            else
            {

                ObstacleCollide();
                Destroy(collision.gameObject);
            }
        }
    }

    private void MyPlay(AudioSource s)
    {
        if (SoundManager.isSoundOn)
        {
            s.Play();
        }
    }

    private void MyPlay2(AudioSource s)
    {
        
        if (SoundManager.isSoundOn)
        {
            if (first)
            {
                first = false;
                s.Play();
            }
            else
            {

            s.UnPause();
            }
        }
    }

    private void InstantiateHorizontalLine(int z)
    {
        string charLine = GetRandomPerm();
        double x = -6.5;
        foreach (char c in charLine)
        {
            if (c == 'E')
                continue;
            GameObject newObject = Instantiate(GETCorrespondingPrefab(c), new Vector3((float)x, 1, spawnPoint.position.z + z), spawnPoint.rotation);
            if (c == 'O')
                obstacles.Add(newObject);
            StartCoroutine(DestroyCoroutine(newObject, z));
            x += 6.5;
        }

        IEnumerator DestroyCoroutine(GameObject gameObject, int z)
        {
            yield return new WaitForSeconds((float)z / 20 + 1);
            if (gameObject != null && obstacles.Contains(gameObject))
            {
                obstacles.Remove(gameObject);
            }
            Destroy(gameObject);
        }

    }

    public GameObject GETCorrespondingPrefab(char c)
    {
        if (c == 'R')
            return redOrb;
        if (c == 'G')
            return greenOrb;
        if (c == 'B')
            return blueOrb;
        return obstacle;


    }

    static ArrayList GetPerms(string permutation)
    {
        ArrayList perms = new();
        if (permutation.Length == 3)
        {
            if (permutation != "OOO")
            {
                perms.Add(permutation);
            }
            return perms;
        }

        for (int i = 0; i < colors.Length; i++)
        {
            perms.AddRange(GetPerms(permutation + colors[i]));
        }
        return perms;

    }

    private string GetRandomPerm()
    {
        return (string)perms[new System.Random().Next(0, perms.Count)];
    }



    void Update()
    {

        CheckPause();
        if (Time.timeScale == 0)
            return;
        AutomaticBallMove();
        CheckSwitchLane();
        CheckForm();
        CheckPower();
        CheckCheats();
        UpdatePlayerMaterial();
    }

    private void UpdatePlayerMaterial()
    {

        material.color = GetColorCorrespondingToCurrentForm();
    }

    private Color GetColorCorrespondingToCurrentForm()
    {
        if (currentForm == 'W')
            return Color.white;
        if (currentForm == 'R')
            return Color.red;
        if (currentForm == 'G')
            return Color.green;
        return Color.blue;

    }

    private void CheckCheats()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            invincible = !invincible;
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");


        }
        if (Input.GetKeyDown(KeyCode.I) && redScore < 5)
        {
            redScore++;
        }
        if (Input.GetKeyDown(KeyCode.O) && greenScore < 5)
        {
            greenScore++;
        }
        if (Input.GetKeyDown(KeyCode.P) && blueScore < 5)
        {
            blueScore++;
        }
        UpdateScoreLine();
    }











    private void CollectOrb(char orbColor)
    {
        int scoreIncrease = 0;
        int colorIncrease = 0;
        if (orbColor == currentForm)
        {
            scoreIncrease += 2;
        }
        else
        {
            scoreIncrease++;
            colorIncrease++;

        }
        if (multiplier)
        {
            scoreIncrease *= 5;
            colorIncrease *= 2;
            multiplier = false;
        }

        totalScore += scoreIncrease;
        if (orbColor == 'R')
        {
            redScore = Math.Min(5, redScore + colorIncrease);

        }
        else if (orbColor == 'G')
        {
            greenScore = Math.Min(5, greenScore + colorIncrease);
        }
        else
        {
            blueScore = Math.Min(5, blueScore + colorIncrease);
        }
        PlayCollectOrb();
        UpdateScoreLine();

    }

    private void UpdateScoreLine()
    {
        _tmPro.text = "R/G/B Points: " + redScore + "/" + greenScore + "/" + blueScore + "\n\n" + "Total Score: " + totalScore;
    }



    private void ObstacleCollide()
    {
        PlayObstacleHit();
        if (shield)
        {
            shield = false;
            return;
        }
        if (currentForm != 'W')
        {
            currentForm = 'W';
            multiplier = false;
            shield = false;

        }
        else
        {

            Invoke(nameof(SwitchToGameOverScreen), (float)0.1);
        }


    }
    
    private void SwitchToGameOverScreen()
    {
        finalScore = totalScore;
        SceneManager.LoadScene("Game Over");
    }

    public void GoRed()
    {

            if (redScore == 5)
            {
                PlaySwitchForm();
                redScore--;
                currentForm = 'R';
                multiplier = false;
                shield = false;
            }
            else
            {
                PlayNonApplicableSound();
            }
        UpdateScoreLine();
    }

    public void GoGreen()
    {
        if (greenScore == 5)
        {

            PlaySwitchForm();
            greenScore--;
            currentForm = 'G';
            shield = false;
        }
        else
        {
            PlayNonApplicableSound();
        }
        UpdateScoreLine();
    }

    public void GoBlue()
    {
        if (blueScore == 5)
        {

            PlaySwitchForm();
            blueScore--;
            currentForm = 'B';
            multiplier = false;
        }
        else
        {
            PlayNonApplicableSound();
        }
        UpdateScoreLine();
    }


    private void CheckForm()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GoRed();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GoGreen();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GoBlue();
        }
        

    }

    private void CheckPower()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UsePower();
        }
    }
    public void UsePower()
    {
        
            if (multiplier || shield || GetScoreOfForm(currentForm) == 0)
            {
                PlayNonApplicableSound();
                return;
            }
            if (currentForm == 'R')
            {
                PerformNuke();
            }
            else if (currentForm == 'G')
            {
                PerformMultiplier();
            }
            else if (currentForm == 'B')
            {
                PerformShield();
            }
            UpdateScoreLine();

        

    }

    private void PerformShield()
    {
        blueScore--;
        shield = true;
        PlayUsePower();
        if (blueScore == 0)
        {
            currentForm = 'W';
            PlaySwitchForm();
             shield = false;
        }


    }

    private void PlayUsePower()
    {
        MyPlay(powerAudioSource);
    }

    private void PerformMultiplier()
    {
        greenScore--;
        multiplier = true;
        MyPlay(powerAudioSource);
        if (greenScore == 0)
        {
            currentForm = 'W';
            PlaySwitchForm();
             multiplier = false;
        }

    }

    private void PerformNuke()
    {
        redScore--;
        MyPlay(powerAudioSource);
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        obstacles.Clear();
        if (redScore == 0)
        {
            currentForm = 'W';
            PlaySwitchForm();
        }

    }

    private void PlayNonApplicableSound()
    {
        MyPlay(wrongAudioSource);
    }

    private int GetScoreOfForm(char form)
    {

        if (form == 'R') return redScore;
        if (form == 'G') return greenScore;
        if (form == 'B') return blueScore;
        return 0;
    }

    private void PlayCollectOrb()
    {
        MyPlay(collectAudioSource);
    }

    private void PlaySwitchForm()
    {
        MyPlay(switchformAudioSource);
    }
    private void PlayObstacleHit()
    {
        MyPlay(collisionAudioSource);
    }

    private void CheckSwitchLane()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x < -9) return;
            MoveToPosition(transform.position.x - 500f);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x > 9) return;
            MoveToPosition(transform.position.x + 500f);
        }
    }

    private void AutomaticBallMove()
    {
        transform.Translate(20f * Time.deltaTime * Vector3.forward);
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        Time.timeScale = 1 - Time.timeScale;
        bool pause = Time.timeScale == 0;
        resumeButton.SetActive(pause);
        restartButton.SetActive(pause);
        homeButton.SetActive(pause);
        pauseButton.SetActive(!pause);
        redButton.SetActive(!pause);
        greenButton.SetActive(!pause);
        blueButton.SetActive(!pause);
        powerButton.SetActive(!pause);

        if (pause)
        {
            audioClipSource.Pause();
            MyPlay2(slowAudioSource);
        }
        else
        {
            slowAudioSource.Pause();
            MyPlay2(audioClipSource);

        }
    }



    void MoveToPosition(float targetX)
    {
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 20f * Time.deltaTime);
    }
}
