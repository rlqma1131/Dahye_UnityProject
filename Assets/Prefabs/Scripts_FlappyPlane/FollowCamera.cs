using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyPlane
{
    public class FollowCamera : MonoBehaviour
    {
        public Transform target; // Follow할 타겟
        float offsetX;

        // Start is called before the first frame update
        void Start()
        {
            if (target == null)
                return;

            offsetX = transform.position.x - target.position.x; // 카메라와 타겟의 x축 거리 (우린 x축으로 움직이니까)

        }

        // Update is called once per frame
        void Update()
        {
            if (target == null)
                return;
            Vector3 pos = transform.position;
            pos.x = target.position.x + offsetX; // 카메라의 x축 위치를 타겟의 x축 위치에 offsetX를 더한 값으로 설정
            transform.position = pos; // 카메라의 위치를 설정
        }
    }
}
