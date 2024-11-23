using UnityEngine;

public class FlipCard : MonoBehaviour
{
    public bool isFlipped = false;
    public float flipSpeed = 5.0f;

    private float targetAngle;
    private bool isAnimating = false;

    void Start()
    {
        targetAngle = 180f; // 시작 시 회전 목표각도 설정
        Flip();
    }

    public void Flip()
    {
        if (isAnimating) return; // 애니메이션 중일 때 추가 입력 방지

        isAnimating = true;
        isFlipped = !isFlipped;
        targetAngle = isFlipped ? 180f : 0f; // 목표 각도 설정
    }

    void Update()
    {
        if (isAnimating)
        {
            // 현재 각도와 목표 각도 사이의 차이를 계산
            float currentZRotation = transform.eulerAngles.z;
            float newZRotation = Mathf.LerpAngle(currentZRotation, targetAngle, Time.deltaTime * flipSpeed);

            // 회전 적용
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);

            // 목표 각도에 가까워졌는지 확인하여 애니메이션 종료
            if (Mathf.Abs(newZRotation - targetAngle) < 0.1f)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);
                isAnimating = false;
            }
        }
    }
}
