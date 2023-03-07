using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideManager : MonoBehaviour
{
    private static SlideManager m_instance;
    public static SlideManager Instance { 
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SlideManager>();
            }
            return m_instance;
        }
    }

    [SerializeField] private Slide[] slides;
    private Slide currSlide;

    public Slide CurrSlide => currSlide;

    private int index = 0;

    public void NextSlide()
    {
        slides[index].gameObject.SetActive(false);
        GameManager.Instance.CurrLevelManager.NextLevel();
        index = (index + 1) % slides.Length;
    }

    public void PrevSlide()
    {
        slides[index].gameObject.SetActive(false);
        GameManager.Instance.CurrLevelManager.PrevLevel();
        index = (index - 1 < 0 ? (slides.Length - 1) : index - 1) % slides.Length;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrLevelManager.ViewingLevel)
        {
            slides[index].gameObject.SetActive(true);
            currSlide = slides[index];
        }

        //Dev
        if (Input.GetMouseButtonDown(0))
        {
            currSlide.AddInfo();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            currSlide.RemoveInfo();
        }
    }
}
