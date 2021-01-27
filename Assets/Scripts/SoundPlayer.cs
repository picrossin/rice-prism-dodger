using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool loopClipForever;

    private SoundPlayer _instance;
    private AudioSource _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = clip;
        
        if (loopClipForever)
        {
            _audioSource.loop = true;
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        _audioSource.Play();
    }
}
