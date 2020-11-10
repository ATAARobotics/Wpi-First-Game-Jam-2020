using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActive : MonoBehaviour
{

    protected bool state = false;

    public void Toggle(bool state) {
        this.state = state;
    }
}
