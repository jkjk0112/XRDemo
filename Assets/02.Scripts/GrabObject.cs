using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public InputManager xrinput;
    public Transform player;
    private Transform handTransform;
    private Rigidbody handRigidbody;
    private Rigidbody attachedObj;
    private List<Rigidbody> contactRigidbodies;
    private ActiveState attachedObjActiveState; // �׷� �� ������Ʈ�� Ȱ��ȭ ���� ������ ���� ����

    void Start()
    {
        handTransform = GetComponent<Transform>();
        handRigidbody = GetComponent<Rigidbody>();
        contactRigidbodies = new List<Rigidbody>();
    }

    void Update()
    {
        if (xrinput.IsGripPressed())
        {
            ObjectPickUp();
        }
        else
        {
            ObjectDrop();
        }
    }

    public void ObjectPickUp()
    {
        if (attachedObj != null)
        {
            return;  // �̹� ������Ʈ�� ��� �ִٸ�, �� �̻� �׷� x
        }

        attachedObj = GetNearestRigidbody();

        if (attachedObj == null)
        {
            return;  // ��ó�� �׷� ���� ������Ʈ�� ������
        }

        attachedObj.useGravity = false;
        attachedObj.isKinematic = true;
        attachedObj.transform.SetParent(handTransform);
        attachedObj.transform.position = handTransform.position;
        attachedObj.transform.rotation = handTransform.rotation;

        // Attached object�� Ȱ��ȭ ���� ����
        attachedObjActiveState = attachedObj.GetComponent<ActiveState>();
        if (attachedObjActiveState != null)
        {
            attachedObjActiveState.SetGrabbed(true);
        }
    }

    public void ObjectDrop()
    {
        if (attachedObj == null)
        {
            return;
        }

        attachedObj.transform.SetParent(null);
        attachedObj.useGravity = true;
        attachedObj.isKinematic = false;

        attachedObj.velocity += player.rotation * xrinput.VelocityInput();
        attachedObj.angularVelocity = player.rotation * xrinput.AngularVelocityInput();

        if (attachedObjActiveState != null)
        {
            attachedObjActiveState.SetGrabbed(false);
        }

        attachedObj = null;
        attachedObjActiveState = null; 
    }

    private Rigidbody GetNearestRigidbody()
    {
        Rigidbody nearestRigidbody = null;
        float minDistance = float.MaxValue;

        foreach (Rigidbody rigidbody in contactRigidbodies)
        {
            float distance = Vector3.Distance(rigidbody.transform.position, handTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestRigidbody = rigidbody;
            }
        }

        return nearestRigidbody;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractionObject"))
        {
            contactRigidbodies.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractionObject"))
        {
            contactRigidbodies.Remove(other.gameObject.GetComponent<Rigidbody>());
        }
    }
}