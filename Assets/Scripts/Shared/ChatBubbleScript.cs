using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubbleScript : MonoBehaviour
{
    public TMP_Text TextElement;
    private GameObject _parent;

    public void Say(string text, int bubbleLayer, GameObject parent, float showDuration)
    {
        //gameObject.transform.SetParent(parent.transform);
        _parent = parent;
        // prepare bubble
        TextElement.text = text;
        // set layer
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = bubbleLayer;
        // Hide chat bubble
        StartCoroutine(HideBubble(showDuration));
    }

    IEnumerator HideBubble(float showDuration)
    {
        yield return new WaitForSeconds(showDuration);
        Destroy(gameObject);
    }



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector2(_parent.transform.position.x + 0.6f, _parent.transform.position.y + 1.71f);
    }
}
