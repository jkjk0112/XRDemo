using UnityEngine;

public class FlipCard : MonoBehaviour
{
    public bool isFlipped = false;
    public float flipSpeed = 5.0f;

    private float targetAngle;
    private bool isAnimating = false;

    void Start()
    {
        targetAngle = 180f; // ���� �� ȸ�� ��ǥ���� ����
        Flip();
    }

    public void Flip()
    {
        if (isAnimating) return; // �ִϸ��̼� ���� �� �߰� �Է� ����

        isAnimating = true;
        isFlipped = !isFlipped;
        targetAngle = isFlipped ? 180f : 0f; // ��ǥ ���� ����
    }

    void Update()
    {
        if (isAnimating)
        {
            // ���� ������ ��ǥ ���� ������ ���̸� ���
            float currentZRotation = transform.eulerAngles.z;
            float newZRotation = Mathf.LerpAngle(currentZRotation, targetAngle, Time.deltaTime * flipSpeed);

            // ȸ�� ����
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);

            // ��ǥ ������ ����������� Ȯ���Ͽ� �ִϸ��̼� ����
            if (Mathf.Abs(newZRotation - targetAngle) < 0.1f)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);
                isAnimating = false;
            }
        }
    }
}
