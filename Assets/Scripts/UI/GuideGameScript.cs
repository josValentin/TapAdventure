using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       if( GameManager.Instance.GetGameScore() > 3)
        {
            anim.SetBool("Dissapear", true);
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        anim.SetBool("Dissapear", false);

    }
}
