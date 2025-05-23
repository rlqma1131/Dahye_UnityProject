using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

namespace TheStack
{
    public abstract class BaseUI : MonoBehaviour // ���
    {
        protected UIManager UIManager; // UIManager�� ��ӹ��� Ŭ������ �ν��Ͻ�

        public virtual void Init(UIManager uiManager) // UIManager�� ���ڷ� �޴� Init �޼���
        {
            this.UIManager = uiManager; // ���ڷ� ���� UIManager�� Ŭ������ UIManager�� ����
        }

        protected abstract UIState GetUIState();
        public void SetActive(UIState state)
        {
            gameObject.SetActive(GetUIState() == state); // ���� UI�� ���¿� ���ڷ� ���� ���°� ������ UI�� Ȱ��ȭ
        }
    }
}
