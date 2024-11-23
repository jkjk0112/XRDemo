using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public GridSlot[] gridSlots; // 배열 슬롯들
    public GameObject trainObject; // 활성화할 기차 오브젝트
    private bool isTrainActivated = false; // 기차 활성화 여부

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
            Debug.Log($"{slot.name}의 상태: {slot.IsCorrectBlockPlaced()}");
            if (!slot.IsCorrectBlockPlaced())
            {
                return false; // 아직 완성되지 않음
            }
        }
        Debug.Log("모든 슬롯이 올바르게 채워졌습니다!");
        return true; // 모든 슬롯이 올바르게 채워짐
    }

    private void ActivateTrain()
    {
        if (trainObject == null)
        {
            Debug.LogError("trainObject가 null입니다! Inspector에서 연결되었는지 확인하세요.");
            return;
        }

        isTrainActivated = true; // 활성화 상태로 변경
        trainObject.SetActive(true); // 기차 활성화

        TrainMovement trainMovement = trainObject.GetComponent<TrainMovement>();
        if (trainMovement != null)
        {
            trainMovement.StartMoving(); // 기차 이동 시작
            Debug.Log("기차가 이동을 시작합니다.");
        }
        else
        {
            Debug.LogError("TrainMovement 스크립트가 trainObject에 없습니다!");
        }
    }
}
