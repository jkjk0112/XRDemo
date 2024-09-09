using UnityEngine;

public class ReturnObject : MonoBehaviour
{
    public float returnDelay = 2.0f; // ���� ��ġ�� ���ư��� �ð�
    public Collider triggerArea; // ���ư��� �ϴ� ����
    public ActiveState activeState; // �׷� ���� Ȯ�� ��

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isReturned = false;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (activeState == null)
        {
            activeState = GetComponent<ActiveState>();
        }
    }

    private void Update()
    {
        if (!activeState.isGrabbed) // ��ü�� �׷����� �ʾ��� ���� �̵�
        {
            if (!isReturned && IsObjectOutOfBounds())
            {
                Invoke("ReturnToOriginalPosition", returnDelay);
                isReturned = true;
            }
        }
        else
        {
            isReturned = false; // ��ü�� �׷��Ǿ��� ���� ����
        }
    }

    private bool IsObjectOutOfBounds()
    {
        return !triggerArea.bounds.Contains(transform.position);
    }

    private void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isReturned = false;
    }
}
