using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


    public class EasyExpandableTextBox : MonoBehaviour
    {
        public TextMeshProUGUI textTMP;
        public Sprite boxSprite;
        public TMP_FontAsset font;
        public float y_offset = 3f;
        [TextArea]
        public string text;

        public int fontSize = 72;
        public bool autoSize = false;
        public Color color = Color.black;
        public TextAlignmentOptions alignment;
        public int autoSizeMin = 12, autoSizeMax = 72; 
        [SerializeField]
        public FontStyles style = FontStyles.Normal;
        public bool settings;

        //Vertical Layout Group
        public int left;
        public int right;
        public int top;
        public int bottom;
        public TextAnchor boxAlignment;

        public AudioSource audioSource;
        [SerializeField]
        public List<AudioClip> typingSounds;

        private static bool mouseButtonPressed;

        private static readonly WaitUntil waitUntil = new (() => mouseButtonPressed);
        private readonly Dictionary<float, WaitForSeconds> waitPool = new();
        

        private GameObject _parent;
    
        public void Awake()
        {
            textTMP = GetComponentInChildren<TextMeshProUGUI>();
            audioSource = GetComponent<AudioSource>();
        }

        public void Update()
        {
            if (Input.GetButtonDown("Fire1")) 
            {
                mouseButtonPressed = true;
            }
            if(_parent != null)
            {
                gameObject.transform.position = new Vector2(_parent.transform.position.x, _parent.transform.position.y + y_offset);
            }
        }

        private WaitForSeconds GetPoolWait(float waitTime)
        {
            if (waitPool.TryGetValue(waitTime, out var wait)) return wait;
            // else
            wait = new WaitForSeconds(waitTime);
            waitPool.Add(waitTime, wait);

            return wait;
        }

        public void Show(GameObject parent, float _y_offset)
        {
            y_offset = _y_offset;
            _parent = parent;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    
        public IEnumerator EasyMessage(string message, float timeBetweenCharacters = 0.125f, bool canSkipText = true, bool waitForButtonClick = true, float timeToWaitAfterTextIsDisplayed = 1f)
        {
            text = "";
            textTMP.text = text;
            message += " ";
            if (timeBetweenCharacters != 0)
            {
                for (int i = 0; i < message.Length - 1; i++)
                {
                    if (message[i] != '<' && message[i + 1] != '#')
                    {
                        text += message[i];
                        textTMP.text = text;
                        if (mouseButtonPressed && canSkipText)
                        {
                            mouseButtonPressed = false;
                            text = message;
                            textTMP.text = text;
                            break;
                        }
                        if (message[i] == ' ') continue;
                        
                        audioSource.PlayOneShot(typingSounds[Random.Range(0, typingSounds.Count)]);
                        yield return GetPoolWait(timeBetweenCharacters);
                    }
                    else
                    {
                        for (int j = i; j <= message.IndexOf('>', i); j++) 
                            text += message[j];

                        i = message.IndexOf('>', i);
                    }
                }
            }
            else
            {
                text = message;
                textTMP.text = text;
            }

            if (waitForButtonClick) yield return waitUntil;
            else yield return GetPoolWait(timeToWaitAfterTextIsDisplayed);
        
            mouseButtonPressed = false;
            Hide();
        }
    }
