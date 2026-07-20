using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class ItemSelector : MonoBehaviour
{
    public ScalePan rightPan;
    public ScalePan leftPan;
    public TMP_Text hoverInfoText;

    [Header("Tuning")]
    public float maxRayDistance = 100f;
    public float selectedScaleMultiplier = 1.4f;
    public float dropHeight = 0.2f;

    [Header("Sound")]
    public AudioClip hoverSound;
    public AudioClip placeSound;
    public AudioClip creakSound;

    Camera cam;
    AudioSource audioSource;
    DraggableItem selected;
    Vector3 selectedOriginalScale;

    void Awake()
    {
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateHoverSelection();

        if (Keyboard.current.eKey.wasPressedThisFrame)
            TogglePlacement(rightPan);

        if (Keyboard.current.fKey.wasPressedThisFrame)
            TogglePlacement(leftPan);
    }

    void UpdateHoverSelection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance))
        {
            var item = hit.collider.GetComponentInParent<DraggableItem>();
            if (item != null)
            {
                Select(item);
                return;
            }
        }

        Deselect();
    }

    void Select(DraggableItem item)
    {
        if (selected == item)
            return;

        Deselect();

        selected = item;
        selectedOriginalScale = selected.transform.localScale;
        selected.transform.localScale = selectedOriginalScale * selectedScaleMultiplier;

        if (hoverInfoText != null)
        {
            var weighable = selected.GetComponent<WeighableItem>();
            hoverInfoText.text = $"{weighable.displayName}: {weighable.weightGrams:F0}g";
        }

        PlaySound(hoverSound);
    }

    void Deselect()
    {
        if (selected != null)
            selected.transform.localScale = selectedOriginalScale;

        selected = null;

        if (hoverInfoText != null)
            hoverInfoText.text = "";
    }

    void TogglePlacement(ScalePan targetPan)
    {
        if (selected == null || targetPan == null)
            return;

        var weighable = selected.GetComponent<WeighableItem>();

        if (weighable.currentPan != null)
        {
            weighable.currentPan.Release(weighable);
            Destroy(selected.gameObject);
            selected = null;

            if (hoverInfoText != null)
                hoverInfoText.text = "";

            PlaySound(placeSound);
            PlaySound(creakSound);
        }
        else
        {
            var slot = targetPan.Reserve(weighable);
            if (slot == null)
                return;

            selected.transform.localScale = selectedOriginalScale;

            var rb = selected.GetComponent<Rigidbody>();
            rb.position = slot.position + Vector3.up * dropHeight;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            selected.NotifyPickedUp();
            selected = null;

            if (hoverInfoText != null)
                hoverInfoText.text = "";

            PlaySound(placeSound);
            PlaySound(creakSound);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
