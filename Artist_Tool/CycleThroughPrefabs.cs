using UnityEngine;

public class CycleThroughPrefabs : MonoBehaviour
{
    [Header("Parent containing prefab instances")]
    public Transform parent; // Parent containing the prefab instances

    [Header("Animator Controller to Assign")]
    public RuntimeAnimatorController animatorController; // Animator Controller to assign to children

    [Header("Animator Trigger Name")]
    public string animatorTrigger = "Show"; // Trigger name for Animator

    private int currentIndex = 0; // Tracks the currently active child

    private void Start()
    {
        if (parent == null)
        {
            Debug.LogError("Parent is not assigned.");
            return;
        }

        // Assign the Animator Controller to all children and ensure only the first child is active at the start
        AssignAnimatorControllerToChildren();
        ShowChildAtIndex(0);
    }

    public void ShowNext()
    {
        if (parent == null || parent.childCount == 0)
        {
            Debug.LogError("Parent is either null or has no children.");
            return;
        }

        // Deactivate the current child
        parent.GetChild(currentIndex).gameObject.SetActive(false);

        // Calculate the next index (wrap around if needed)
        currentIndex = (currentIndex + 1) % parent.childCount;

        // Show the next child
        ShowChildAtIndex(currentIndex);
    }

    private void ShowChildAtIndex(int index)
    {
        if (parent == null) return;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            bool isActive = i == index;

            // Activate/deactivate the child
            child.gameObject.SetActive(isActive);

            if (isActive)
            {
                // Attempt to trigger animation on the active child
                Animator animator = child.GetComponent<Animator>();
                if (animator != null && !string.IsNullOrEmpty(animatorTrigger))
                {
                    animator.SetTrigger(animatorTrigger);
                }
            }
        }
    }

    private void AssignAnimatorControllerToChildren()
    {
        if (animatorController == null) return;

        foreach (Transform child in parent)
        {
            Animator animator = child.GetComponent<Animator>();
            if (animator == null)
            {
                animator = child.gameObject.AddComponent<Animator>();
            }

            animator.runtimeAnimatorController = animatorController;
        }
    }
}
