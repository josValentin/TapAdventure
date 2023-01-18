using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{

	bool isRewinding = false;

	public float recordTime = 5f;

	public List<PointInTime> pointsInTime;

	Rigidbody2D rb;

	// Use this for initialization
	void Start()
	{
		pointsInTime = new List<PointInTime>();
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		/*if (Input.GetKeyDown(KeyCode.Return))
			StartRewind();
		if (Input.GetKeyUp(KeyCode.Return))
			StopRewind();*/
	}

	void FixedUpdate()
	{
		if (isRewinding)
			Rewind();
		else
			Record();
	}

	void Rewind()
	{
		if (pointsInTime.Count > 0)
		{
			PointInTime pointInTime = pointsInTime[0];
			transform.position = pointInTime.position;
			transform.rotation = pointInTime.rotation;
			pointsInTime.RemoveAt(0);
		}
		else
		{
			StopRewind();
		}

	}

	void Record()
	{
		if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
		{
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}

		pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
	}

	public void StartRewind()
	{
		isRewinding = true;
		rb.isKinematic = true;
	}

	public void StopRewind()
	{
		isRewinding = false;
		rb.isKinematic = false;
		PlayerController.Instance.gameObject.GetComponent<BoxCollider2D>().enabled = true;
		GameManager.Instance.IsGameOver = false;
		PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().enabled = true;

		PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
		StartCoroutine(WaitForPlayerd());
	}

	public IEnumerator WaitForPlayerd()
    {
		yield return new WaitForSeconds(1f);
		PlayerController.Instance.isDead = false;

	}
}
