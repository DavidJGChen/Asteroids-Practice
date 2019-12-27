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

    private Transform[] ghostTransforms = new Transform[8];
    private SpriteRenderer[] ghostRenderers = new SpriteRenderer[8];
    private bool ghostsCreated = false;
    private bool revealed;
    private void Awake() {
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
    public void CreateGhosts(bool revealObject = true) {

        var container = GhostContainer.SharedInstance;

        for(int i = 0; i < 8; i++) {
            var obj = Instantiate(ghostPrefab, Vector3.zero, Quaternion.identity);
            ghostTransforms[i] = obj.transform;
            ghostTransforms[i].parent = container.transform;
            ghostRenderers[i] = obj.GetComponent<SpriteRenderer>();
        }

        ghostsCreated = true;
        revealed = revealObject;
        if (!revealObject) HideObject();

        PositionGhosts();
    }
    public void DestroyGhosts() {
        if (!ghostsCreated) return;

        for (int i = 0; i < 8; i++) {
            Destroy(ghostTransforms[i].gameObject);
        }

        ghostsCreated = false;
    }
    public void HideObject() {
        SetReveal(false);
    } 
    #endregion

    private void PositionGhosts() {
        if (!ghostsCreated) return;

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
        if (!ghostsCreated) return;

        for(int i = 0; i < 8; i++) {
            var ghost = ghostTransforms[i];

            if (ghost.position.x < screenWidth / 2 &&
                ghost.position.x > -screenWidth / 2 &&
                ghost.position.y < screenHeight / 2 &&
                ghost.position.y > -screenHeight / 2) {
                
                this.transform.position = ghost.position;

                if (!revealed) SetReveal(true);

                PositionGhosts();
    
                break;
            }
        }
    }

    private void UpdateSprites() {
        if (!ghostsCreated) return;

        for(int i = 0; i < 8; i++) {
            ghostRenderers[i].sprite = spriteRenderer.sprite;
            ghostRenderers[i].color = spriteRenderer.color;
        }
    }

    private void OnDisable() {
        if (!ghostsCreated) return;

        for (int i = 0; i < 8; i++) {
            if (ghostTransforms[i] != null)
                ghostTransforms[i].gameObject.SetActive(false);
        }
    }
    private void OnEnable() {
        if (!ghostsCreated) return;

        for (int i = 0; i < 8; i++) {
            ghostTransforms[i].gameObject.SetActive(true);
        }
    }

    private void SetReveal(bool value) {
        revealed = value;
        spriteRenderer.enabled = value;
        GetComponent<Collider2D>().enabled = value;
    }
}
}