using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public EasyExpandableTextBox easyText;
 
    void Start()
    {
        StartCoroutine(Text());
    }

    private IEnumerator Text()
    {
        yield return StartCoroutine(easyText.EasyMessage("Hi! Welcome to the example scene! (Press mouse left to make the text continue)."));
        yield return StartCoroutine(easyText.EasyMessage("I'm waiting until you press a button, using the easyText.EasyMessage with the yield return StartCoroutine expression!"));
        yield return StartCoroutine(easyText.EasyMessage("Isn't it interesting?"));
        yield return StartCoroutine(easyText.EasyMessage("Also, I can continue the messages on my own, ignoring your input.", 0.1f, false, false, 1.5f));
        yield return StartCoroutine(easyText.EasyMessage("Like this.", 0.1f, false, false, 1.5f));
        yield return StartCoroutine(easyText.EasyMessage(@"Thank you so much for buying this asset! \(^-^)/"));
    }
}
