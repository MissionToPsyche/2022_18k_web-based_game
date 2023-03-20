using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public GameObject fadeintext;
    public GameObject fadeinbutton;
    private Color objectColortext;
    private Color objectColorbuttontext;
    private Color objectColorbutton;

    void Start()
    {
        //objectColortext = fadeintext.GetComponent<Text>().color;
        //objectColorbuttontext = fadeinbutton.GetComponent<Text>().color;
        //objectColorbutton = fadeinbutton.GetComponent<Image>().color;
        //objectColortext.a = 0;
        //objectColorbuttontext.a = 0;
        //objectColorbutton.a = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        //StartCoroutine(Fadein());
    }

    public IEnumerator Fadein(bool fadeToBlack = true, int fadeSpeed = 5)
    {
        float fadeAmount;

        if (fadeToBlack)
        {
            while (fadeintext.GetComponent<Image>().color.a < 255)
            {
                fadeAmount = objectColortext.a + (fadeSpeed * Time.deltaTime);

                objectColortext = new Color(objectColortext.r, objectColortext.g, objectColortext.b, fadeAmount);
                fadeintext.GetComponent<Image>().color = objectColortext;
                yield return new WaitForSeconds(.4f);
            }
            while (fadeinbutton.GetComponent<Text>().color.a < 255)
            {
                fadeAmount = objectColorbuttontext.a + (fadeSpeed * Time.deltaTime);

                objectColortext = new Color(objectColortext.r, objectColortext.g, objectColortext.b, fadeAmount);
                fadeinbutton.GetComponent<Text>().color = objectColortext;
                fadeinbutton.GetComponent<Image>().color = objectColortext;
                yield return new WaitForSeconds(.2f);
            }

        }
        else
            yield return new WaitForEndOfFrame();
    }

    public void ReturnButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
