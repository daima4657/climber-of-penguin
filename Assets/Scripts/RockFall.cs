using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    //��������Q�[���I�u�W�F�N�g
    public GameObject target;

    
    private GManager gManager = GManager.instance;
    private float rockPositionX;
    private float rockStayTime = 0;
    public GameObject firstRock;
    public GameObject secondRock;
    public bool active;

    private GManager myself;

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        myself = gameObject.GetComponent<GManager>();
        
        StartCoroutine("GenerateRocks");
    }

    // Update is called once per frame
    void Update()
    {
    }


    //���Ί֐�
    public void GenerateRock(float posX)
    {
        
        GameObject rock1 = Instantiate(firstRock, new Vector3(posX, myself.progress + 10.0f, 0), Quaternion.identity);
        rockStayTime += Time.deltaTime;
    }



    private IEnumerator GenerateRocks()
    {
        //3�b���ƂɎ��s
        while (active)
        {
            // 2�b�ԑ҂�
            yield return new WaitForSeconds(2);

            //x�ʒu���m��
            rockPositionX = Random.Range(-7.0f, 7.0f);
            //�X�𗎂Ƃ�
            GenerateRock(rockPositionX);
            yield return GenerateBigRock(rockPositionX);
        }

    }

    IEnumerator GenerateBigRock(float posX)
    {
        yield return new WaitForSeconds(1.5f);
        GameObject rock2 = Instantiate(secondRock, new Vector3(posX, myself.progress + 10.0f, 0), Quaternion.identity);

    }

    public void ChangeActivation(bool flag)
    {
        Debug.Log("���Β�~");
        active = flag;
    }

}
