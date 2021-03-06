﻿using UnityEngine;
using System.Collections;

public class BulletTime : MonoBehaviour
{
	public float bulletTime = 1000.0f;
	public bool bulletTimeOn = false;
	public static bool bulletTimeOnStatic = false;
	public float bulletTimeScale = 2f;
	public float bulletSpeed = 100f;
	bool debugMode = true;

	BTS global;
	GameObject player;
	PlayerController playerC;
	Rigidbody2D playerR;
	PlayerController walkSpeed;
	GameObject[] gunmen;
	GameObject[] sentries;

	void Start()
	{
		global = GameObject.Find("GameScripts").GetComponent<BTS>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerC = player.GetComponent<PlayerController>();
		playerR = player.GetComponent<Rigidbody2D>();
		walkSpeed = player.GetComponent<PlayerController>();
		StartCoroutine(DrainBulletTime());

		gunmen = GameObject.FindGameObjectsWithTag("Gunman");

		sentries = GameObject.FindGameObjectsWithTag("Sentry");
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.LeftShift) && !bulletTimeOn)
		{
			StartBulletTime();
		}
		else if (!Input.GetKey(KeyCode.LeftShift) && bulletTimeOn)
		{
			StopBulletTime();
		}

		if (bulletTimeOn && !bulletTimeOnStatic)
		{
			bulletTimeOnStatic = true;
		}
		else if (!bulletTimeOn && bulletTimeOnStatic)
		{
			bulletTimeOnStatic = false;
		}
	}

	void StartBulletTime()
	{
		if (bulletTime > 0)
		{
			try
			{
				bulletTimeOn = true;
				bulletSpeed /= bulletTimeScale * 4f;
				walkSpeed.speed /= bulletTimeScale;
                rigidbody2D.mass = 11f;
                rigidbody2D.gravityScale = 0.7f;
                playerC.jumpForce = 1400;

				for (int i = 0; i < global.sentryCount; i++)
				{
					sentries[i].GetComponent<Sentry>().timeToWait *= bulletTimeScale;
				}

				for (int i = 0; i < global.gunmanCount; i++)
				{
					gunmen[i].GetComponent<Gunman>().gunRotationSpeed /= 4f;
				}

			}
			catch (System.NullReferenceException e)
			{
				Debug.LogError(e);
			}
		}
		else
		{
			Debug.Log("no BT left");
		}
	}

	void StopBulletTime()
	{
		try
		{
			bulletTimeOn = false;
			bulletSpeed *= bulletTimeScale * 4f;
			walkSpeed.speed *= bulletTimeScale;
            rigidbody2D.mass = 1f;
            rigidbody2D.gravityScale = 2f;
            playerC.jumpForce = 200;

			for (int i = 0; i < global.sentryCount; i++)
			{
				sentries[i].GetComponent<Sentry>().timeToWait /= bulletTimeScale;
			}

			for (int i = 0; i < global.gunmanCount; i++)
			{
				gunmen[i].GetComponent<Gunman>().gunRotationSpeed *= 4f;
			}
		}
		catch (System.NullReferenceException)
		{

		}
	}

	void OnGUI()
	{
		GUI.TextArea(new Rect(10.0f, 10.0f, 70.0f, 20.0f), bulletTime.ToString() + ", " + bulletTimeOn.ToString());
	}

	IEnumerator DrainBulletTime()
	{
		if (debugMode == false)
		{
			while (true)
			{
				if (bulletTimeOn)
				{
					if (bulletTime > 0 && bulletTime <= 1000)
					{
						bulletTime--;
					}
					else if (bulletTime <= 0)
					{
						StopBulletTime();
						Debug.Log("no BT left");
					}

					yield return new WaitForSeconds(0.005f);
				}

				yield return 0;
			}
		}
	}
}
