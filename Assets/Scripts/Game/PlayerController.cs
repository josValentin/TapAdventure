using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Linq;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;
    public Transform rayDown, rayLeft, rayRight;
    public LayerMask platformLayer, obstacleLayer;
    public bool isDead = false;

    private bool isMoveLeft = false;

    private bool isJumping = false;
    private Vector3 nextPlatformLeft, nextPlatformRight;
    private ManagerVars vars;
    private Rigidbody2D my_Body;
    private SpriteRenderer spriteRenderer;
    private bool isMove = false;
    private AudioSource m_AudioSource;
    private bool WillSurvive = false;
    private bool canSurvive = true;

    private bool DieByHit = false;

    public int WhichPlayer = 0;
    private void Awake()
    {
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.AddListener<float>(EventDefine.UpdateSliderBarSound, UpdateSliderBarSound);

        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        vars = ManagerVars.GetManagerVars();
        spriteRenderer = GetComponent<SpriteRenderer>();
        my_Body = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();

        Instance = this;
    }
    private void Start()
    {
        ChangeSkin(GameManager.Instance.GetCurrentSelectedSkin());
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.RemoveListener<float>(EventDefine.UpdateSliderBarSound, UpdateSliderBarSound);

    }

    private void IsMusicOn(bool value)
    {
        m_AudioSource.mute = !value;
    }

    private void UpdateSliderBarSound(float value)
    {
        m_AudioSource.volume = value;
    }

    private void ChangeSkin(int skinIndex)
    {
        spriteRenderer.sprite = vars.characterSkinSpriteList[skinIndex];
        WhichPlayer = skinIndex;
    }

    public SpriteRenderer GetPlayerSpriteRenderer()
    {
        return spriteRenderer;
    }
    private bool IsPointerOverGameObject(Vector2 mousePosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }
    private void Update()
    {
        Debug.DrawRay(rayDown.position, Vector2.down * 1, Color.red);
        Debug.DrawRay(rayLeft.position, Vector2.left * 0.15f, Color.red);
        Debug.DrawRay(rayRight.position, Vector2.right * 0.15f, Color.red);

        if (IsPointerOverGameObject(Input.mousePosition)) return;

        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver
            || GameManager.Instance.IsPause)
            return;

        if (Input.GetMouseButtonDown(0) && isJumping == false && nextPlatformLeft != Vector3.zero)
        {
            if (isMove == false)
            {
                EventCenter.Broadcast(EventDefine.PlayerMove);
                isMove = true;
            }
            m_AudioSource.PlayOneShot(vars.jumpClip);
            EventCenter.Broadcast(EventDefine.DecidePath);
            isJumping = true;
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x <= Screen.width / 2)
            {
                isMoveLeft = true;
            }
            else if (mousePos.x > Screen.width / 2)
            {
                isMoveLeft = false;
            }
            Jump();
        }

        if (my_Body.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
        {
            GameManager.Instance.Fall = true;

            isDead = true;


            StartCoroutine(DealyShowGameOverResetPanel());
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;

        }
        if (isJumping && IsRayObstacle() && GameManager.Instance.IsGameOver == false)
        {
            GameManager.Instance.Fall = true;

            m_AudioSource.PlayOneShot(vars.hitClip);
            DieByHit = true;
            GameObject go = ObjectPool.Instance.GetDeathEffect();
            go.SetActive(true);
            go.transform.position = transform.position;
            isDead = true;
            GameManager.Instance.IsGameOver = true;
            spriteRenderer.enabled = false;
            StartCoroutine(DealyShowGameOverResetPanel());

        }
        if (transform.position.y - Camera.main.transform.position.y < -5 && GameManager.Instance.IsGameOver == false)
        {
            GameManager.Instance.Fall = true;

            isDead = true;
            GameManager.Instance.IsGameOver = true;
            StartCoroutine(DealyShowGameOverResetPanel());

        }

    }
    IEnumerator DealyShowGameOverResetPanel()
    {

        yield return new WaitForSeconds(0.5f);
        if (!DieByHit)
        {
            if (WhichPlayer == 6)
            {
                int Rand = Random.Range(0, 3);
                if (Rand == 0)
                {
                    m_AudioSource.PlayOneShot(vars.FallsDeath[GameManager.Instance.GetCurrentSelectedSkin()]);
                }
                else if (Rand == 1)
                {
                    m_AudioSource.PlayOneShot(vars.SpongeBob2);

                }
                else if (Rand == 2)
                {
                    m_AudioSource.PlayOneShot(vars.SpongeBob3);
                }
            }
            else
            {
                m_AudioSource.PlayOneShot(vars.FallsDeath[GameManager.Instance.GetCurrentSelectedSkin()]);

            }
        }


        yield return new WaitForSeconds(0.5f);

        GameObject[] NearPlatforms = GameObject.FindGameObjectsWithTag("Platform");
        for (int i = 0; i < NearPlatforms.Length; i++)
        {
            Vector2 PlatformNearestPos = (Vector2)NearPlatforms[i].transform.position;

            Vector2 CameraPosY = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y - 2.6f);
            float dist = Vector2.Distance(PlatformNearestPos, CameraPosY);
            if (dist <= 0.93f)
            {

                WillSurvive = true;
                break;
            }
            else
            {
                WillSurvive = false;
            }
        }
        if ((WillSurvive && (int)(GameManager.Instance.GetGameDiamond() * 0.2f) != 0 && WhichPlayer == 7) || (WillSurvive && WhichPlayer == 10))
        {
            if (canSurvive)
            {
                EventCenter.Broadcast(EventDefine.ShowResetPanel);
                canSurvive = false;
            }
            else
            {
                EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
            }

        }
        else
        {
            EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
        }
    }



    private GameObject lastHitGo = null;
    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                if (lastHitGo != hit.collider.gameObject)
                {
                    if (lastHitGo == null)
                    {
                        lastHitGo = hit.collider.gameObject;
                        return true;
                    }
                    EventCenter.Broadcast(EventDefine.AddScore);
                    lastHitGo = hit.collider.gameObject;
                }
                return true;
            }
        }
        return false;
    }
    private bool IsRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);

        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "Obstacle")
            {
                return true;
            }
        }

        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "Obstacle")
            {
                return true;
            }
        }
        return false;
    }
    private void Jump()
    {
        if (isJumping)
        {
            if (isMoveLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.DOMoveX(nextPlatformLeft.x, 0.2f);
                transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
            }
            else
            {
                transform.DOMoveX(nextPlatformRight.x, 0.2f);
                transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
                transform.localScale = Vector3.one;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            isJumping = false;
            Vector3 currentPlatformPos = collision.gameObject.transform.position;
            nextPlatformLeft = new Vector3(currentPlatformPos.x -
                vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x +
                vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Pickup")
        {
            if (WhichPlayer == 11)
            {
                m_AudioSource.PlayOneShot(vars.diamondClip2);

            }
            else if (WhichPlayer == 9 || WhichPlayer == 10)
            {
                m_AudioSource.PlayOneShot(vars.diamondClip3);
            }
            else
            {
                m_AudioSource.PlayOneShot(vars.diamondClip);

            }
            EventCenter.Broadcast(EventDefine.AddDiamond);

            collision.gameObject.SetActive(false);
        }
    }
}
