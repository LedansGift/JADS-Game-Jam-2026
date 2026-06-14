using UnityEngine;

public class chargeAttackSample : MonoBehaviour
{
    public Animator anim;
    public float chargeTime;
    public bool charging;
    


    public void AttackInputFromButton() 
    {
        anim.SetTrigger("attack");
        charging = true;
    }

    public void ButtonRelease() 
    {
        if (charging) 
        {
            if (chargeTime > 1.5f)
            {
                anim.SetTrigger("chargeAttackStrong");
                chargeTime = 0;
            }
            else
            {
                anim.SetTrigger("chargeAttackWeak");
                chargeTime = 0;
            }
            charging = false;
            Debug.Log("button release");
        }
        
    }
    
    void ChargeUp() 
    
    {
        if (charging) 
        {
            anim.SetTrigger("chargeAttackReady");
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (charging) 
        {
            chargeTime += Time.deltaTime;
        }
      
        
    }
  
}
