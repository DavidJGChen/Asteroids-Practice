using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WrapAround : MonoBehaviour
{
    private GameObject obj; // Prefab to clone
    public GameObject[] objArr; // Set of Prefabs to clone-- chosen randomly
    protected Transform currObject; // The only actual object
    protected Transform[] ghosts; // Ghosts

    private float screenWidth;
    private float screenHeight;

    private bool revealed = true;
    private float bufferVal = 1f;

    #region Initialization
    protected abstract void OnAwake();
    protected abstract void OnStart();
    void Awake() {
        // Pick and instantiate a random object

        var cam = Camera.main;
 
        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));
 
        screenWidth = screenTopRight.x - screenBottomLeft.x + bufferVal;
        screenHeight = screenTopRight.y - screenBottomLeft.y + bufferVal;

        ghosts = new Transform[8];
        OnAwake();
    }

    private void Start() {
        obj = objArr[Random.Range(0,objArr.Length)];
        currObject = Instantiate(obj, transform).transform;
        CreateGhosts();
        OnStart();
    }

    private void CreateGhosts() {
        for(int i = 0; i < 8; i++) {
            ghosts[i] = Instantiate(obj, Vector3.zero, Quaternion.identity).transform;
            ghosts[i].parent = this.transform;
            Destroy(ghosts[i].GetComponent<Rigidbody2D>());
            Destroy(ghosts[i].GetComponent<Collider2D>());
        }
        PositionGhosts();
    }
    #endregion

    protected void PositionGhosts() {
        var ghostPosition = currObject.position;

        // Right
        ghostPosition.x = currObject.position.x + screenWidth;
        ghostPosition.y = currObject.position.y;
        ghosts[0].position = ghostPosition;
    
        // Bottom Right
        ghostPosition.x = currObject.position.x + screenWidth;
        ghostPosition.y = currObject.position.y - screenHeight;
        ghosts[1].position = ghostPosition;
    
        // Bottom
        ghostPosition.x = currObject.position.x;
        ghostPosition.y = currObject.position.y - screenHeight;
        ghosts[2].position = ghostPosition;
    
        // Bottom Left
        ghostPosition.x = currObject.position.x - screenWidth;
        ghostPosition.y = currObject.position.y - screenHeight;
        ghosts[3].position = ghostPosition;
    
        // Left
        ghostPosition.x = currObject.position.x - screenWidth;
        ghostPosition.y = currObject.position.y;
        ghosts[4].position = ghostPosition;
    
        // Top Left
        ghostPosition.x = currObject.position.x - screenWidth;
        ghostPosition.y = currObject.position.y + screenHeight;
        ghosts[5].position = ghostPosition;
    
        // Top
        ghostPosition.x = currObject.position.x;
        ghostPosition.y = currObject.position.y + screenHeight;
        ghosts[6].position = ghostPosition;
    
        // Top Right
        ghostPosition.x = currObject.position.x + screenWidth;
        ghostPosition.y = currObject.position.y + screenHeight;
        ghosts[7].position = ghostPosition;
    
        // All ghost ships should have the same rotation as the main ship
        for(int i = 0; i < 8; i++)
        {
            ghosts[i].rotation = currObject.rotation;
        }
    }

    private void SwapGhosts() {
        for(int i = 0; i < 8; i++) {
            var ghost = ghosts[i];

            if (ghost.position.x < screenWidth / 2 &&
                ghost.position.x > -screenWidth / 2 &&
                ghost.position.y < screenHeight / 2 &&
                ghost.position.y > -screenHeight / 2) {
                
                currObject.position = ghost.position;

                if (!revealed) SetReveal(true);

                PositionGhosts();
    
                break;
            }
        }
    }
    protected abstract void OnUpdate();
    protected abstract void OnFixedUpdate();

    void Update() {
        OnUpdate();
    }

    void FixedUpdate() {
        PositionGhosts();
        SwapGhosts();
        OnFixedUpdate();
    }

    protected void DestroyAll() {
        for (int i = 0; i < 8; i++) {
            Destroy(ghosts[i].gameObject);
        }
    }

    protected void SetReveal(bool value) {
        revealed = value;
        currObject.GetComponent<SpriteRenderer>().enabled = value;
        currObject.GetComponent<Collider2D>().enabled = value;
    }
}