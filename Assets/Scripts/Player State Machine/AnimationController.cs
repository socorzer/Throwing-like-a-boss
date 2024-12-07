using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    Coroutine jumpCoroutine;
    GameData data;
    float targetMoveX, targetMoveZ;
    float move;
    float smoothTimeDelta;

    // Start is called before the first frame update
    void Start()
    {
        data = GameData.Instance;
        smoothTimeDelta = data.Player.moveSmoothTime * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        float currentMoveX = Mathf.Lerp(animator.GetFloat("MoveX"), targetMoveX, smoothTimeDelta);
        float currentMoveZ = Mathf.Lerp(animator.GetFloat("MoveZ"), targetMoveZ, smoothTimeDelta);
        float velocity = Mathf.Lerp(animator.GetFloat("Velocity"), move, smoothTimeDelta);

        animator.SetFloat("MoveX", currentMoveX);
        animator.SetFloat("MoveZ", currentMoveZ);
        animator.SetFloat("Velocity", velocity);
    }

    /*public void Jump(bool isJump = true)
    {
        if (jumpCoroutine != null)
            StopCoroutine(jumpCoroutine);

        float targetJump = isJump ? 1f : 0f;
        jumpCoroutine = StartCoroutine(LerpJump(targetJump));
    }

    IEnumerator LerpJump(float target)
    {
        float currentJump = animator.GetFloat("Jump");

        while (Mathf.Abs(currentJump - target) > 0.001f)
        {
            currentJump = Mathf.Lerp(currentJump, target, smoothTimeDelta);
            animator.SetFloat("Jump", currentJump);
            yield return new WaitForFixedUpdate();
        }

        // Ensure the target jump value is set exactly
        animator.SetFloat("Jump", target);
    }*/

    public void Move(float x, float z, float move)
    {
        targetMoveX = x;
        targetMoveZ = z;
        this.move = move;
    }
    public void SetTired(float stamina)
    {
        animator.SetLayerWeight(1, -stamina + 0.5f);
    }
}
