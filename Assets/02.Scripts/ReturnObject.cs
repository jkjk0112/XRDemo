using UnityEngine;

public class ReturnObject : MonoBehaviour
{
    public float returnDelay = 2.0f; // 원래 위치로 돌아가는 시간
    public Collider triggerArea; // 돌아가야 하는 영역
    public ActiveState activeState; // 그랩 상태 확인 용

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
        if (!activeState.isGrabbed) // 물체가 그랩되지 않았을 때만 이동
        {
            if (!isReturned && IsObjectOutOfBounds())
            {
                Invoke("ReturnToOriginalPosition", returnDelay);
                isReturned = true;
            }
        }
        else
        {
            isReturned = false; // 물체가 그랩되었을 때는 리셋
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
