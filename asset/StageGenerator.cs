using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGenerator : MonoBehaviour
{
    const int StageTipSize = 50;

    int currentTipIndex;

    public Transform character;
    public GameObject[] stageTips;
    public int startTipIndex;
    public int preInstantiate;
    public List<GameObject> generatedStageList = new List<GameObject>();

	// Use this for initialization
	void Start () {
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiate);
	}
	
	// Update is called once per frame
	void Update () {
        //캐릭터 위치에로부터 현재의 스테이지 팁 인덱스를 계산
        int charaPositionIndex = (int)(character.position.z / StageTipSize);

        //다음 스테이지 팁에 들어갔다면 스테이지의 변경처리를 한다
        if(charaPositionIndex + preInstantiate > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
	}

    //지정된 Index까지 스테이지팁을 생성하여 관리화에 둔다.
    void UpdateStage(int toTipIndex)
    {
        if (toTipIndex <= currentTipIndex)
            return;

        //지정 스테이지팁까지의 작성
        for(int i = currentTipIndex +1; i <= toTipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            //생성한 스테이지팁을 관리리스트에 추가한다
            generatedStageList.Add(stageObject);

            //스테이지 생성상한에 맞춰 오래된 스테이지 삭제
            while (generatedStageList.Count > preInstantiate + 4)
                DestroyOldestStage();

            currentTipIndex = toTipIndex;
        }
    }

    //지정 인덱스 위치에 Stage 오브젝트를 랜덤으로 생성한다.
    GameObject GenerateStage(int tipIndex)
    {
        int nextStageTip = Random.Range(0, stageTips.Length);

        GameObject stageObject = (GameObject)Instantiate(stageTips[nextStageTip], new Vector3(0, 0, tipIndex * StageTipSize),Quaternion.identity);
        return stageObject;
    }
    
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
