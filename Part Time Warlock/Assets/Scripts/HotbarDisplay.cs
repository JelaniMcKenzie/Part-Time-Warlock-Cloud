using System.Diagnostics;
using UnityEngine.InputSystem;

public class HotbarDisplay : StaticInventoryDisplay
{
    private Player p;
    private int _maxIndexSize = 9;
    private int _currentIndex = 0;

    private PlayerInputActions _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerInputActions();
    }

    protected override void Start()
    {
        base.Start();
        p = FindAnyObjectByType<Player>();
        _currentIndex = 0;
        _maxIndexSize = slots.Length - 1;
        
        //slots[_currentIndex].ToggleHighlight();
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        _playerControls.Enable();

        _playerControls.Player.Spell1.performed += Spell1;
        _playerControls.Player.Spell2.performed += Spell2;
        _playerControls.Player.Spell3.performed += Spell3;
        _playerControls.Player.Spell4.performed += Spell4;
        _playerControls.Player.ConsumableItem.performed += ConsumableItem;
        
    }

    

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerControls.Disable();

        _playerControls.Player.Spell1.performed += Spell1;
        _playerControls.Player.Spell2.performed += Spell2;
        _playerControls.Player.Spell3.performed += Spell3;
        _playerControls.Player.Spell4.performed += Spell4;
        _playerControls.Player.ConsumableItem.performed += ConsumableItem;

    }

    #region Hotbar Select Methods

    private void Spell1(InputAction.CallbackContext obj)
    {
        SetIndex(0);

        if (slots[_currentIndex].AssignedInventorySlot.Item is SpellClass)
        {
            slots[_currentIndex].AssignedInventorySlot.Item.Use(p);
        }
        
        
    }
    
    private void Spell2(InputAction.CallbackContext obj)
    {
        SetIndex(1);
        if (slots[_currentIndex].AssignedInventorySlot.Item is SpellClass)
        {
            slots[_currentIndex].AssignedInventorySlot.Item.GetSpell().Use(p);
        }
    }
    
    private void Spell3(InputAction.CallbackContext obj)
    {
        SetIndex(2);
        if (slots[_currentIndex].AssignedInventorySlot.Item is SpellClass)
        {
            slots[_currentIndex].AssignedInventorySlot.Item.GetSpell().Use(p);
        }
    }
    
    private void Spell4(InputAction.CallbackContext obj)
    {
        SetIndex(3);
        if (slots[_currentIndex].AssignedInventorySlot.Item is SpellClass)
        {
            slots[_currentIndex].AssignedInventorySlot.Item.GetSpell().Use(p);
        }
    }
    
    private void ConsumableItem(InputAction.CallbackContext obj)
    {
        SetIndex(4);
        if (slots[_currentIndex].AssignedInventorySlot.Item is ConsumableClass)
        {
            slots[_currentIndex].AssignedInventorySlot.Item.GetConsumable().Use(p);
        }
    }
    
    #endregion

    private void Update()
    {
        //if (_playerControls.Player.MouseWheel.ReadValue<float>() > 0.1f) ChangeIndex(1);
        //if (_playerControls.Player.MouseWheel.ReadValue<float>() < -0.1f) ChangeIndex(-1);
        for (int i = 0; i < _maxIndexSize; i++)
        {
            if (slots[_currentIndex].AssignedInventorySlot.Item is SpellClass spell)
            {
                spell.UpdateCooldown();
            }
        }
        
    }
    
    private void UseItem(InputAction.CallbackContext obj)
    {
        //(slots[_currentIndex].AssignedInventorySlot.ItemData != null) slots[_currentIndex].AssignedInventorySlot.ItemData.UseItem();
    }

    private void ChangeIndex(int direction)
    {
        //slots[_currentIndex].ToggleHighlight();
        _currentIndex += direction;

        if (_currentIndex > _maxIndexSize) _currentIndex = 0;
        if (_currentIndex < 0) _currentIndex = _maxIndexSize;
        
        //slots[_currentIndex].ToggleHighlight();
    }

    private void SetIndex(int newIndex)
    {
        //slots[_currentIndex].ToggleHighlight();
        if (newIndex < 0) _currentIndex = 0;
        if (newIndex > _maxIndexSize) _currentIndex = _maxIndexSize;
        
        _currentIndex = newIndex;
        //slots[_currentIndex].ToggleHighlight();
    }
}
