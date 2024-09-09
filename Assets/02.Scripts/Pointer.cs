using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pointer : MonoBehaviour
{
    public Transform pointer; // 레이 시작 지점
    public Color noHitColor = Color.red; // 레이가 닿지 않았을 때 색상
    public Color hitColor = Color.green; // 레이가 닿았을 때 색상
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

            // 라인의 시작점과 끝점 설정
            lineRenderer.SetPosition(0, pointer.position); // 시작점
            lineRenderer.SetPosition(1, hit.point);        // 끝점 

            // 태그로 상호작용 가능 여부 검사
            if (xrinput.IsTriggerPressed() && (hit.collider.CompareTag("RayInteractable") || hit.collider.CompareTag("InteractionObject")))
            {
                TriggerSelect(hit.collider.gameObject);
            }

            // 충돌한 오브젝트의 아웃라인 처리
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

    // 아웃라인
    private void ShowOutline(GameObject obj)
    {
        Outline outline = obj.GetComponent<Outline>();

        if (outline != null)
        {
            if (currentOutline != outline)
            {
                if (currentOutline != null)
                {
                    currentOutline.enabled = false; // 이전 오브젝트의 아웃라인 비활성화
                }
                currentOutline = outline;
                currentOutline.enabled = true; // 현재 오브젝트의 아웃라인 활성화
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
        Debug.Log($"상호작용: {obj.name}");
    }
}
