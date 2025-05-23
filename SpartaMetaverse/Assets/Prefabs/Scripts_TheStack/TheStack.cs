using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Main;

namespace TheStack
{
    public class TheStack : MonoBehaviour
    {
        private const float BoundSize = 3.5f;
        private const float MovingBoundsSize = 3f;
        private const float StackMovingSpeed = 5.0f;
        private const float BlockMovingSpeed = 3.5f;
        private const float ErrorMargin = 0.1f;

        public GameObject originBlock = null;

        private Vector3 prevBlockPosition;
        private Vector3 desiredPosition;
        private Vector3 stackBounds = new Vector2(BoundSize, BoundSize);

        Transform lastBlock = null;
        float blockTransition = 0f;
        float secondaryPosition = 0f;

        int stackCount = -1;
        public int Score { get { return stackCount; } }
        int comboCount = 0;
        public int Combo { get { return comboCount; } }

        private int maxCombo = 0;
        public int MaxCombo { get => maxCombo; }

        public Color prevColor;
        public Color nextColor;

        bool isMovingX = true;

        int bestScore = 0;
        public int BestScore { get => bestScore; }

        int bestCombo = 0;
        public int BestCombo { get => bestCombo; }

        private const string BestScoreKey = "BestScore";
        private const string BestComboKey = "BestCombo";

        public AudioClip MiniGameBGM;

        private bool isGameOver = true;

        void Start()
        {
            SoundManager.instance.ChangeBackGroundMusic(MiniGameBGM);

            if (originBlock == null)
            {
                Debug.LogError("OriginBlock is NULL");
                return;
            }

            bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
            bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

            prevColor = GetRandomColor();
            nextColor = GetRandomColor();

            prevBlockPosition = Vector3.down; // y�� -1�� ����

            Spawn_Block(); // ù ��� ����
            Spawn_Block(); // ù ��� ����
        }

        void Update()
        {
            if (isGameOver) return; // ���ӿ��� �����϶��� �ƹ��͵� ���� ����

            if (Input.GetMouseButtonDown(0))
            {
                if (PlaceBlock())
                {
                    Spawn_Block();
                }
                else
                {
                    // ���ӿ���
                    Debug.Log("Game Over");
                    UpdateScore();
                    isGameOver = true; // ���ӿ��� ���·� ����
                    GameOverEffect();
                    UIManager.Instance.SetScoreUI(); // UI ������Ʈ
                }
            }

            MoveBlock();
            transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime); // �������� �ε巴�� ó��
        }

        bool Spawn_Block()
        {
            if (lastBlock)
                prevBlockPosition = lastBlock.localPosition;

            GameObject newBlock = null;
            Transform newTrans = null;

            newBlock = Instantiate(originBlock);

            if (newBlock == null)
            {
                Debug.LogError("NewBlock Instantiate Failed");
                return false;
            }

            ColorChage(newBlock); // ���� ����

            newTrans = newBlock.transform;
            newTrans.parent = this.transform; // TheStack�� ���� ������Ʈ�� ����
            newTrans.localPosition = prevBlockPosition + Vector3.up; // scale�� 1�̱� ������ ��ĭ ���� ��
            newTrans.localRotation = Quaternion.identity; // ȸ�� ����
            newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y); // scale ����

            stackCount++;

            desiredPosition = Vector3.down * stackCount; // ���� �����ɶ����� ��ü�� ��ĭ�� ������
            blockTransition = 0f; // �̵��� ���� ���ذ� �ʱ�ȭ

            lastBlock = newTrans; // ������ ����� ���� ������� ����

            isMovingX = !isMovingX; // x������ �̵����� y������ �̵����� ����

            UIManager.Instance.UpdateScore(); // UI ������Ʈ
            return true;
        }

        Color GetRandomColor()
        {
            float r = Random.Range(100f, 250f) / 255f; // 100���� ���ϴ� �ʹ� ��ο� �÷�
            float g = Random.Range(100f, 250f) / 255f;
            float b = Random.Range(100f, 250f) / 255f;

            return new Color(r, g, b);
        }

        void ColorChage(GameObject go)
        {
            Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f); // stackCount % 11�� 0~10���� �ݺ��ϰ� 10���� ������ 0~1�� ��ȯ

            Renderer rn = go.GetComponent<Renderer>();

            if (rn == null)
            {
                Debug.Log("Renderer is NULL");
                return;
            }

            rn.material.color = applyColor; // ���� ����
            Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f); // ī�޶� ��� ���� ����

            if (applyColor.Equals(nextColor)) // 10��° ��� ����� ������ �ٸ� �������� ����
            {
                prevColor = nextColor;
                nextColor = GetRandomColor();
            }
        }

        void MoveBlock()
        {
            blockTransition += Time.deltaTime * BlockMovingSpeed;

            float movePosition = Mathf.PingPong(blockTransition, BoundSize) - BoundSize / 2;

            if (isMovingX)
            {
                lastBlock.localPosition = new Vector3(movePosition * MovingBoundsSize, stackCount, secondaryPosition); // X�ุ �����̰� ������ ���� , y���� ���� ����
            }
            else
            {
                lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, -movePosition * MovingBoundsSize); // z�ุ �����̰� ������ ����, y���� ���� ����
            }
        }

        bool PlaceBlock()
        {
            Vector3 lastPosition = lastBlock.localPosition;

            if (isMovingX)
            {
                float deltaX = prevBlockPosition.x - lastPosition.x; // �� ���� �߽���ǥ�� ���� (��߳� ����)
                bool isNegativeNum = (deltaX < 0) ? true : false; // �������� üũ (��߳� ������ ������ üũ�ϱ� ����) Rubble�� ������ ��ġ

                // ���� ����� ũ�⸦ ������ŭ �ڸ��� �߽ɰ��� ����� �ű�� ����
                deltaX = Mathf.Abs(deltaX); // ���밪���� ��ȯ
                if (deltaX > ErrorMargin)
                {
                    stackBounds.x -= deltaX; // ����� ���϶����� ���� ����
                    if (stackBounds.x < 0)
                    {
                        return false; // ����� ���� �� ������
                    }

                    float middle = (prevBlockPosition.x + lastPosition.x) / 2; // �� ����� �߽���ǥ�� �߰���
                    lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y); // ����� ���� ����

                    Vector3 tempPosition = lastBlock.localPosition; // ����� ��ġ�� ����
                    tempPosition.x = middle;
                    lastBlock.localPosition = lastPosition = tempPosition; // ����� ��ġ�� �߰������� ����

                    float rubbleHalfScale = deltaX / 2f;
                    CreateRubble(
                        new Vector3(
                        isNegativeNum
                        ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                        : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale
                        , lastPosition.y
                        , lastPosition.z
                        ),
                        new Vector3(deltaX, 1, stackBounds.y)
                        );

                    comboCount = 0; // �޺� �ʱ�ȭ
                }
                else
                {
                    ComboCheck(); // �޺� üũ
                    lastBlock.localPosition = prevBlockPosition + Vector3.up; // ��ĭ ����
                }
            }
            else
            {
                float deltaZ = prevBlockPosition.z - lastPosition.z;
                bool isNegativeNum = (deltaZ < 0) ? true : false; // �������� üũ (��߳� ������ ������ üũ�ϱ� ����) Rubble�� ������ ��ġ

                deltaZ = Mathf.Abs(deltaZ);
                if (deltaZ > ErrorMargin)
                {
                    stackBounds.y -= deltaZ;
                    if (stackBounds.y < 0)
                    {
                        return false;
                    }

                    float middle = (prevBlockPosition.z + lastPosition.z) / 2;
                    lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                    Vector3 tempPosition = lastBlock.localPosition;
                    tempPosition.z = middle;
                    lastBlock.localPosition = lastPosition = tempPosition;

                    float rubbleHalfScale = deltaZ / 2f;
                    CreateRubble(
                        new Vector3(
                        lastPosition.x
                        , lastPosition.y
                        , isNegativeNum
                        ? lastPosition.z + stackBounds.y / 2 + rubbleHalfScale
                        : lastPosition.z - stackBounds.y / 2 - rubbleHalfScale
                        ),// ������ ��ġ
                        new Vector3(stackBounds.x, 1, deltaZ)
                        );// ������ ������

                    comboCount = 0; // �޺� �ʱ�ȭ
                }
                else
                {
                    ComboCheck(); // �޺� üũ
                    lastBlock.localPosition = prevBlockPosition + Vector3.up;
                }
            }

            secondaryPosition = (isMovingX) ? lastBlock.localPosition.x : lastBlock.localPosition.z; // ���� ����� ��ġ�� ����

            return true; // ����� ���� �� ������
        }

        void CreateRubble(Vector3 pos, Vector3 scale)
        {
            GameObject go = Instantiate(lastBlock.gameObject);
            go.transform.parent = this.transform; // TheStack�� ���� ������Ʈ�� ����

            go.transform.localPosition = pos; // ����� ��ġ�� ����
            go.transform.localScale = scale; // ����� ũ�⸦ ����
            go.transform.localRotation = Quaternion.identity; // ȸ�� ����

            go.AddComponent<Rigidbody>(); // �������� �߰�
            go.name = "Rubble"; // �̸� ����

        }

        void ComboCheck()
        {
            comboCount++;

            if (comboCount > maxCombo)
                maxCombo = comboCount;

            if ((comboCount % 5) == 0)
            {
                Debug.Log("5 Combo Success!");
                stackBounds += new Vector3(0.5f, 0.5f); // ����� ũ�⸦ ����
                stackBounds.x =
                    (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
                stackBounds.y =
                    (stackBounds.y > BoundSize) ? BoundSize : stackBounds.y;
            }
        }

        void UpdateScore()
        {
            if (bestScore < stackCount)
            {
                Debug.Log("New Best Score!");
                bestScore = stackCount;
                bestCombo = maxCombo;

                PlayerPrefs.SetInt(BestScoreKey, bestScore);
                PlayerPrefs.SetInt(BestComboKey, bestCombo);
            }
        }

        void GameOverEffect()
        {
            int childCount = this.transform.childCount; // ���� ������Ʈ(��, ����) ����

            for (int i = 1; i < 20; i++) // �� 20�� ������Ʈ�� ������ٵ� �ް� ���� ���� ����������
            {
                if (childCount < i) break;

                GameObject go = transform.GetChild(childCount - i).gameObject;

                if (go.name == "Rubble") continue; // ������ ����

                Rigidbody rigidbody = go.AddComponent<Rigidbody>(); // �������� �߰�

                rigidbody.AddForce(
                    (Vector3.up * Random.Range(0, 10f) + Vector3.right * (Random.Range(0, 10f) - 5f)) * 100f
                    );
            }
        }

        public void Restart()
        {
            int childCount = transform.childCount; // ���� ������Ʈ(��, ����) ����

            for (int i = 0; i < childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject); // ���� ������Ʈ ����
            }

            isGameOver = false; // ���ӿ��� ���� ����

            lastBlock = null; // ������ ��� �ʱ�ȭ
            desiredPosition = Vector3.zero;
            stackBounds = new Vector2(BoundSize, BoundSize); // ��� ũ�� �ʱ�ȭ

            stackCount = -1; // ��� ���� �ʱ�ȭ
            isMovingX = true; // x������ �̵����� y������ �̵����� ����
            blockTransition = 0f; // �̵��� ���� ���ذ� �ʱ�ȭ
            secondaryPosition = 0f; // ���� ����� ��ġ�� ����

            comboCount = 0; // �޺� �ʱ�ȭ
            maxCombo = 0; // �ִ� �޺� �ʱ�ȭ

            prevBlockPosition = Vector3.down; // y�� -1�� ����

            prevColor = GetRandomColor();
            nextColor = GetRandomColor();

            Spawn_Block(); // ù ��� ����
            Spawn_Block(); // �̵� ��� ����
        }
    }
}
