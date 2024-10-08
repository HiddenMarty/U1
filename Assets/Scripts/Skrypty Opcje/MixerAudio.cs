using System.IO;
using UnityEngine;
using System;

public class MixerAudio : MonoBehaviour
{
    public static MixerAudio Instance { get; private set; }

    private float volume = 1f;  // Domy�lna warto�� g�o�no�ci (od 0 do 1)
    private const string VolumeFileName = "Test.txt";  // Nazwa pliku, w kt�rym zapisujemy g�o�no��

    public float Volume
    {
        get { return volume; }
        set
        {
            volume = Mathf.Clamp(value, 0f, 1f);
            Debug.Log("Volume set to: " + volume);

            try
            {
                // Zapisz aktualn� warto�� g�o�no�ci do pliku
                File.WriteAllText(VolumeFileName, volume.ToString());
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
            finally
            {
                Debug.Log("Executing finally block.");
            }

            UpdateVolume(); // Aktualizuj g�o�no�� w grze
        }
    }

    // Inicjalizacja singletona
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Wczytaj zapisane ustawienia d�wi�kowe przy starcie
        LoadVolume();
    }

    // Metoda do aktualizacji g�o�no�ci w grze
    private void UpdateVolume()
    {
        // Aktualizuj g�o�no�� globaln�, np. dla AudioListener
        AudioListener.volume = volume;
        Debug.Log("Global volume updated to: " + volume);
    }

    // Metoda do wczytywania g�o�no�ci z pliku
    public void LoadVolume()
    {
        if (File.Exists(VolumeFileName))
        {
            try
            {
                string volumeString = File.ReadAllText(VolumeFileName);

                if (float.TryParse(volumeString, out float loadedVolume))
                {
                    volume = Mathf.Clamp(loadedVolume, 0f, 1f);
                    Debug.Log("Volume loaded: " + volume);
                    UpdateVolume(); // Aktualizuj g�o�no�� po wczytaniu
                }
                else
                {
                    Debug.LogWarning("Nie uda�o si� przekonwertowa� warto�ci g�o�no�ci.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception podczas wczytywania g�o�no�ci: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Plik z g�o�no�ci� nie istnieje. U�ywana warto�� domy�lna: " + volume);
        }
    }
}
