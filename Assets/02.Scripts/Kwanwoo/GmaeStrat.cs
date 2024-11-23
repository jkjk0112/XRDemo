using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public XylophoneGame game;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Stick" && !game.isSequenceRunning)
        {
            Debug.Log("���� ����");
            game.SelectRandomSong(); // ���� ���� �� �������� �� ����
            StartCoroutine(game.StartCircleSequence()); // ���õ� �� ������ ����

        }
    }
}
