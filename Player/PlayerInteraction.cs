using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactables = new List<Interactable>();
    private Interactable closestInteractable;

    void Start()
    {
        Player player = GetComponent<Player>();
        player.controls.Character.Interaction.performed += ctx => InteractWithClosest();
    }

    public void InteractWithClosest()
    {
        closestInteractable?.Interaction();
        interactables.Remove(closestInteractable);

        UpdateClosestInteractable();
    }

    public void UpdateClosestInteractable()
    {
        closestInteractable?.HighlightActive(false);
        closestInteractable = null;

        float closestDistance = float.MaxValue;
        foreach (Interactable interact in interactables)
        {
            float distance = Vector3.Distance(transform.position, interact.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interact;
            }
        }

        closestInteractable?.HighlightActive(true);
    }

    public List<Interactable> GetInteractables() => interactables;

}
