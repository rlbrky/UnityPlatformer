﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Trigger girdi");
        player.GetComponent<PlayerController>().onGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Triggerdan çıktı");
        player.GetComponent<PlayerController>().onGround = false;
    }
}
