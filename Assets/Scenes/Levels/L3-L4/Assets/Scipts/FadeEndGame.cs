using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeEndGame : MonoBehaviour
{
    public GameObject fadeout;
    private Color objectColor;

    private void Start()
    {
        objectColor = fadeout.GetComponent<Image>().color;
        objectColor.a = 0;
        
    }
    void Update()
    {
        if (GameObject.Find("Gold") == null && GameObject.Find("Iron") == null && GameObject.Find("Nickel") == null && GameObject.Find("Silicate") == null)
        {
            if(GameObject.Find("Gold(Clone)") == null && GameObject.Find("Iron(Clone)") == null && GameObject.Find("Nickel(Clone)") == null && GameObject.Find("Silicate(Clone)") == null)
            {
                //StartCoroutine(Wrapper());

                LoadNext();

            }
            
        }
    }

    public IEnumerator Wrapper()
    {
        yield return StartCoroutine(FadeToBlack());

        LoadNext();
    }

    public IEnumerator FadeToBlack(int fadeSpeed = 3)
    {
        float fadeAmount;

            while (fadeout.GetComponent<Image>().color.a < 255)
            {
                if (fadeout.GetComponent<Image>().color.a > 255)
                {
                    break;
                }
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeout.GetComponent<Image>().color = objectColor;
                yield return new WaitForSeconds(.5f);
            }

        yield return null;
    }

    void LoadNext()
    {
        SceneManager.LoadScene("End");
    }
}
