using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueProducer : TextProducer
{
    private bool running;
    protected override IEnumerator TypeWriterEffect(string a_text, float a_speed, float a_delay)
    {
        running = true;
        //StartCoroutine(SkipText());
        float soundDelay = Time.time;
        for (int n = 0; n < a_text.Length && running; n++)
        {
            char character = (char) a_text.Substring(n, 1).ToCharArray()[0];
            if (Time.time >= soundDelay)
            {
                soundSource?.Play();
                soundDelay = 0.1f + Time.time;
            }
        
            m_textLabel.text += character;
            yield return new WaitForSeconds(PunctuationTime(character) + a_delay + (1 / (a_speed * 10f)));
        }
        m_textLabel.text = a_text;
        running = false;
    }

    //private IEnumerator SkipText()
    //{
    //    while (running)
    //    {
    //        running = !PlayerController.Instance.PIA.UserInteraction.Cancel.IsPressed();
    //        yield return null;
    //    }
    //}
}
