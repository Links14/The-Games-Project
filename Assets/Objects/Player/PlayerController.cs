using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject MovePoint;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] PlayerInputActions PlayerControls;

    private InputAction move;
    private InputAction interact;
    private Vector3 moveDirection;
    private Vector3 roundedPos;
    private bool isMoving = false;


    private void Awake()
    {
        PlayerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        MovePoint.transform.parent = null;
        isMoving = false;

        move = PlayerControls.Player.Move;
        interact = PlayerControls.Player.Interact;
        move.Enable();
        interact.Enable();
    } 
    private void OnDisable()
    {
        move.Disable();
        interact.Disable();
    }

    private void FixedUpdate()
    {
        // Player Interact
        if (interact.ReadValue<bool>())
        {
            Debug.Log("Interact");
            StartCoroutine(Interact());
        }


        // Player Movement
        moveDirection = new Vector3(Mathf.Round(move.ReadValue<Vector2>().x), Mathf.Round(move.ReadValue<Vector2>().y), 0f);
        roundedPos = new Vector3(Mathf.Round(rb.transform.position.x), Mathf.Round(rb.transform.position.y), 0f);

        rb.transform.position = Vector2.MoveTowards(rb.transform.position, MovePoint.transform.position, moveSpeed * Time.fixedDeltaTime);


        if ((Vector3.Distance(rb.transform.position, MovePoint.transform.position) < 0.05f) && !isMoving)
        {
            isMoving = true;

            Vector3 newX = new Vector3(Mathf.Round(MovePoint.transform.position.x + moveDirection.x), Mathf.Round(MovePoint.transform.position.y), 0f);
            Vector3 newY = new Vector3(Mathf.Round(MovePoint.transform.position.x), Mathf.Round(MovePoint.transform.position.y + moveDirection.y), 0f);

            if (Mathf.Abs(moveDirection.x) > 0.5f && IsTraversable(newX))
                MovePoint.transform.position = newX;
            else if (Mathf.Abs(moveDirection.y) > 0.5f && IsTraversable(newY))
                MovePoint.transform.position = newY;
        }
        else if (Vector3.Distance(rb.transform.position, MovePoint.transform.position) < 0.05f)
        {
            isMoving = false;
        }
    }


    private bool IsTraversable(Vector3 _pos)
    {
        List<int> layers = GetLayerMasksAtPosition(_pos);

        foreach (int layer in layers)
        {
            Debug.Log($"Layer Detected: {LayerMask.LayerToName(layer)} ({layer})");
            if (LayerMask.LayerToName(layer) == "Collisions")
                return false;
        }

        return true;
    }

    private IEnumerator Interact()
    {
        //var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"), 0f);
        //colliderPoint.position = movePoint.position + facingDir;

        //var collider = Physics2D.OverlapCircle(colliderPoint.position, 0.2f, questLayer | interactables);
        //if (collider != null)
        //{
        //    animator.SetBool("isMoving", false);
        //    new WaitForEndOfFrame();
        //    collider.GetComponent<Interactable>()?.Interact(-1f * facingDir);
        //}
        //else
        //{
        //    // uses waterLayers - 
        //    // waterLayerBeach | waterLayerLake | waterLayerOcean | waterLayerRiver
        //    var c = Physics2D.OverlapCircle(colliderPoint.position, 0.2f, waterLayerBeach | waterLayerLake | waterLayerOcean | waterLayerRiver);

        //    if (c != null)
        //    {
        //        animator.SetBool("isMoving", false);
        //        new WaitForEndOfFrame();
        //        StartCoroutine(FishingManager.Instance.StartFishing(c.gameObject.tag));
        //    }
        //}

        yield return null;
    }

    private static List<int> GetLayerMasksAtPosition(Vector2 _pos)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(_pos);
        List<int> layerMasks = new List<int>();

        foreach (Collider2D col in colliders)
        {
            int layer = col.gameObject.layer;
            if (!layerMasks.Contains(layer))
                layerMasks.Add(layer);
        }

        return layerMasks;
    }
}

