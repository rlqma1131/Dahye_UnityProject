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

            prevBlockPosition = Vector3.down; // y가 -1로 시작

            Spawn_Block(); // 첫 블록 생성
            Spawn_Block(); // 첫 블록 생성
        }

        void Update()
        {
            if (isGameOver) return; // 게임오버 상태일때는 아무것도 하지 않음

            if (Input.GetMouseButtonDown(0))
            {
                if (PlaceBlock())
                {
                    Spawn_Block();
                }
                else
                {
                    // 게임오버
                    Debug.Log("Game Over");
                    UpdateScore();
                    isGameOver = true; // 게임오버 상태로 변경
                    GameOverEffect();
                    UIManager.Instance.SetScoreUI(); // UI 업데이트
                }
            }

            MoveBlock();
            transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime); // 움직임을 부드럽게 처리
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

            ColorChage(newBlock); // 색상 변경

            newTrans = newBlock.transform;
            newTrans.parent = this.transform; // TheStack의 하위 오브젝트로 설정
            newTrans.localPosition = prevBlockPosition + Vector3.up; // scale이 1이기 때문에 한칸 위가 됨
            newTrans.localRotation = Quaternion.identity; // 회전 고정
            newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y); // scale 고정

            stackCount++;

            desiredPosition = Vector3.down * stackCount; // 새로 생성될때마다 전체가 한칸씩 내려감
            blockTransition = 0f; // 이동에 대한 기준값 초기화

            lastBlock = newTrans; // 마지막 블록을 현재 블록으로 설정

            isMovingX = !isMovingX; // x축으로 이동할지 y축으로 이동할지 결정

            UIManager.Instance.UpdateScore(); // UI 업데이트
            return true;
        }

        Color GetRandomColor()
        {
            float r = Random.Range(100f, 250f) / 255f; // 100보다 이하는 너무 어두운 컬러
            float g = Random.Range(100f, 250f) / 255f;
            float b = Random.Range(100f, 250f) / 255f;

            return new Color(r, g, b);
        }

        void ColorChage(GameObject go)
        {
            Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f); // stackCount % 11로 0~10까지 반복하고 10으로 나누어 0~1로 변환

            Renderer rn = go.GetComponent<Renderer>();

            if (rn == null)
            {
                Debug.Log("Renderer is NULL");
                return;
            }

            rn.material.color = applyColor; // 색상 적용
            Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f); // 카메라 배경 색상 적용

            if (applyColor.Equals(nextColor)) // 10번째 블록 색상과 같으면 다른 색상으로 변경
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
                lastBlock.localPosition = new Vector3(movePosition * MovingBoundsSize, stackCount, secondaryPosition); // X축만 움직이고 나머지 고정 , y축은 쌓인 높이
            }
            else
            {
                lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, -movePosition * MovingBoundsSize); // z축만 움직이고 나머지 고정, y축은 쌓인 높이
            }
        }

        bool PlaceBlock()
        {
            Vector3 lastPosition = lastBlock.localPosition;

            if (isMovingX)
            {
                float deltaX = prevBlockPosition.x - lastPosition.x; // 두 블럭의 중심좌표의 차이 (어긋난 길이)
                bool isNegativeNum = (deltaX < 0) ? true : false; // 음수인지 체크 (어긋난 길이의 방향을 체크하기 위함) Rubble이 떨어질 위치

                // 쌓인 블록의 크기를 오차만큼 자르고 중심값을 가운데로 옮기는 로직
                deltaX = Mathf.Abs(deltaX); // 절대값으로 변환
                if (deltaX > ErrorMargin)
                {
                    stackBounds.x -= deltaX; // 블록이 쌓일때마다 길이 감소
                    if (stackBounds.x < 0)
                    {
                        return false; // 블록이 쌓일 수 없을때
                    }

                    float middle = (prevBlockPosition.x + lastPosition.x) / 2; // 두 블록의 중심좌표의 중간값
                    lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y); // 블록의 길이 조정

                    Vector3 tempPosition = lastBlock.localPosition; // 블록의 위치를 저장
                    tempPosition.x = middle;
                    lastBlock.localPosition = lastPosition = tempPosition; // 블록의 위치를 중간값으로 설정

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

                    comboCount = 0; // 콤보 초기화
                }
                else
                {
                    ComboCheck(); // 콤보 체크
                    lastBlock.localPosition = prevBlockPosition + Vector3.up; // 한칸 위로
                }
            }
            else
            {
                float deltaZ = prevBlockPosition.z - lastPosition.z;
                bool isNegativeNum = (deltaZ < 0) ? true : false; // 음수인지 체크 (어긋난 길이의 방향을 체크하기 위함) Rubble이 떨어질 위치

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
                        ),// 러블의 위치
                        new Vector3(stackBounds.x, 1, deltaZ)
                        );// 러블의 사이즈

                    comboCount = 0; // 콤보 초기화
                }
                else
                {
                    ComboCheck(); // 콤보 체크
                    lastBlock.localPosition = prevBlockPosition + Vector3.up;
                }
            }

            secondaryPosition = (isMovingX) ? lastBlock.localPosition.x : lastBlock.localPosition.z; // 쌓인 블록의 위치를 저장

            return true; // 블록이 쌓일 수 있을때
        }

        void CreateRubble(Vector3 pos, Vector3 scale)
        {
            GameObject go = Instantiate(lastBlock.gameObject);
            go.transform.parent = this.transform; // TheStack의 하위 오브젝트로 설정

            go.transform.localPosition = pos; // 블록의 위치를 저장
            go.transform.localScale = scale; // 블록의 크기를 저장
            go.transform.localRotation = Quaternion.identity; // 회전 고정

            go.AddComponent<Rigidbody>(); // 물리엔진 추가
            go.name = "Rubble"; // 이름 변경

        }

        void ComboCheck()
        {
            comboCount++;

            if (comboCount > maxCombo)
                maxCombo = comboCount;

            if ((comboCount % 5) == 0)
            {
                Debug.Log("5 Combo Success!");
                stackBounds += new Vector3(0.5f, 0.5f); // 블록의 크기를 증가
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
            int childCount = this.transform.childCount; // 하위 오브젝트(블럭, 러블) 개수

            for (int i = 1; i < 20; i++) // 위 20개 오브젝트에 리지드바디 달고 힘을 가해 날려보낸다
            {
                if (childCount < i) break;

                GameObject go = transform.GetChild(childCount - i).gameObject;

                if (go.name == "Rubble") continue; // 러블은 제외

                Rigidbody rigidbody = go.AddComponent<Rigidbody>(); // 물리엔진 추가

                rigidbody.AddForce(
                    (Vector3.up * Random.Range(0, 10f) + Vector3.right * (Random.Range(0, 10f) - 5f)) * 100f
                    );
            }
        }

        public void Restart()
        {
            int childCount = transform.childCount; // 하위 오브젝트(블럭, 러블) 개수

            for (int i = 0; i < childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject); // 하위 오브젝트 삭제
            }

            isGameOver = false; // 게임오버 상태 해제

            lastBlock = null; // 마지막 블록 초기화
            desiredPosition = Vector3.zero;
            stackBounds = new Vector2(BoundSize, BoundSize); // 블록 크기 초기화

            stackCount = -1; // 블록 개수 초기화
            isMovingX = true; // x축으로 이동할지 y축으로 이동할지 결정
            blockTransition = 0f; // 이동에 대한 기준값 초기화
            secondaryPosition = 0f; // 쌓인 블록의 위치를 저장

            comboCount = 0; // 콤보 초기화
            maxCombo = 0; // 최대 콤보 초기화

            prevBlockPosition = Vector3.down; // y가 -1로 시작

            prevColor = GetRandomColor();
            nextColor = GetRandomColor();

            Spawn_Block(); // 첫 블록 생성
            Spawn_Block(); // 이동 블록 생성
        }
    }
}
