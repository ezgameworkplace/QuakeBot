using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Ruby")
        {
            RubyController controller = other.GetComponent<RubyController>();
            if (controller != null)
            {
                controller.ChangeHealth(-1);
            }
        }
    }
}
