using TMPro;
using UnityEngine;

public class SpawnerStatsView : MonoBehaviour
{
    [SerializeField] private ObjectSpawnerBase _spawner;
    [SerializeField] private TextMeshProUGUI _spawnedText;
    [SerializeField] private TextMeshProUGUI _createdText;
    [SerializeField] private TextMeshProUGUI _activeText;

    private void OnEnable()
    {
        if (_spawner == null)
        {
            Debug.LogError($"{name}: не назначен Spawner в SpawnerStatsView");
            return;
        }

        _spawner.StatsChanged += OnStatsChanged;
        OnStatsChanged();
    }

    private void OnDisable()
    {
        if (_spawner != null)
            _spawner.StatsChanged -= OnStatsChanged;
    }

    private void OnStatsChanged()
    {
        if (_spawnedText != null)
            _spawnedText.text = $"Заспавнено: {_spawner.TotalSpawned}";

        if (_createdText != null)
            _createdText.text = $"Создано: {_spawner.TotalCreated}";

        if (_activeText != null)
            _activeText.text = $"Активно на сцене: {_spawner.TotalActive}";
    }
}