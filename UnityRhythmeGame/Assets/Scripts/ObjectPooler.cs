using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//특정 오브젝트가 화면에 나올 때 그 오브젝트를 생성, 파괴하는 것이 아니라 미리 많이 만들어 놓고 활성, 비활성화 하는 방법 
//-> 오브젝트의 불필요한 생성 삭제를 줄여 성능을 향상 (Instantiate와 Destroy의 사용을 줄이기 위함)
public class ObjectPooler : MonoBehaviour
{
    // Start is called before the first frame update
    //노트 10개씩 묶어서 list에 담기 -> 이 list들을 담고 있는 이중 list 사용
    public List<GameObject> Notes;
    private List<List<GameObject>> poolsOfNotes;
    public int noteCount = 10;
    private bool more = true;

    void Start()
    {
        poolsOfNotes = new List<List<GameObject>>();
        for(int i = 0; i<Notes.Count; i++){ //4번 반복
            poolsOfNotes.Add(new List<GameObject>());
            for(int n = 0; n < noteCount; n++){ //10번 반복
                GameObject obj = Instantiate(Notes[i]);
                obj.SetActive(false); //노트 비활성화
                poolsOfNotes[i].Add(obj);
            }
        }
    }

    public void Judge(int noteType)
    {
        foreach(GameObject obj in poolsOfNotes[noteType - 1])
        {
            if(!obj.activeInHierarchy)
            {
                obj.GetComponent<NoteBehavior>().Judge();
            }
        }
    }

    public GameObject getObject(int noteType){
        foreach(GameObject obj in poolsOfNotes[noteType-1]){
            if(!obj.activeInHierarchy){ //object가 전부 활성화 되지 않았을 때
                return obj;
            }
        }
        if(more){ //노트가 필요한 만큼 사용될 수 있도록 설정
            GameObject obj = Instantiate(Notes[noteType - 1]);
            poolsOfNotes[noteType - 1].Add(obj);
            return obj;
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
