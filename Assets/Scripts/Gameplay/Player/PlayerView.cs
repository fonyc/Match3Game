using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [Header(" --- PLAYER ---")]
    [Space(5)]
    [SerializeField] private Image _heroImage;
    [SerializeField] private TMP_Text _hpText = null;
    [SerializeField] private Image _hpFill;
    [Header(" --- ITEMS ---")]
    [Space(5)]
    [SerializeField] private Image _item1;
    [SerializeField] private Image _item2;
    [SerializeField] private TMP_Text _item1Qty = null;
    [SerializeField] private TMP_Text _item2Qty = null;
    [Header(" --- SKILL ---")]
    [Space(5)]
    [SerializeField] private Image skill;
    [SerializeField] private Image _manaFill;
    [SerializeField] private TMP_Text _manaText = null;

    private UserData _userData;
    private PlayerController _controller;

    public void Initialize(PlayerController controller, UserData userData)
    {
        _controller = controller;
        _userData = userData;
    }

    public void SetData(HeroItemModel heroSelected, UserData userData)
    {

    }
}
