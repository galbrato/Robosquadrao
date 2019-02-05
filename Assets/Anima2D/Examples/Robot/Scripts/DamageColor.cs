using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColor : MonoBehaviour
{
    private Renderer rend;

    [SerializeField]
    private Color colorToTurnTo = Color.white;
    // Start is called before the first frame update
    
    public void DamageFeedbackIn (){
        foreach(SpriteRenderer variableName in GetComponentsInChildren<SpriteRenderer>()){
           variableName.material.color = Color.red;
        }
    }

    public void DamageFeedbackOut(){
        foreach(SpriteRenderer variableName in GetComponentsInChildren<SpriteRenderer>()){
           variableName.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
