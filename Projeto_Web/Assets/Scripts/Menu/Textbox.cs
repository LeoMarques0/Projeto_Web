using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour
{

    public Text textbox;

    public bool hasAnimationNext;

    public Animator anim;
    public string animationName;

    [TextArea]
    public List<string> dialogues;

    int index = 0;
    bool writing = false, continueText = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!writing && continueText)
        {
            StartCoroutine(CompleteText(dialogues[index]));
        }

        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (index < dialogues.Count)
            {
                if (writing)
                {
                    StopAllCoroutines();
                    textbox.text = dialogues[index];
                    writing = false;
                    index++;
                }
                else
                    continueText = true;
            }
            else if(hasAnimationNext)
                anim.Play(animationName);
        }
    }

    IEnumerator CompleteText(string currentText)
    {
        continueText = false;
        writing = true;

        textbox.text = "";

        for(int x = 0; x < currentText.Length; x++)
        {
            string currentTextbox = textbox.text;
            textbox.text = currentTextbox + currentText[x];

            yield return new WaitForSeconds(.1f);
        }
        index++;
    }
}
