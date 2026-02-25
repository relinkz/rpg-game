using System;
using Godot;

public partial class HealthBar : Node2D
{
    private ProgressBar _progressBar;
    private int _maxHealth;

    public void Initialize(int maxHealth, Sprite2D sprite)
    {
        _maxHealth = maxHealth;

        // Create ProgressBar node if it doesn't exist
        if (_progressBar == null)
        {
            _progressBar = new ProgressBar();
            AddChild(_progressBar);
        }

        _progressBar.MaxValue = maxHealth;
        _progressBar.Value = maxHealth;

        // Set progress bar width to match sprite and position it
        if (sprite != null && sprite.Texture != null)
        {
            float spriteWidth = sprite.Texture.GetWidth();
            float spriteHeight = sprite.Texture.GetHeight();
            _progressBar.CustomMinimumSize = new Vector2(spriteWidth, 20);  // 20 is a reasonable height

            // Position the progress bar above the sprite
            _progressBar.Position = new Vector2(-spriteWidth / 2, -spriteHeight / 2 - 20);
            _progressBar.Size = new Vector2(spriteWidth, 20);
            _progressBar.ShowPercentage = false;  // Hide percentage text
        }

        UpdateColor(maxHealth);
    }

    public void UpdateHealth(int currentHealth)
    {
        if (_progressBar != null)
        {
            _progressBar.Value = currentHealth;
            UpdateColor(currentHealth);
        }
    }

    private void UpdateColor(int currentHealth)
    {
        if (_progressBar == null) return;

        float healthPercent = (float)currentHealth / (float)_maxHealth * 100f;
        Color barColor;

        if (healthPercent > 60f)
        {
            barColor = new Color(0.2f, 0.8f, 0.2f);  // Green
        }
        else if (healthPercent > 20f)
        {
            barColor = new Color(0.8f, 0.8f, 0.2f);  // Yellow
        }
        else
        {
            barColor = new Color(0.8f, 0.2f, 0.2f);  // Red
        }

        // Apply color using Modulate as a fallback
        _progressBar.Modulate = barColor;
        GD.Print($"Health: {currentHealth}/{_maxHealth} ({healthPercent:F1}%) - Color: {barColor}");
    }
}
