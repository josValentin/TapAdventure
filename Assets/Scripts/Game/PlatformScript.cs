using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;
    public GameObject obstacle;
    private bool startTimer;
    private float fallTime;
    private Rigidbody2D my_Body;
    [HideInInspector]
    public bool SonicSkill = false;

    private void Awake()
    {
        my_Body = GetComponent<Rigidbody2D>();
    }
    public void Init(Sprite sprite, float fallTime, int obstacleDir)
    {
        my_Body.bodyType = RigidbodyType2D.Static;
        this.fallTime = fallTime;
        startTimer = true;
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprite;
        }

        if (obstacleDir == 0)//朝右边
        {
            if (obstacle != null)
            {
                obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x,
                    obstacle.transform.localPosition.y, 0);
            }
        }
    }
    private void Update()
    {
        if (!PlayerController.Instance.isDead)
        {
            if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.PlayerIsMove == false) return;

            if (startTimer)
            {
                fallTime -= Time.deltaTime;
                if (fallTime < 0)//倒计时结束
                {
                    //掉落
                    startTimer = false;
                    if (my_Body.bodyType != RigidbodyType2D.Dynamic)
                    {
                        my_Body.bodyType = RigidbodyType2D.Dynamic;
                        if (SonicSkill)
                        {
                            my_Body.gravityScale = 0.1f;
                        } else
                        {
                            StartCoroutine(DealyHide());
                            if(my_Body.gravityScale != 1f)
                            {
                                my_Body.gravityScale = 1f;
                            }
                        }

                    }
                }
            }
            if (transform.position.y - Camera.main.transform.position.y < -6)
            {
                StartCoroutine(DealyHide());
            }
        }

        if (GameObject.FindGameObjectWithTag("GamePanel").GetComponent<GamePanel>().getDropSlowly() && !SonicSkill)
        {
            SonicSkill = true;
        } else if(!GameObject.FindGameObjectWithTag("GamePanel").GetComponent<GamePanel>().getDropSlowly() && SonicSkill)
        {
            SonicSkill = false;
        }

    }
    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
