using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WrapAround : MonoBehaviour
{
    private GameObject obj;
    public GameObject[] objArr;
    protected Transform currObject;
    // protected GameObject[] clones; // Clockwise starting from upper left
    protected Transform[] ghosts;

    private float screenWidth;
    private float screenHeight;

    private bool revealed = true;
    private float bufferVal = 1f;

    void Awake()
    {
        obj = objArr[Random.Range(0,objArr.Length)];
        currObject = Instantiate(obj, transform).transform;
        var cam = Camera.main;
 
        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));
 
        screenWidth = screenTopRight.x - screenBottomLeft.x + bufferVal;
        screenHeight = screenTopRight.y - screenBottomLeft.y + bufferVal;

        ghosts = new Transform[8];
    }

    private void Start() {
        CreateGhosts();
        OnStart();
    }

    protected void CreateGhosts()
    {
        for(int i = 0; i < 8; i++)
        {
            ghosts[i] = Instantiate(obj, Vector3.zero, Quaternion.identity).transform;
            ghosts[i].parent = transform;
            DestroyImmediate(ghosts[i].GetComponent<WrapAround>());
        }

        PositionGhosts();
    }

    protected void PositionGhosts()
    {
        // All ghost positions will be relative to the ships (this) transform,
        // so let's star with that.
        var ghostPosition = currObject.position;
    
        // We're positioning the ghosts clockwise behind the edges of the screen.
        // Let's start with the far right.
        ghostPosition.x = currObject.position.x + screenWidth;
        ghostPosition.y = currObject.position.y;
        ghosts[0].position = ghostPosition;
    
        // Bottom-right
        ghostPosition.x = currObject.position.x + screenWidth;
        ghostPosition.y = currObject.position.y - screenHeight;
        ghosts[1].position = ghostPosition;
    
        // Bottom
        ghostPosition.x = currObject.position.x;
        ghostPosition.y = currObject.position.y - screenHeight;
        ghosts[2].position = ghostPosition;
    
        // Bottom-left
        ghostPosition.x = currObject.position.x - screenWidth;
        ghostPosition.y = currObject.position.y - screenHeight;
        ghosts[3].position = ghostPosition;
    
        // Left
        ghostPosition.x = currObject.position.x - screenWidth;
        ghostPosition.y = currObject.position.y;
        ghosts[4].position = ghostPosition;
    
        // Top-left
        ghostPosition.x = currObject.position.x - screenWidth;
        ghostPosition.y = currObject.position.y + screenHeight;
        ghosts[5].position = ghostPosition;
    
        // Top
        ghostPosition.x = currObject.position.x;
        ghostPosition.y = currObject.position.y + screenHeight;
        ghosts[6].position = ghostPosition;
    
        // Top-right
        ghostPosition.x = currObject.position.x + screenWidth;
        ghostPosition.y = currObject.position.y + screenHeight;
        ghosts[7].position = ghostPosition;
    
        // All ghost ships should have the same rotation as the main ship
        for(int i = 0; i < 8; i++)
        {
            ghosts[i].rotation = currObject.rotation;
        }
    }

    protected void SwapGhosts()
    {
        foreach(var ghost in ghosts)
        {
            if (ghost.position.x < screenWidth / 2 && ghost.position.x > -screenWidth / 2 &&
                ghost.position.y < screenHeight / 2 && ghost.position.y > -screenHeight / 2)
            {
                currObject.position = ghost.position;

                if (!revealed) {
                    Debug.Log("REVEALLL");
                    Revealed(true);
                }
                // Debug.Log("THIS IS WEIRD");
    
                break;
            }
        }
    
        PositionGhosts();
    }

    void Update() {
        OnUpdate();
    }

    void FixedUpdate() {
        OnFixedUpdate();
        SwapGhosts();
    }

    protected void DestroyAll() {
        for (int i = 0; i < 8; i++)
        {
            Destroy(ghosts[i].gameObject);
        }
    }

    protected void Revealed(bool yes) {
        revealed = yes;
        currObject.GetComponent<SpriteRenderer>().enabled = yes;
        currObject.GetComponent<Collider2D>().enabled = yes;
        Debug.Log("weehaw");
    }

    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void OnFixedUpdate();

}