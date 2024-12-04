//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Card : MonoBehaviour
//{
//    public int CardID;
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

using UnityEngine;

public class Card : MonoBehaviour
{
    public int CardID; // ī�� ���� ID
    public bool isFlipped = false; // ���� ������ ����
    public bool isMatched = false; // ��Ī ����

    private FlipCard flipCard;

    private void Start()
    {
        flipCard = GetComponent<FlipCard>(); // FlipCard ��ũ��Ʈ ����
    }

    // ī�带 ������ �޼���
    public void Flip()
    {
        if (!isMatched) // ��Ī�� ī��� ������ ����
        {
            isFlipped = !isFlipped;
            flipCard.Flip();
        }
    }
     
    // ī�� ��Ī ���� �� ȣ��
    public void MarkAsMatched()
    {
        isMatched = true;
    }
}
