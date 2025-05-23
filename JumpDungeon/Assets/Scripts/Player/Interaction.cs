using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    public float maxCheckDistance;
    private float lastCheckTime;

    public LayerMask interactableLayerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptTxt;
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, interactableLayerMask))
            {
                if(hit.collider.gameObject != curInteractGameObject) 
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = curInteractGameObject.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else    // ºó°ø°£¿¡ Raycast
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptTxt.gameObject.SetActive(false);
            }
        }

    }
    private void SetPromptText()
    {
        promptTxt.gameObject.SetActive(true);
        promptTxt.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptTxt.gameObject.SetActive(false);
        }
    }
}
