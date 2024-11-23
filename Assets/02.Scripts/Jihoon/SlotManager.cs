using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public GridSlot[] gridSlots; // �迭 ���Ե�
    public GameObject trainObject; // Ȱ��ȭ�� ���� ������Ʈ
    private bool isTrainActivated = false; // ���� Ȱ��ȭ ����

    void Update()
    {
        if (!isTrainActivated && CheckTrainCompletion())
        {
            ActivateTrain();
        }
    }

    private bool CheckTrainCompletion()
    {
        foreach (GridSlot slot in gridSlots)
        {
            Debug.Log($"{slot.name}�� ����: {slot.IsCorrectBlockPlaced()}");
            if (!slot.IsCorrectBlockPlaced())
            {
                return false; // ���� �ϼ����� ����
            }
        }
        Debug.Log("��� ������ �ùٸ��� ä�������ϴ�!");
        return true; // ��� ������ �ùٸ��� ä����
    }

    private void ActivateTrain()
    {
        if (trainObject == null)
        {
            Debug.LogError("trainObject�� null�Դϴ�! Inspector���� ����Ǿ����� Ȯ���ϼ���.");
            return;
        }

        isTrainActivated = true; // Ȱ��ȭ ���·� ����
        trainObject.SetActive(true); // ���� Ȱ��ȭ

        TrainMovement trainMovement = trainObject.GetComponent<TrainMovement>();
        if (trainMovement != null)
        {
            trainMovement.StartMoving(); // ���� �̵� ����
            Debug.Log("������ �̵��� �����մϴ�.");
        }
        else
        {
            Debug.LogError("TrainMovement ��ũ��Ʈ�� trainObject�� �����ϴ�!");
        }
    }
}
