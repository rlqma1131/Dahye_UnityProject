using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

namespace TheStack
{
    public abstract class BaseUI : MonoBehaviour // 상속
    {
        protected UIManager UIManager; // UIManager를 상속받은 클래스의 인스턴스

        public virtual void Init(UIManager uiManager) // UIManager를 인자로 받는 Init 메서드
        {
            this.UIManager = uiManager; // 인자로 받은 UIManager를 클래스의 UIManager에 저장
        }

        protected abstract UIState GetUIState();
        public void SetActive(UIState state)
        {
            gameObject.SetActive(GetUIState() == state); // 현재 UI의 상태와 인자로 받은 상태가 같으면 UI를 활성화
        }
    }
}
