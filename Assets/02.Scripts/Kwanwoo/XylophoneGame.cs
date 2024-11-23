using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneGame : MonoBehaviour
{
    [SerializeField] private AudioSource[] successAudios; // ���� ����� �ҽ� �迭
    public AudioSource failureAudio; // ���� �� ����� ����� �ҽ�
    public List<AudioSource> notes; // �� ���迡 �ش��ϴ� ����� �ҽ� ����Ʈ
    public List<GameObject> circles; // ���򺰷� �ð��� �ǵ���� �ֱ� ���� ���� ������Ʈ ����Ʈ

    // ���� ���� ���� ���� ����Ʈ
    private Dictionary<int, List<string>> correctOrders = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "Yellow", "Orange", "Red", "Orange", "Yellow", "Yellow", "Yellow" } }, // �����
        { 1, new List<string> { "Red", "Orange", "Red", "Orange", "Green", "Blue", "Green", "Blue" } }, // �Ƹ���
        { 2, new List<string> { "Blue", "Blue", "Yellow", "Green", "Blue", "Purple", "Purple", "Blue" } } // ���� ��� ������
    };



    private List<string> playerOrder = new List<string>(); // �÷��̾��� �Է� ���� ����Ʈ
    private List<string> correctOrder; // ���� ���ӿ��� ����� ���� ���� ����Ʈ
    private int selectedSongIndex; // ���õ� �� �ε���
    public bool isSequenceRunning = false; // ������ ���� �� ����

    void Start()
    {
        // ���� ���� �� ��� ���� ������Ʈ�� ��Ȱ��ȭ
        foreach (GameObject circle in circles)
        {
            circle.SetActive(false);
        }


    }

    // �������� ���� �����ϰ�, �ش� ���� ���� ������ ����
    public void SelectRandomSong()
    {
        selectedSongIndex = Random.Range(0, correctOrders.Count); // �������� �� �ε��� ����
        correctOrder = correctOrders[selectedSongIndex]; // ���õ� � �´� ���� ���� ����
        Debug.Log($"�������� ���õ� ��: {selectedSongIndex}");
    }

    // ���� ���� �� ȣ���� �Լ�
    public void PlaySuccessAudio()
    {
        switch (selectedSongIndex)
        {
            case 0: // �����
                successAudios[0].Play();
                Debug.Log("��� ���� �����: ����� (Element 0)");
                break;

            case 1: // �Ƹ���
                successAudios[1].Play();
                Debug.Log("��� ���� �����: �Ƹ��� (Element 1)");
                break;

            case 2: // ���� ��� ������
                successAudios[2].Play();
                Debug.Log("��� ���� �����: ���� ��� ������ (Element 2)");
                break;

            default:
                Debug.LogError("������� ����� ���� ���õ��� �ʾҽ��ϴ�. ���õ� �� �ε����� �ùٸ��� �ʽ��ϴ�.");
                break;
        }
    }

    // ���� �´� ���� ������Ʈ�� ���������� ǥ���ϰ�, �׿� �´� ���踦 ����ϴ� �������� ����
    public IEnumerator StartCircleSequence()
    {
        isSequenceRunning = true; // ������ ���� ������ ����

        foreach (string color in correctOrder)
        {
            GameObject circle = circles.Find(c => c.name == color); // ���� �´� ���� ������Ʈ ã��
            if (circle != null)
            {
                circle.SetActive(true); // ���� ������Ʈ Ȱ��ȭ
                PlayNoteByColor(color); // �ش� ������ ���� ���
                yield return new WaitForSeconds(0.1f); // 1�� ���
                circle.SetActive(false); // ���� ������Ʈ ��Ȱ��ȭ
                yield return new WaitForSeconds(0.5f); // 0.5�� ���
            }
        }
        isSequenceRunning = false; // ������ ���� ���·� ����
    }

    // �÷��̾ ������ ������ ó���ϰ�, ������ �´��� Ȯ���ϴ� �Լ�
    public void PlayXylophone(string color)
    {
        playerOrder.Add(color); // �÷��̾ ������ ������ �Է� ������ �߰�
        PlayNoteByColor(color); // ������ ������ ���� ���

        // �Էµ� ������ ����� ��ġ�ϴ��� Ȯ��
        if (!IsCorrectSequence())
        {
            failureAudio.Play(); // Ʋ�� ������ ��� ���� ���� ���
            playerOrder.Clear(); // �Է� ���� �ʱ�ȭ
            Debug.Log("Ʋ�Ƚ��ϴ�! ó������ �ٽ� �����ϼ���."); // ���� �޽��� ���
        }
        else if (playerOrder.Count == correctOrder.Count)
        {
            // ���� ������ �´� ��� ���� ������ ���
            Invoke("PlaySuccessAudio", 1.5f);
            Debug.Log("�̼� ����!"); // ���� �޽��� ���
            playerOrder.Clear(); // �Է� ���� �ʱ�ȭ

        }
    }

    // �÷��̾��� �Է� ������ ����� ��ġ�ϴ��� Ȯ���ϴ� �Լ�
    private bool IsCorrectSequence()
    {
        for (int i = 0; i < playerOrder.Count; i++)
        {
            if (playerOrder[i] != correctOrder[i]) // �Է��� ����� �ٸ���
            {
                return false; // ��ġ���� ����
            }
        }
        return true; // ��� ��ġ��
    }

    // ���� �´� ���踦 ����ϴ� �Լ�
    private void PlayNoteByColor(string color)
    {
        switch (color)
        {
            case "Red":
                notes[0].Play(); // '��' ���� ���
                break;
            case "Orange":
                notes[1].Play(); // '��' ���� ���
                break;
            case "Yellow":
                notes[2].Play(); // '��' ���� ���
                break;
            case "Green":
                notes[3].Play(); // '��' ���� ���
                break;
            case "Blue":
                notes[4].Play(); // '��' ���� ���
                break;
            case "Purple":
                notes[5].Play(); // '��' ���� ���
                break;
        }
    }
}
