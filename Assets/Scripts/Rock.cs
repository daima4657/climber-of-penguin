using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rock : MonoBehaviour
{
    [SerializeField] private float aliveTime = 3.0f; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Lifespan");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            //GameSceneÇ∂Ç·Ç»Ç©Ç¡ÇΩÇÁè¡Ç∑
            Destroy(gameObject);
        }
    }

    private IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(aliveTime);
        Destroy(gameObject);
    }
}
