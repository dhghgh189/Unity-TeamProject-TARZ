using UnityEngine;

public class UNQuest_Interaction : InteractionOBJ_Base, Interaction_Ibase_Activate
{
    [Header("돌발 퀘스트 NPC")]
    [SerializeField] float Reward;

    void Start() => Init();

    void Init()
    {

    }

    public void Activate()
    {
        // TODO : UI 대화창 띄움과 동시에, 돌발 퀘스트 승낙 여부 표시
        // 승낙 시 목표치 설정해줌
        // 거절 시 그 자리에 가만히 있음

        /* 경우의 수
         
        1. 아무 상호작용도 하지 않고, 가장 처음 말을 걸엇을 때 = 퀘스트 승낙 여부를 묻는다
        1-1. 승낙 시 퀘스트 진행
        1-2. 이후 대사 없음. 다시 말 걸면 1의 상황으로 되돌아감

        2. 승낙 후 퀘스트 진행 사항을 표시한다.
        2-1. 퀘스트 진행이 완료되지 않았을 경우, 같은 말을 반복하도록 한다. (완료 여부는 bool형으로 해결)
        2-2. 퀘스트가 완료되었을 경우, 지정된 랜덤한 보상을 플레이어에게 전달한다.

        3. 퀘스트 완료 후, NPC는 비활성화 혹은 삭제된다.
         */
    }

    //===============================================================================
}
