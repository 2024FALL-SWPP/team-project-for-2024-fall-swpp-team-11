using UnityEngine;

public interface IInputHandler
{
    Vector3 GetMoveInput();
    Quaternion GetViewRot();
    bool IsJumpRequested();
    bool IsInteractRequested();
}
