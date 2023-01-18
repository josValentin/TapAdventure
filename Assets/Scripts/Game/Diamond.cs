using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    private ManagerVars vars;
    private SpriteRenderer spriteR;
    // Start is called before the first frame update
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        spriteR = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
        spriteR.sprite = vars.DiamondsSpritesList[0];
    }

    // Update is called once per frame
    void Update()
    {
        int GetScore = GameManager.Instance.GetGameScore();

        if (GetScore <= 150)
        {
            //vars.diamondPre.GetComponent<SpriteRenderer>().sprite = vars.DiamondsSpritesList[0];

            spriteR.sprite = vars.DiamondsSpritesList[0];
        }
        else if (GetScore > 150 && GetScore <= 500)
        {
            //vars.diamondPre.GetComponent<SpriteRenderer>().sprite = vars.DiamondsSpritesList[1];

            spriteR.sprite = vars.DiamondsSpritesList[1];
        }
        else if (GetScore > 500)
        {
            //vars.diamondPre.GetComponent<SpriteRenderer>().sprite = vars.DiamondsSpritesList[2];

            spriteR.sprite = vars.DiamondsSpritesList[2];
        }
    }
}
