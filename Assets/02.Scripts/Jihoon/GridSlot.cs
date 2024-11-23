using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public string requiredTag; // �ش� ���Կ� �ʿ��� ���� �±�
    private bool isOccupied = false; // ������ �����Ǿ����� ����
    public ActiveState activeState; // ���� �����ִ��� Ȯ��
    public bool isBottomSlot = false; // �� �Ʒ� �������� ����

    private void Update()
    {
        // Ray �ð�ȭ�� Update���� ���������� ����
        DrawDebugRay();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �� �Ʒ� ������ �ƴϸ鼭 �Ʒ� ���� ��ġ���� ���� ���
        if (!isBottomSlot && !IsBelowBlockPlaced())
        {
            Debug.Log($"�Ʒ� ���� ��ġ���� �ʾҽ��ϴ�. {requiredTag}��(��) ��ġ�� �� �����ϴ�.");
            return;
        }

        if (!activeState.isGrabbed && !isOccupied && other.CompareTag(requiredTag))
        {
            // ���� ���� ���� ����
            isOccupied = true;

            // �� ��ġ�� ȸ�� ����
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;

            // �� ���� ��Ȱ��ȭ
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            Debug.Log($"�� {requiredTag}��(��) �ùٸ� ��ġ�� �������ϴ�.");
        }
        else
        {
            Debug.Log("�߸��� ���Դϴ�.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            // ���� ����
            isOccupied = false;
            Debug.Log($"�� {requiredTag}��(��) ���Կ��� ������ϴ�.");
        }
    }

    public bool IsCorrectBlockPlaced()
    {
        return isOccupied;
    }

    private bool IsBelowBlockPlaced()
    {
        // Ray ���� ��ġ
        Vector3 rayStart = transform.position; // ���� �߽ɿ��� �߻�

        // ������ Ray �ð�ȭ
        Debug.DrawRay(rayStart, Vector3.down * 5.0f, Color.red); // �Ÿ�: 5.0f

        // Raycast ����
        RaycastHit hit;
        int blockLayer = LayerMask.NameToLayer("Interactable"); // "Block" ���̾� ��������
        int layerMask = 1 << blockLayer; // ���̾� ����ũ ����

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 5.0f, layerMask)) // Raycast �Ÿ�: 5.0f
        {
            Debug.Log($"Ray�� �浹�� ������Ʈ: {hit.collider.name}, ���̾�: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");

            // ���Կ� �浹�ߴ��� Ȯ��
            GridSlot belowSlot = hit.collider.GetComponent<GridSlot>();
            if (belowSlot != null)
            {
                Debug.Log($"�Ʒ� ���� ����: {belowSlot.IsCorrectBlockPlaced()}");

                if (belowSlot.IsCorrectBlockPlaced())
                {
                    return true; // �Ʒ� ���Կ� ���� ��ġ�� ����
                }
                else
                {
                    Debug.Log("�Ʒ� ���Կ� ���� �����ϴ�.");
                    return false;
                }
            }

            // ���� �浹�ߴ��� Ȯ��
            if (hit.collider.gameObject.layer == blockLayer) // �� ���̾� Ȯ��
            {
                Debug.Log($"�Ʒ� ���� �����Ǿ����ϴ�: {hit.collider.name}");
                return true; // �Ʒ� ���� ������ ����
            }
            else
            {
                Debug.Log("Ray�� GridSlot �Ǵ� Block ���̾ �ƴ� �ٸ� ������Ʈ�� �浹�߽��ϴ�.");
            }
        }
        else
        {
            Debug.Log("Ray�� �ƹ��͵� �������� ���߽��ϴ�.");
        }

        return false; // �⺻������ false ��ȯ
    }

    private void DrawDebugRay()
    {
        // ������ Ray �ð�ȭ
        Vector3 rayStart = transform.position; // Ray ���� ��ġ (���� �߽�)
        Debug.DrawRay(rayStart, Vector3.down * 5.0f, Color.red); // �Ÿ�: 5.0f
    }
}
