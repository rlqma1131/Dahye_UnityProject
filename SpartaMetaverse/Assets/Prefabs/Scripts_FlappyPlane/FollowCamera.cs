using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyPlane
{
    public class FollowCamera : MonoBehaviour
    {
        public Transform target; // Follow�� Ÿ��
        float offsetX;

        // Start is called before the first frame update
        void Start()
        {
            if (target == null)
                return;

            offsetX = transform.position.x - target.position.x; // ī�޶�� Ÿ���� x�� �Ÿ� (�츰 x������ �����̴ϱ�)

        }

        // Update is called once per frame
        void Update()
        {
            if (target == null)
                return;
            Vector3 pos = transform.position;
            pos.x = target.position.x + offsetX; // ī�޶��� x�� ��ġ�� Ÿ���� x�� ��ġ�� offsetX�� ���� ������ ����
            transform.position = pos; // ī�޶��� ��ġ�� ����
        }
    }
}
