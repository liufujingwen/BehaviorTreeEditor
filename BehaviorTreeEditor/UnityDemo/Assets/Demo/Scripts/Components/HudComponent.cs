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

        m_Name.text = Owner.Name;
    }

    void LateUpdate()
    {
        Vector3 position = Owner.transform.position;
        position.y = 1.2f;
        HudTransform.position = position;
        HudTransform.rotation = HudCamera.transform.rotation;

        int hp = Owner.GetAttr(AttrType.Hp);
        int hpMax = Owner.GetAttr(AttrType.HpMax);

        if (hp == 0)
        {
            if (HudTransform.gameObject.activeSelf)
                HudTransform.gameObject.SetActive(false);
        }
        else
        {
            if (!HudTransform.gameObject.activeSelf)
                HudTransform.gameObject.SetActive(true);

            //if (hp != hpMax)
            {
                if (Owner.UnitType == EUnitType.Enemy)
                {
                    if (!m_RedHpBar.gameObject.activeSelf)
                    {
                        m_RedHpBar.gameObject.SetActive(true);
                    }

                    m_RedHpBar.value = (float)hp / hpMax;
                }
                else
                {
                    if (!m_GreenHpBar.gameObject.activeSelf)
                    {
                        m_GreenHpBar.gameObject.SetActive(true);
                    }

                    m_GreenHpBar.value = (float)hp / hpMax;
                }
            }
        }
    }
}