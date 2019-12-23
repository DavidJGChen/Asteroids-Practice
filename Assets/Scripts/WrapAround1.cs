using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class WrapAround1 : MonoBehaviour {
    private float screenWidth;
    private float screenHeight;
    private const float bufferVal = 1f;
    private SpriteRenderer spriteRenderer;
    public GameObject ghostPrefab;

    private Transform[] ghostTransforms;
    private SpriteRenderer[] ghostRenderers;
    private void Awake() {
        ghostTransforms = new Transform[8];
        ghostRenderers = new SpriteRenderer[8];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start() {
        RecalculateCameraBounds();    
    }
    private void FixedUpdate() {
        PositionGhosts();
        SwapPositions();
    }

    private void LateUpdate() {
        UpdateSprites();    
    }
    #region Public Methods
    public void RecalculateCameraBounds() {
        var cam = Camera.main;
 
        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));
 
        screenWidth = screenTopRight.x - screenBottomLeft.x + bufferVal;
        screenHeight = screenTopRight.y - screenBottomLeft.y + bufferVal;

        PositionGhosts();
    }
    public void CreateGhosts() {
        for(int i = 0; i < 8; i++) {
            var obj = Instantiate(ghostPrefab, Vector3.zero, Quaternion.identity);
            ghostTransforms[i] = obj.transform;
            ghostTransforms[i].parent = this.transform;
            ghostRenderers[i] = obj.GetComponent<SpriteRenderer>();
        }
        PositionGhosts();
        UpdateSprites();
    }
    public void DestroyGhosts() {
        for (int i = 0; i < 8; i++) {
            Destroy(ghostTransforms[i].gameObject);
        }
    }
    #endregion

    private void PositionGhosts() {
        var ghostPosition = this.transform.position;

        // Right
        ghostPosition.x = this.transform.position.x + screenWidth;
        ghostPosition.y = this.transform.position.y;
        ghostTransforms[0].position = ghostPosition;
    
        // Bottom Right
        ghostPosition.x = this.transform.position.x + screenWidth;
        ghostPosition.y = this.transform.position.y - screenHeight;
        ghostTransforms[1].position = ghostPosition;
    
        // Bottom
        ghostPosition.x = this.transform.position.x;
        ghostPosition.y = this.transform.position.y - screenHeight;
        ghostTransforms[2].position = ghostPosition;
    
        // Bottom Left
        ghostPosition.x = this.transform.position.x - screenWidth;
        ghostPosition.y = this.transform.position.y - screenHeight;
        ghostTransforms[3].position = ghostPosition;
    
        // Left
        ghostPosition.x = this.transform.position.x - screenWidth;
        ghostPosition.y = this.transform.position.y;
        ghostTransforms[4].position = ghostPosition;
    
        // Top Left
        ghostPosition.x = this.transform.position.x - screenWidth;
        ghostPosition.y = this.transform.position.y + screenHeight;
        ghostTransforms[5].position = ghostPosition;
    
        // Top
        ghostPosition.x = this.transform.position.x;
        ghostPosition.y = this.transform.position.y + screenHeight;
        ghostTransforms[6].position = ghostPosition;
    
        // Top Right
        ghostPosition.x = this.transform.position.x + screenWidth;
        ghostPosition.y = this.transform.position.y + screenHeight;
        ghostTransforms[7].position = ghostPosition;
    
        // All ghosts should have the same rotation
        for(int i = 0; i < 8; i++) {
            ghostTransforms[i].rotation = this.transform.rotation;
        }
    }

    private void SwapPositions() {
        for(int i = 0; i < 8; i++) {
            var ghost = ghostTransforms[i];

            if (ghost.position.x < screenWidth / 2 &&
                ghost.position.x > -screenWidth / 2 &&
                ghost.position.y < screenHeight / 2 &&
                ghost.position.y > -screenHeight / 2) {
                
                this.transform.position = ghost.position;

                PositionGhosts();
    
                break;
            }
        }
    }

    private void UpdateSprites() {
        for(int i = 0; i < 8; i++) {
            ghostRenderers[i].sprite = spriteRenderer.sprite;
            ghostRenderers[i].color = spriteRenderer.color;
        }
    }
}
}