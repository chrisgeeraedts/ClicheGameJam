using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCColliderScript : MonoBehaviour
{
    public int Phase;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

}
