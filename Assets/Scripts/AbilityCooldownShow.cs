using TMPro;
using UnityEngine;

public class AbilityCooldownShow : MonoBehaviour
{
    [SerializeField] private TMP_Text _abilityText;
    [SerializeField] private string _stringAbilityName;
    [SerializeField] private Player _player;

    private string _abilityName = "";

    private void Start()
    {
        string[] wordsOfAbilityName = _stringAbilityName.Split(' ');

        foreach (string word in wordsOfAbilityName)
            _abilityName += word + "\n";
    }

    private void Update()
    {
        if (_player.CurrentVampireAbilityCooldown >= _player.VampireAbilityCooldown)
            _abilityText.text = _abilityName + "ready";
        else
            _abilityText.text = _abilityName + Mathf.Round(_player.VampireAbilityCooldown - _player.CurrentVampireAbilityCooldown);
    }
}
