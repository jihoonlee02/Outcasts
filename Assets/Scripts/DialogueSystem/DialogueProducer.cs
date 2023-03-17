using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueProducer : TextProducer
{
    private bool running;
    protected override IEnumerator TypeWriterEffect(string a_text, float a_speed, float a_delay)
    {
        running = true;
        float soundDelay = Time.time;
        char c;
        for (int n = 0; n < a_text.Length && running; n++)
        {
            c = a_text[n];
            if (Time.time >= soundDelay)
            {
                soundSource?.Play();
                soundDelay = 0.1f + Time.time;
            }
        
            m_textLabel.text += c;
            yield return new WaitForSeconds(PunctuationTime(c) + (1 / (a_speed * 10f)));
        }

        yield return new WaitForSeconds(a_delay);

        m_textLabel.text = a_text;
        running = false;
    }
}
