using UnityEngine;
using UnityEngine.UI;

public class HudComponent : MonoBehaviour
{
    public Camera HudCamera;
    public Unit Owner;

    private Transform HudTransform;
    private Text m_Name;
    private Slider m_GreenHpBar;
    private Slider m_RedHpBar;

    public void SetOwner(Unit owner)
    {
        Owner = owner;
    }

    void Start()
    {
        HudCamera = CameraManager.Instance.HudCamera;
        GameObject hudGo = Instantiate(Resources.Load("HUD") as GameObject);
        ObjectCollection objectCollection = hudGo.GetComponent<ObjectCollection>();
        HudTransform = objectCollection.transform;
        HudTransform.SetParent(HudManager.Instance.HudRoot, false);
        HudTransform.name = "HUD_" + Owner.Name;
        m_Name = objectCollection.GetT<Text>("Name");
        m_GreenHpBar = objectCollection.GetT<Slider>("GreenHpBar");
        m_RedHpBar = objectCollection.GetT<Slider>("RedHpBar");
    }

    void LateUpdate()
    {
        Vector3 position = Owner.transform.position;
        position.y = 1.2f;
        HudTransform.position = position;
    }
}