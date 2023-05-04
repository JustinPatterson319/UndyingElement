using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject controls;

    public GameObject overview;
   
    public GameObject credits;

    [SerializeField] AudioClip select;
    [SerializeField] AudioClip start;

    public GameObject gameOverScreen;
    public GameObject winScreen;

    GameObject forGameOver;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        forGameOver = GameObject.Find("Essentials(Clone)");
        if(forGameOver != null)
        {
            if (forGameOver.GetComponent<EssentialObjects>().gameOver == true)
            {
                gameOverScreen.SetActive(true);
            }
            else
            {
                winScreen.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (controls.activeSelf != true && overview.activeSelf == false && credits.activeSelf == false && winScreen.activeSelf != true && gameOverScreen.activeSelf != true)
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                controls.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if(forGameOver != null)
            {
                gameOverScreen.SetActive(false);
                winScreen.SetActive(false);
                Destroy(forGameOver);
            }
            if (controls.activeSelf == true && overview.activeSelf == false)
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                controls.SetActive(false);
            }
            if (credits.activeSelf == true && overview.activeSelf == false)
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                credits.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {

            if (overview.activeSelf == true)
            {
                GetComponent<AudioSource>().clip = start;
                GetComponent<AudioSource>().Play(0);
                //Load Scene
                SceneManager.LoadScene("Starting Room");
            }
            else if (controls.activeSelf != true && credits.activeSelf != true && gameOverScreen.activeSelf != true && winScreen.activeSelf != true)
            {
                GetComponent<AudioSource>().clip = start;
                GetComponent<AudioSource>().Play(0);
                overview.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (controls.activeSelf != true && overview.activeSelf == false && credits.activeSelf == false)
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                credits.SetActive(true);
            }
        }
    }
}
