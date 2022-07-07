using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private PlayerCharacter character;
    private PlayerCamera cam;

    protected override void Init()
    {
        base.Init();
    }
}
