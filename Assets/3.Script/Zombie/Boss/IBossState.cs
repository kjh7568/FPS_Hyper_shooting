using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossState
{
    void EnterState(BossController boss);
    void UpdateState(BossController boss);
    void ExitState(BossController boss);
}
