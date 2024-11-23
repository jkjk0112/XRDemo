using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneGame : MonoBehaviour
{
    [SerializeField] private AudioSource[] successAudios; // 성공 오디오 소스 배열
    public AudioSource failureAudio; // 실패 시 재생할 오디오 소스
    public List<AudioSource> notes; // 각 음계에 해당하는 오디오 소스 리스트
    public List<GameObject> circles; // 색깔별로 시각적 피드백을 주기 위한 원형 오브젝트 리스트

    // 여러 곡의 정답 순서 리스트
    private Dictionary<int, List<string>> correctOrders = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "Yellow", "Orange", "Red", "Orange", "Yellow", "Yellow", "Yellow" } }, // 비행기
        { 1, new List<string> { "Red", "Orange", "Red", "Orange", "Green", "Blue", "Green", "Blue" } }, // 아리랑
        { 2, new List<string> { "Blue", "Blue", "Yellow", "Green", "Blue", "Purple", "Purple", "Blue" } } // 나의 살던 고향은
    };



    private List<string> playerOrder = new List<string>(); // 플레이어의 입력 순서 리스트
    private List<string> correctOrder; // 현재 게임에서 사용할 정답 순서 리스트
    private int selectedSongIndex; // 선택된 곡 인덱스
    public bool isSequenceRunning = false; // 시퀀스 실행 중 여부

    void Start()
    {
        // 게임 시작 시 모든 원형 오브젝트를 비활성화
        foreach (GameObject circle in circles)
        {
            circle.SetActive(false);
        }


    }

    // 랜덤으로 곡을 선택하고, 해당 곡의 정답 순서를 설정
    public void SelectRandomSong()
    {
        selectedSongIndex = Random.Range(0, correctOrders.Count); // 랜덤으로 곡 인덱스 선택
        correctOrder = correctOrders[selectedSongIndex]; // 선택된 곡에 맞는 정답 순서 설정
        Debug.Log($"랜덤으로 선택된 곡: {selectedSongIndex}");
    }

    // 정답 성공 시 호출할 함수
    public void PlaySuccessAudio()
    {
        switch (selectedSongIndex)
        {
            case 0: // 비행기
                successAudios[0].Play();
                Debug.Log("재생 중인 오디오: 비행기 (Element 0)");
                break;

            case 1: // 아리랑
                successAudios[1].Play();
                Debug.Log("재생 중인 오디오: 아리랑 (Element 1)");
                break;

            case 2: // 나의 살던 고향은
                successAudios[2].Play();
                Debug.Log("재생 중인 오디오: 나의 살던 고향은 (Element 2)");
                break;

            default:
                Debug.LogError("오디오를 재생할 곡이 선택되지 않았습니다. 선택된 곡 인덱스가 올바르지 않습니다.");
                break;
        }
    }

    // 색깔에 맞는 원형 오브젝트를 순차적으로 표시하고, 그에 맞는 음계를 재생하는 시퀀스를 실행
    public IEnumerator StartCircleSequence()
    {
        isSequenceRunning = true; // 시퀀스 실행 중으로 설정

        foreach (string color in correctOrder)
        {
            GameObject circle = circles.Find(c => c.name == color); // 색깔에 맞는 원형 오브젝트 찾기
            if (circle != null)
            {
                circle.SetActive(true); // 원형 오브젝트 활성화
                PlayNoteByColor(color); // 해당 색깔의 음계 재생
                yield return new WaitForSeconds(0.1f); // 1초 대기
                circle.SetActive(false); // 원형 오브젝트 비활성화
                yield return new WaitForSeconds(0.5f); // 0.5초 대기
            }
        }
        isSequenceRunning = false; // 시퀀스 종료 상태로 설정
    }

    // 플레이어가 선택한 색깔을 처리하고, 순서가 맞는지 확인하는 함수
    public void PlayXylophone(string color)
    {
        playerOrder.Add(color); // 플레이어가 선택한 색깔을 입력 순서에 추가
        PlayNoteByColor(color); // 선택한 색깔의 음계 재생

        // 입력된 순서가 정답과 일치하는지 확인
        if (!IsCorrectSequence())
        {
            failureAudio.Play(); // 틀린 순서일 경우 실패 음향 재생
            playerOrder.Clear(); // 입력 순서 초기화
            Debug.Log("틀렸습니다! 처음부터 다시 시작하세요."); // 실패 메시지 출력
        }
        else if (playerOrder.Count == correctOrder.Count)
        {
            // 정답 순서에 맞는 경우 성공 음원을 재생
            Invoke("PlaySuccessAudio", 1.5f);
            Debug.Log("미션 성공!"); // 성공 메시지 출력
            playerOrder.Clear(); // 입력 순서 초기화

        }
    }

    // 플레이어의 입력 순서가 정답과 일치하는지 확인하는 함수
    private bool IsCorrectSequence()
    {
        for (int i = 0; i < playerOrder.Count; i++)
        {
            if (playerOrder[i] != correctOrder[i]) // 입력이 정답과 다르면
            {
                return false; // 일치하지 않음
            }
        }
        return true; // 모두 일치함
    }

    // 색깔에 맞는 음계를 재생하는 함수
    private void PlayNoteByColor(string color)
    {
        switch (color)
        {
            case "Red":
                notes[0].Play(); // '도' 음계 재생
                break;
            case "Orange":
                notes[1].Play(); // '레' 음계 재생
                break;
            case "Yellow":
                notes[2].Play(); // '미' 음계 재생
                break;
            case "Green":
                notes[3].Play(); // '파' 음계 재생
                break;
            case "Blue":
                notes[4].Play(); // '솔' 음계 재생
                break;
            case "Purple":
                notes[5].Play(); // '라' 음계 재생
                break;
        }
    }
}
