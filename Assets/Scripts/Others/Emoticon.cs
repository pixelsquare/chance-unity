﻿using UnityEngine;
using System.Collections;
using GameUtilities.NpcUtility;

public enum EmoticonNameID { Happy, Sad }

public class Emoticon : MonoBehaviour {
	private SpriteRenderer spriteRenderer;
	private Animator emoticonAnimator;

	private AnimatorStateInfo emoticonStateInfo;
	private int emoticonHashID;

	private float randomEmoticon;
	private bool emoticonEnabled;

	private GameManager gameManager;
	private PlayerInformation playerInformation;

	public bool EmoticonEnabled {
		get { return emoticonEnabled; }
		set { 
			emoticonEnabled = value;
			spriteRenderer.enabled = value;
		}
	}

	private void Start() {
		gameManager = GameManager.current;
		spriteRenderer = GetComponent<SpriteRenderer>();
		emoticonAnimator = GetComponent<Animator>();
		randomEmoticon = 0f;
		EmoticonEnabled = false;
		emoticonStateInfo = emoticonAnimator.GetCurrentAnimatorStateInfo(0);
		emoticonHashID = Animator.StringToHash("Base Layer.None");
	}

	private void Update() {
		emoticonStateInfo = emoticonAnimator.GetCurrentAnimatorStateInfo(0);
		if (IsAnimating()) {
			if (gameManager.BasePlayerData != null && playerInformation == null) {
				playerInformation = gameManager.BasePlayerData.PlayerInformation;
			}
			if (emoticonEnabled && playerInformation != null) {
				transform.LookAt(playerInformation.PlayerCamera.transform.position);
			}
		}
		else {
			randomEmoticon = 0f;
		}
	}

	public void RunEmoticon(EmoticonNameID nameID) {
		EmoticonEnabled = true;
		randomEmoticon = Random.Range(0.1f, 0.9f);
		if (nameID == EmoticonNameID.Happy) {
			emoticonAnimator.SetFloat(NpcUtility.EmoticonHappy, randomEmoticon);	
		}
		else if (nameID == EmoticonNameID.Sad) {
			emoticonAnimator.SetFloat(NpcUtility.EmoticonSad, randomEmoticon);
		}
		emoticonAnimator.SetBool("Run", true);
		StartCoroutine(EmoticonDelay());
	}

	private IEnumerator EmoticonDelay() {
		yield return new WaitForSeconds(2f);
		emoticonAnimator.SetBool("Run", false);
		EmoticonEnabled = false;
	}

	private bool IsAnimating() {
		return emoticonStateInfo.nameHash != emoticonHashID;
	}
}