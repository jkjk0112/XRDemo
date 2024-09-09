using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pointer : MonoBehaviour
{
    public Transform pointer; // ���� ���� ����
    public Color noHitColor = Color.red; // ���̰� ���� �ʾ��� �� ����
    public Color hitColor = Color.green; // ���̰� ����� �� ����
    private RaycastHit hit;
    public LineRenderer lineRenderer;
    public InputManager xrinput;
    public LayerMask interactable;
    public float rayDistance = 5f;
    private Outline currentOutline;

    private void Start()
    {
        lineRenderer.startColor = noHitColor;
        lineRenderer.endColor = noHitColor;
        lineRenderer.enabled = false; 
    }
    private void Update()
    {
        if (Physics.Raycast(pointer.position, pointer.forward, out hit, rayDistance, interactable))
        {
            Debug.Log($"Hit: {hit.collider.name}");

            lineRenderer.enabled = true;
            lineRenderer.startColor = hitColor;
            lineRenderer.endColor = hitColor;

            // ������ �������� ���� ����
            lineRenderer.SetPosition(0, pointer.position); // ������
            lineRenderer.SetPosition(1, hit.point);        // ���� 

            // �±׷� ��ȣ�ۿ� ���� ���� �˻�
            if (xrinput.IsTriggerPressed() && (hit.collider.CompareTag("RayInteractable") || hit.collider.CompareTag("InteractionObject")))
            {
                TriggerSelect(hit.collider.gameObject);
            }

            // �浹�� ������Ʈ�� �ƿ����� ó��
            ShowOutline(hit.collider.gameObject);
        }
        else
        {
            lineRenderer.enabled = false;
            if (currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
            }
        }
    }

    // �ƿ�����
    private void ShowOutline(GameObject obj)
    {
        Outline outline = obj.GetComponent<Outline>();

        if (outline != null)
        {
            if (currentOutline != outline)
            {
                if (currentOutline != null)
                {
                    currentOutline.enabled = false; // ���� ������Ʈ�� �ƿ����� ��Ȱ��ȭ
                }
                currentOutline = outline;
                currentOutline.enabled = true; // ���� ������Ʈ�� �ƿ����� Ȱ��ȭ
            }
        }
        else
        {
            if (currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
            }
        }
    }

    private void TriggerSelect(GameObject obj)
    {
        Debug.Log($"��ȣ�ۿ�: {obj.name}");
    }
}
