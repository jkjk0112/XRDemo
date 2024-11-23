using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public string requiredTag; // 해당 슬롯에 필요한 블럭의 태그
    private bool isOccupied = false; // 슬롯이 점유되었는지 여부
    public ActiveState activeState; // 블럭이 잡혀있는지 확인
    public bool isBottomSlot = false; // 맨 아래 슬롯인지 여부

    private void Update()
    {
        // Ray 시각화를 Update에서 지속적으로 실행
        DrawDebugRay();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 맨 아래 슬롯이 아니면서 아래 블럭이 배치되지 않은 경우
        if (!isBottomSlot && !IsBelowBlockPlaced())
        {
            Debug.Log($"아래 블럭이 배치되지 않았습니다. {requiredTag}을(를) 배치할 수 없습니다.");
            return;
        }

        if (!activeState.isGrabbed && !isOccupied && other.CompareTag(requiredTag))
        {
            // 슬롯 점유 상태 설정
            isOccupied = true;

            // 블럭 위치와 회전 고정
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;

            // 블럭 물리 비활성화
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            Debug.Log($"블럭 {requiredTag}이(가) 올바른 위치에 놓였습니다.");
        }
        else
        {
            Debug.Log("잘못된 블럭입니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            // 슬롯 비우기
            isOccupied = false;
            Debug.Log($"블럭 {requiredTag}이(가) 슬롯에서 벗어났습니다.");
        }
    }

    public bool IsCorrectBlockPlaced()
    {
        return isOccupied;
    }

    private bool IsBelowBlockPlaced()
    {
        // Ray 시작 위치
        Vector3 rayStart = transform.position; // 슬롯 중심에서 발사

        // 디버깅용 Ray 시각화
        Debug.DrawRay(rayStart, Vector3.down * 5.0f, Color.red); // 거리: 5.0f

        // Raycast 실행
        RaycastHit hit;
        int blockLayer = LayerMask.NameToLayer("Interactable"); // "Block" 레이어 가져오기
        int layerMask = 1 << blockLayer; // 레이어 마스크 생성

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 5.0f, layerMask)) // Raycast 거리: 5.0f
        {
            Debug.Log($"Ray가 충돌한 오브젝트: {hit.collider.name}, 레이어: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");

            // 슬롯에 충돌했는지 확인
            GridSlot belowSlot = hit.collider.GetComponent<GridSlot>();
            if (belowSlot != null)
            {
                Debug.Log($"아래 슬롯 상태: {belowSlot.IsCorrectBlockPlaced()}");

                if (belowSlot.IsCorrectBlockPlaced())
                {
                    return true; // 아래 슬롯에 블럭이 배치된 상태
                }
                else
                {
                    Debug.Log("아래 슬롯에 블럭이 없습니다.");
                    return false;
                }
            }

            // 블럭에 충돌했는지 확인
            if (hit.collider.gameObject.layer == blockLayer) // 블럭 레이어 확인
            {
                Debug.Log($"아래 블럭이 감지되었습니다: {hit.collider.name}");
                return true; // 아래 블럭이 감지된 상태
            }
            else
            {
                Debug.Log("Ray가 GridSlot 또는 Block 레이어가 아닌 다른 오브젝트에 충돌했습니다.");
            }
        }
        else
        {
            Debug.Log("Ray가 아무것도 감지하지 못했습니다.");
        }

        return false; // 기본적으로 false 반환
    }

    private void DrawDebugRay()
    {
        // 디버깅용 Ray 시각화
        Vector3 rayStart = transform.position; // Ray 시작 위치 (슬롯 중심)
        Debug.DrawRay(rayStart, Vector3.down * 5.0f, Color.red); // 거리: 5.0f
    }
}
