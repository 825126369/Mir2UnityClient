using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameratrack : MonoBehaviour
{
    public Transform playerTransform;//跟踪的对象
    private Vector3 offset;
    private Vector3 ShakeOffset;
    private bool bInShake = false;

    void Start()
    {
        offset = transform.position - playerTransform.position;//计算相对距离
    }

    void LateUpdate()
    {
        if (bInShake)
        {

        }
        else
        {
            transform.position = playerTransform.position + offset; //保持相对距离
        }
    }

    public void Shake()
    {
        AndroidVibrateHelper.Call_VIBRATOR(50);
        bInShake = true;
        ShakeOffset = playerTransform.position + offset + new Vector3(Random.value, Random.value, Random.value) * 0.05f;
        LeanTween.move(gameObject, ShakeOffset, 0.15f).setEaseShake().setOnComplete(() =>
        {
            bInShake = false;
        });
    }
}
